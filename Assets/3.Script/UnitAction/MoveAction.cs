using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{

    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;

    [Header("Target Position")]
    [SerializeField] private List<Vector3> positionList;
    private int currentPositionIndex;

    [Header("Movefloat")]
    [SerializeField] private float speed = 4f;
    [SerializeField] private float rotationSpeed = 8f;
    private float stopDistance = .05f;

    [Header("MaxMoveDistance")]
    [SerializeField] private int maxMoveDistance = 4;

    [Header("Image")]
    public Sprite sprite;

    [SerializeField] private Unit targetUnit;
    private bool isNowNodeisMaxDistance = false;

    private void Update()
    {
        if (!isActive) return;
        Vector3 targetPosition = positionList[currentPositionIndex];
        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        transform.forward = Vector3.Slerp(transform.forward, moveDirection, rotationSpeed * Time.deltaTime);
    }


    private void FixedUpdate()
    {
        if (!isActive) return;

        Vector3 targetPosition = positionList[currentPositionIndex];

        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        if (Vector3.Distance(transform.position, targetPosition) > stopDistance)
        {
            transform.position += moveDirection * speed * Time.deltaTime;
        }
        else
        {
            currentPositionIndex++;
            if(currentPositionIndex >= positionList.Count)
            {
                OnStopMoving?.Invoke(this, EventArgs.Empty);
                ActionComplete();
            }
        }

    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        if (unit.isDie) return;
        List<GridPosition> pathGridPositionList = Pathfinding.Instance.FindPath(unit.GetGridPostion(), gridPosition, out int pathLegth);


        currentPositionIndex = 0;
        positionList = new List<Vector3>();

        foreach(GridPosition pathGridPosition in pathGridPositionList)
        {
            positionList.Add(LevelGrid.Instance.GetWorldPosition(pathGridPosition));
        }


        OnStartMoving?.Invoke(this, EventArgs.Empty);
        ActionStart(onActionComplete);
    }


    public override List<GridPosition> GetValidGridPostionList()
    {
        List<GridPosition> validGridPostionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPostion();

        for(int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for(int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if(!LevelGrid.Instance.isValidGridPosition(testGridPosition))
                {
                    //해당 unit의 최대 거리만큼만 움직이게
                    continue;
                }

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > maxMoveDistance)
                {
                    continue;
                }

                if (unitGridPosition == testGridPosition)
                {
                    //자신의 그리드는 제외한다.
                    continue;
                }

                if(LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    //해당 위치 그리드에 Unit이 있다면?
                    continue;
                }

                if(!Pathfinding.Instance.IsWalkableGridPosition(testGridPosition))
                {
                    continue;
                }

                if(!Pathfinding.Instance.HasPath(unitGridPosition, testGridPosition))
                {
                    continue;
                }

                int pathFindingDistanceMulti = 10;
                if(Pathfinding.Instance.PathLength(unitGridPosition, testGridPosition) > maxMoveDistance * pathFindingDistanceMulti)
                {
                    //해당 위치가 maxMoveDistance보다 더 많이 움직여야 한다면?
                    continue;
                }

                validGridPostionList.Add(testGridPosition);
            }
        }
        return validGridPostionList;
    }

    public override int GetActionPointCost()
    {
        return base.GetActionPointCost();
    }

    public override string GetActionName()
    {
        return "Move";
    }

    public override Sprite GetActionImage()
    {
        return sprite;
    }

    private void FindNearestUnit(float distance, List<Unit> targetUnitList)
    {
        if (targetUnit == null)
        {
            foreach (Unit target in targetUnitList)
            {
                if (!target.IsEnemy())
                {
                    if (Vector3.Distance(unit.GetWorldPosition(), target.GetWorldPosition()) < distance)
                    {
                        distance = Vector3.Distance(unit.GetWorldPosition(), target.GetWorldPosition());
                        targetUnit = target;
                    }
                }
            }
        }
    }

    private int ShooterMoveToMaxDistance(int maxShootDistance, GridPosition gridPosition)
    {
        GridPosition targetGridPosition = targetUnit.GetGridPostion();
        List<GridPosition> pathLengthList = Pathfinding.Instance.FindPath(targetGridPosition, gridPosition, out int pathLength);

        if (unit.GetAction<ShootAction>().GetTargetCountAtPosition(gridPosition) > 0)
        {
            if(pathLengthList != null)
            {
                if (pathLengthList.Count >= maxShootDistance)
                {
                    //Debug.Log($"{gridPosition} 가장 먼 비거리입니다.");
                    return 15;
                }
            }
        }
        return 0;
    }


    private int CalculateValue(int calculateActionValue, GridPosition gridPosition)
    {
        GridPosition targetGridPosition = targetUnit.GetGridPostion();
        int baseValue = maxMoveDistance * 10;
        List<GridPosition> pathLengthList = Pathfinding.Instance.FindPath(targetGridPosition, gridPosition, out int pathLength);


        if (pathLengthList !=null)
        {
            calculateActionValue += baseValue - pathLengthList.Count;
        }

        if (calculateActionValue <= 0)
        {
            calculateActionValue = 0;
        }

        return calculateActionValue;
    }


    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        
        int calculateActionValue = 0;
        List<Unit> targetUnitList = UnitManager.Instance.GetFriendlyUnitList();
        float distance = float.MaxValue;

        if (unit.isAchor)
        {
            int targetCountAtGridPosition = unit.GetAction<ShootAction>().GetTargetCountAtPosition(gridPosition);
            int maxShootDistance = unit.GetAction<ShootAction>().GetMaxShootDistance();

            FindNearestUnit(distance, targetUnitList); //가장 가까운 적은 지정

            calculateActionValue += ShooterMoveToMaxDistance(maxShootDistance, gridPosition); // 슛 거리 닿는 지역 중 가장 타겟과 먼 쪽으로 이동
            calculateActionValue += CalculateValue(calculateActionValue, gridPosition); // 이동할 때 타깃한테 가는 최단 루트로 이동 (A*)

            return new EnemyAIAction
            {
                gridPosition = gridPosition,
                actionValue = targetCountAtGridPosition * 10 + calculateActionValue,
            };
        }
        else
        {
            int targetCountAtGridPosition = unit.GetAction<SwordAction>().GetTargetCountAtPosition(gridPosition);
            FindNearestUnit(distance, targetUnitList);
            calculateActionValue += CalculateValue(calculateActionValue, gridPosition);

            return new EnemyAIAction
            {
                gridPosition = gridPosition,
                actionValue = targetCountAtGridPosition * 10 + calculateActionValue,
            };
        }
    }
}
