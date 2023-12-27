using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{
    public event EventHandler OnShooting;
    public event EventHandler OnAiming;

    private enum State
    {
        Aiming,
        Shooting,
        Cooloff
    }

    private State state;
    private float stateTimer;

    [SerializeField] private Sprite icon;

    [SerializeField] private int maxShootDistance = 7;
    [SerializeField] private int damage = 50;
    private Unit targetUnit;
    private bool canShootArrow;

    private void Update()
    {
        if (!isActive) return;

        stateTimer -= Time.deltaTime;

        switch (state)
        {
            case State.Aiming:
                Debug.Log(targetUnit.name);
                Vector3 aimDirection = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                float rotationSpeed = 10f;
                transform.forward = Vector3.Slerp(transform.forward, aimDirection, rotationSpeed * Time.deltaTime);
                break;

            case State.Shooting:
                if(canShootArrow)
                {
                    Shoot();
                    canShootArrow = false;
                }
                break;

            case State.Cooloff:
                break;
        }
        if(stateTimer <= 0f)
        {
            NextState();
        }
    }

    private void Shoot()
    {
        OnShooting?.Invoke(this, EventArgs.Empty);
        //targetUnit.Damage();
    }

    private void NextState()
    {
        switch (state)
        {
            case State.Aiming:
                state = State.Shooting;
                float shootingStateTimer = 0.1f;
                stateTimer = shootingStateTimer;
                break;

            case State.Shooting:
                state = State.Cooloff;
                float coolOffStateTimer = 1.7f;
                stateTimer = coolOffStateTimer;
                break;

            case State.Cooloff:
                ActionComplete();
                break;
        }
    }

    public override Sprite GetActionImage()
    {
        return icon;
    }

    public override string GetActionName()
    {
        return "Shoot";
    }


    public override List<GridPosition> GetValidGridPostionList()
    {
        GridPosition unitGridPosition = unit.GetGridPostion();
        return GetValidGridPostionList(unitGridPosition);
    }

    public List<GridPosition> GetValidGridPostionList(GridPosition gridPosition)
    {
        List<GridPosition> validGridPostionList = new List<GridPosition>();

        for (int x = -maxShootDistance; x <= maxShootDistance; x++)
        {
            for (int z = -maxShootDistance; z <= maxShootDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = gridPosition + offsetGridPosition;
                if (!LevelGrid.Instance.isValidGridPosition(testGridPosition))
                {
                    //�׸��� �ȿ����� �����̰Բ�
                    continue;
                }

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if(testDistance > maxShootDistance)
                {
                    continue;
                }

                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    //�ش� ��ġ �׸��忡 Unit�� ���ٸ�? = Unit.Empty
                    continue;
                }

                Unit targetUnit = LevelGrid.Instance.GetAnyUnitOnGridPosition(testGridPosition);

                if(targetUnit.IsEnemy() == unit.IsEnemy())
                {
                    //���� ���� ���� ���
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

        state = State.Aiming;
        float aimingStateTimer = 1.0f;
        stateTimer = aimingStateTimer;
        canShootArrow = true;
        OnAiming?.Invoke(this, EventArgs.Empty);
        ActionStart(onActionComplete);
    }

    public override int GetActionPointCost()
    {
        return 2;
    }

    public Unit GetTargetUnit()
    {
        return targetUnit;
    }
    public int GetDamage()
    {
        return damage;
    }
    public int GetMaxShootDistance()
    {
        return maxShootDistance;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        Unit targetUnit = LevelGrid.Instance.GetAnyUnitOnGridPosition(gridPosition);
        Vector3 direction = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;

        if (targetUnit == null)
        {
                            return new EnemyAIAction
                {
                    gridPosition = gridPosition,
                    actionValue = 0,
                };
        }

        if(Physics.Raycast(unit.GetWorldPosition(), direction, out RaycastHit hit, 1000f, unit.unitLayer))
        {
            if(hit.transform.CompareTag("Enemy"))
            {
                return new EnemyAIAction
                {
                    gridPosition = gridPosition,
                    actionValue = 0,
                };
            }
            else
            {
                return new EnemyAIAction
                {
                    gridPosition = gridPosition,
                    actionValue = 100 + Mathf.RoundToInt((1 - targetUnit.GetHealthSystem().GetHealthNormalized()) * 100f),
                };
            }
        }
        else
        {
            return new EnemyAIAction
            {
                gridPosition = gridPosition,
                actionValue = 100 + Mathf.RoundToInt((1 - targetUnit.GetHealthSystem().GetHealthNormalized()) * 100f),
            };
        }
    }

    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
        return GetValidGridPostionList(gridPosition).Count;
    }

}
