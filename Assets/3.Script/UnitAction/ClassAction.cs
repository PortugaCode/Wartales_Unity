using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClassAction : BaseAction
{
    public delegate void SpinCompleteDelegate();

    public event EventHandler OnBerserk;

    

    [Header("Image")]
    public Sprite Trap;
    public Sprite Berserk;
    public Sprite Assassination;
    public Sprite Healing;


    private bool isActive2;

    private void Update()
    {
        if (!isActive) return;
        if(isActive2)
        {
            StartCoroutine(DelayActionComplete());
        }
    }

    private IEnumerator DelayActionComplete()
    {
        yield return new WaitForSeconds(1f);
        isActive2 = false;
        ActionComplete();
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        if (unit.isDie) return;
        if (unit.isWarrior)
        {
            OnBerserk?.Invoke(this, EventArgs.Empty);
            unit.GetAction<SwordAction>().SetBerserk(true);
            unit.GetAction<SwordAction>().berserkEffect.Play();
            unit.GetHealthSystem().Sethealth(-50);
        }
        isActive2 = true;
        ActionStart(onActionComplete);
    }


    public override string GetActionName()
    {
        if (unit.isAchor)
        {
            return "Trap";
        }
        else if(unit.isWarrior)
        {
            return "Berserk";
        }
        else if (unit.isRogue)
        {
            return "Assassination";
        }
        else if (unit.isWizard)
        {
            return "Healing";
        }
        else
        {
            return "null";
        }
    }

    public override Sprite GetActionImage()
    {
        if (unit.isAchor)
        {
            return Trap;
        }
        else if (unit.isWarrior)
        {
            return Berserk;
        }
        else if (unit.isRogue)
        {
            return Assassination;
        }
        else if (unit.isWizard)
        {
            return Healing;
        }
        else
        {
            return null;
        }
    }

    public override List<GridPosition> GetValidGridPostionList()
    {
        if (unit.isWarrior)
        {
            List<GridPosition> validGridPostionList = new List<GridPosition>();
            GridPosition unitGridPosition = unit.GetGridPostion();

            if (unit.GetAction<SwordAction>().IsBerserk)
            {
                return validGridPostionList;
            }
            else
            {
                return new List<GridPosition>
                {
                    unitGridPosition
                };
            }
        }
        else if(unit.isAchor)
        {
            List<GridPosition> validGridPostionList = new List<GridPosition>();
            GridPosition unitGridPosition = unit.GetGridPostion();
            int maxDistance = 1;

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

                    if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                    {
                        //해당 위치 그리드에 Unit이 있다면
                        continue;
                    }

                    if (unitGridPosition == testGridPosition)
                    {
                        //자신의 그리드는 제외한다.
                        continue;
                    }

                    if (!Pathfinding.Instance.IsWalkableGridPosition(testGridPosition))
                    {
                        continue;
                    }

                    validGridPostionList.Add(testGridPosition);
                }
            }
            return validGridPostionList;
        }
        else if(unit.isWizard)
        {
            List<GridPosition> validGridPostionList = new List<GridPosition>();
            GridPosition unitGridPosition = unit.GetGridPostion();
            int maxDistance = 2;

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

                    int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                    if (testDistance > maxDistance)
                    {
                        continue;
                    }

                    if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                    {
                        //해당 위치 그리드에 Unit이 없다면? = Unit.Empty
                        continue;
                    }

                    Unit targetUnit = LevelGrid.Instance.GetAnyUnitOnGridPosition(testGridPosition);

                    if (targetUnit.IsEnemy() == !unit.IsEnemy())
                    {
                        //서로 같은 팀이 아닐 경우
                        continue;
                    }

                    validGridPostionList.Add(testGridPosition);
                }
            }
            return validGridPostionList;
        }
        else if(unit.isRogue)
        {
            List<GridPosition> validGridPostionList = new List<GridPosition>();
            GridPosition unitGridPosition = unit.GetGridPostion();
            int maxDistance = 6;

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

                    int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                    if (testDistance > maxDistance)
                    {
                        continue;
                    }

                    if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                    {
                        //해당 위치 그리드에 Unit이 없다면? = Unit.Empty
                        continue;
                    }

                    Unit targetUnit = LevelGrid.Instance.GetAnyUnitOnGridPosition(testGridPosition);

                    if (targetUnit.IsEnemy() == unit.IsEnemy())
                    {
                        //서로 같은 팀일 경우
                        continue;
                    }

                    validGridPostionList.Add(testGridPosition);
                }
            }
            return validGridPostionList;
        }
        else
        {
            GridPosition unitGridPosition = unit.GetGridPostion();

            return new List<GridPosition>
            {
                unitGridPosition
            };
        }
    }

    public override int GetActionPointCost()
    {
        return 1;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 0,
        };
    }
}
