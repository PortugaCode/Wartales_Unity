using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallAction : BaseAction
{

    public event EventHandler OnShootingFireBall;



    private Vector3 targetVector;

    [SerializeField] private Sprite sprite;

    [Header("Shoot Distance")]
    [SerializeField] private int maxShootDistance;

    [Header("FireBallPrefab")]
    [SerializeField] private Transform fireBallPrefab;

    [Header("Damage")]
    [SerializeField] private int damage;

    [Header("Pivot")]
    [SerializeField] private Transform pivot;


    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        Vector3 aimDirection = (targetVector - unit.GetWorldPosition()).normalized;
        float rotationSpeed = 8f;
        transform.forward = Vector3.Slerp(transform.forward, aimDirection, rotationSpeed * Time.deltaTime);
    }


    public override Sprite GetActionImage()
    {
        return sprite;
    }

    public override string GetActionName()
    {
        return "FireBall";
    }

    public override int GetActionPointCost()
    {
        return 2;
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

        for (int x = -maxShootDistance; x <= maxShootDistance; x++)
        {
            for (int z = -maxShootDistance; z <= maxShootDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                if (!LevelGrid.Instance.isValidGridPosition(testGridPosition))
                {
                    //그리드 안에서만 움직이게끔
                    continue;
                }

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > maxShootDistance)
                {
                    continue;
                }

                if (unitGridPosition == testGridPosition)
                {
                    //자신의 그리드는 제외한다.
                    continue;
                }


                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    if(LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition)[0].IsEnemy() == unit.IsEnemy())
                    {
                        continue;
                    }
                }
                validGridPostionList.Add(testGridPosition);
            }
        }
        return validGridPostionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        if (unit.isDie) return;
        targetVector = LevelGrid.Instance.GetWorldPosition(gridPosition);

        

        OnShootingFireBall?.Invoke(this, EventArgs.Empty);
        StartCoroutine(Delay(gridPosition));

        ActionStart(onActionComplete);
    }

    private void OnFireBallBehaviourComplete()
    {
        ActionComplete();
    }

    private IEnumerator Delay(GridPosition gridPosition)
    {
        yield return new WaitForSeconds(0.5f);
        Transform fireBallClone = Instantiate(fireBallPrefab, pivot.position, Quaternion.identity);
        FireBall fireballObject = fireBallClone.GetComponent<FireBall>();
        fireballObject.SetUp(gridPosition, OnFireBallBehaviourComplete);
        fireballObject.SetDamage(damage);
    }
}
