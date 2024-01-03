using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAction : BaseAction
{
    public event EventHandler OnAttack;


    [SerializeField] private Sprite sprite;
    [SerializeField] private int damage = 50;
    public int intdamage => damage;

    private enum State
    {
        SwingingSwordBeforeHit,
        SwingingSwordAfterHit
    }


    private int maxSwordDistance = 1;

    private State state;
    private float stateTimer;
    private Unit targetUnit;
    private bool canAttack;



    private void Update()
    {
        if (!isActive) return;

        stateTimer -= Time.deltaTime;

        switch (state)
        {
            case State.SwingingSwordBeforeHit:
                Vector3 aimDirection = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                float rotationSpeed = 8f;
                transform.forward = Vector3.Slerp(transform.forward, aimDirection, rotationSpeed * Time.deltaTime);
                break;

            case State.SwingingSwordAfterHit:
                break;
        }

        if (stateTimer <= 0f)
        {
            NextState();
        }
    }

    private void NextState()
    {
        switch (state)
        {
            case State.SwingingSwordBeforeHit:
                state = State.SwingingSwordAfterHit;
                float afterHitState = 2.5f;
                stateTimer = afterHitState;
                if(canAttack)
                {
                    AxeAttack();
                    canAttack = false;
                }
                break;

            case State.SwingingSwordAfterHit:
                ActionComplete();
                break;
        }
    }

    private void AxeAttack()
    {
        OnAttack?.Invoke(this, EventArgs.Empty);
    }

    public int GetMaxSwordDistance()
    {
        return maxSwordDistance;
    }

    public Unit GetTargetUnit()
    {
        return targetUnit;
    }


    public override Sprite GetActionImage()
    {
        return sprite;
    }

    public override string GetActionName()
    {
        return "Axe";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        Unit targetUnit = LevelGrid.Instance.GetAnyUnitOnGridPosition(gridPosition);

        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 200 + Mathf.RoundToInt((1 - targetUnit.GetHealthSystem().GetHealthNormalized()) * 100f),
        };
    }

    public override List<GridPosition> GetValidGridPostionList()
    {
        GridPosition unitGridPosition = unit.GetGridPostion();
        return GetValidGridPostionList(unitGridPosition);
    }

    public List<GridPosition> GetValidGridPostionList(GridPosition unitGridPosition)
    {
        List<GridPosition> validGridPostionList = new List<GridPosition>();

        for (int x = -maxSwordDistance; x <= maxSwordDistance; x++)
        {
            for (int z = -maxSwordDistance; z <= maxSwordDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                if (!LevelGrid.Instance.isValidGridPosition(testGridPosition))
                {
                    //그리드 안에서만 움직이게끔
                    continue;
                }

                if (unitGridPosition == testGridPosition)
                {
                    //자신의 그리드는 제외한다.
                    continue;
                }


                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    if (LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition)[0].IsEnemy() == unit.IsEnemy())
                    {
                        continue;
                    }
                }
                else
                {
                    continue;
                }


                if (!Pathfinding.Instance.IsWalkableGridPosition(testGridPosition))
                {
                    if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                    {
                        if (LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition)[0].IsEnemy() != unit.IsEnemy())
                        {
                            validGridPostionList.Add(testGridPosition);
                        }
                    }
                    continue;
                }

                validGridPostionList.Add(testGridPosition);
            }
        }
        return validGridPostionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        targetUnit = LevelGrid.Instance.GetAnyUnitOnGridPosition(gridPosition);
        canAttack = true;
        state = State.SwingingSwordBeforeHit;
        float beforeHitState = 1f;
        stateTimer = beforeHitState;

        ActionStart(onActionComplete);
    }

    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
        return GetValidGridPostionList(gridPosition).Count;
    }
}
