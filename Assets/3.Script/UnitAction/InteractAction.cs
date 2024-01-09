using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractAction : BaseAction
{
    [SerializeField] private Sprite sprite;
    private int maxDistance = 1;

    public event EventHandler OnInteract;

    private Vector3 target;

    private void Update()
    {
        if (!isActive) return;

        Vector3 aimDirection = (target - unit.GetWorldPosition()).normalized;
        float rotationSpeed = 8f;
        transform.forward = Vector3.Slerp(transform.forward, aimDirection, rotationSpeed * Time.deltaTime);
    }




    public override Sprite GetActionImage()
    {
        return sprite;
    }

    public override string GetActionName()
    {
        return "Interact";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 0
        };
    }

    public override List<GridPosition> GetValidGridPostionList()
    {
        List<GridPosition> validGridPostionList = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPostion();

        for (int x = -maxDistance; x <= maxDistance; x++)
        {
            for (int z = -maxDistance; z <= maxDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                if (!LevelGrid.Instance.isValidGridPosition(testGridPosition))
                {
                    //그리드 안에서만 움직이게끔
                    continue;
                }
                Door door = LevelGrid.Instance.GetDoorAtGridPosition(testGridPosition);
                DestructibleCrate crate = LevelGrid.Instance.GetCrateAtGridPosition(testGridPosition);

                if(door == null && crate == null)
                {
                    //해당 그리드에 문이 없다면?
                    continue;
                }

                validGridPostionList.Add(testGridPosition);
            }
        }
        return validGridPostionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        if (unit.isDie) return;
        target = LevelGrid.Instance.GetWorldPosition(gridPosition);

        Door door = LevelGrid.Instance.GetDoorAtGridPosition(gridPosition);
        DestructibleCrate crate = LevelGrid.Instance.GetCrateAtGridPosition(gridPosition);
        door?.Interact(OnInteractComplete);
        crate?.Interact(OnInteractComplete);
        OnInteract?.Invoke(this, EventArgs.Empty);
        ActionStart(onActionComplete);
    }

    private void OnInteractComplete()
    {
        ActionComplete();
    }

    public override int GetMaxDistance()
    {
        return maxDistance;
    }
}
