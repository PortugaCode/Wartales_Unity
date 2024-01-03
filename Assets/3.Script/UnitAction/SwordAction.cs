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

    private float dotProduct;



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
                if(unit.isRogue)
                {
                    float afterHitState = 2.0f;
                    stateTimer = afterHitState;
                }
                else
                {
                    float afterHitState = 2.7f;
                    stateTimer = afterHitState;
                }

                if(canAttack)
                {
                    Attack();
                    canAttack = false;
                }
                break;

            case State.SwingingSwordAfterHit:
                ActionComplete();
                break;
        }
    }

    private IEnumerator TargetDamage(Unit target)
    {
        yield return new WaitForSeconds(0.25f);
        Vector3 targetdir = unit.GetWorldPosition() + Vector3.up * 1.2f - targetUnit.GetWorldPosition() + Vector3.up * 1.2f;

        Vector3 targetforward = target.transform.forward;
        targetdir.Normalize();

        dotProduct = Vector3.Dot(targetdir, targetforward);
        Debug.Log(dotProduct);

        if (dotProduct < -0.6 && dotProduct > -0.7)
        {
            target.Damage(damage * 2);
        }
        else
        {
            target.Damage(damage);
        }

        EffectSystem.Instance.hitEffect.transform.position = target.GetWorldPosition() + Vector3.up * 1.2f;
        EffectSystem.Instance.hitEffect.Play();
    }


    private void Attack()
    {
        if (unit.isRogue)
        {
            StartCoroutine(TargetDamage(targetUnit));
        }
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
        if(unit.isRogue)
        {
            return "Dagger";
        }
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
