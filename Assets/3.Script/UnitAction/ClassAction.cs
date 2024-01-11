using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClassAction : BaseAction
{
    public delegate void SpinCompleteDelegate();

    public event EventHandler OnBerserk;
    public event EventHandler OnHealing;
    public event EventHandler OnAssassination;
    public event EventHandler OnTrap;

    private Unit targetUnit;
    private Vector3 targetPosition;


    [Header("Image")]
    public Sprite Trap;
    public Sprite Berserk;
    public Sprite Assassination;
    public Sprite Healing;

    [Header("Trap")]
    public GameObject trapPrefab;


    private bool isActive2;

    private int maxDistance;

    private void Start()
    {
        if (unit.isWarrior) maxDistance = 0;
        if (unit.isAchor) maxDistance = 1;
        if (unit.isWizard) maxDistance = 2;
        if (unit.isRogue) maxDistance = 6;
    }

    private void Update()
    {
        if (!isActive) return;
        if (unit.isWizard)
        {
            Vector3 aimDirection = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
            float rotationSpeed = 8f;
            transform.forward = Vector3.Slerp(transform.forward, aimDirection, rotationSpeed * Time.deltaTime);
        }
        else if(unit.isAchor)
        {
            Vector3 aimDirection = (targetPosition - unit.GetWorldPosition()).normalized;
            float rotationSpeed = 8f;
            transform.forward = Vector3.Slerp(transform.forward, aimDirection, rotationSpeed * Time.deltaTime);
        }

        if(isActive2)
        {
            StartCoroutine(DelayActionComplete());
        }
    }

    public override int GetActionPointCost()
    {
        if(unit.isRogue)
        {
            return 2;
        }
        else
        {
            return 1;
        }
    }

    private IEnumerator DelayActionComplete()
    {
        yield return new WaitForSeconds(2f);
        isActive2 = false;

        ActionComplete();
    }

    private IEnumerator DelayHeal()
    {
        yield return new WaitForSeconds(0.5f);
        targetUnit.SetHealth(20);
    }

    private IEnumerator DelayBlood()
    {
        yield return new WaitForSeconds(0.5f);
        AudioManager.Instance.TakeDamageSoundPlay();
        EffectSystem.Instance.hitEffect.transform.position = targetUnit.GetWorldPosition() + Vector3.up * 1.6f;
        EffectSystem.Instance.hitEffect.Play();
    }

    private IEnumerator DelayTrap()
    {
        yield return new WaitForSeconds(0.8f);
        GameObject cloneTrap = Instantiate(trapPrefab, targetPosition, Quaternion.identity);
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        if (unit.isDie) return;
        if (unit.isWarrior)
        {
            unit.GetAction<SwordAction>().SetBerserk(true);
            unit.GetAction<SwordAction>().berserkEffect.Play();
            unit.SetHealth(-100);
            OnBerserk?.Invoke(this, EventArgs.Empty);
        }

        else if(unit.isWizard)
        {
            targetUnit = LevelGrid.Instance.GetAnyUnitOnGridPosition(gridPosition);

            StartCoroutine(DelayHeal());

            EffectSystem.Instance.HealingPlay(targetUnit.GetWorldPosition());

            OnHealing?.Invoke(this, EventArgs.Empty);
        }

        else if(unit.isRogue)
        {
            targetUnit = LevelGrid.Instance.GetAnyUnitOnGridPosition(gridPosition);
            Vector3 aimDirection = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
            EffectSystem.Instance.SmokePlay(unit.GetWorldPosition());
            GridPosition oldGridPosition = unit.GetGridPostion();
            transform.position = targetUnit.GetWorldPosition() + targetUnit.transform.forward * -0.3f;
            transform.forward = targetUnit.transform.forward;
            AudioManager.Instance.AssasinAttackSoundPlay();
            AudioManager.Instance.SmokeSoundPlay();

            GridPosition newgridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
            if (newgridPosition != oldGridPosition)
            {
                //유닛이 기존 그리드 포지션에서 위치가 바뀌었다면
                LevelGrid.Instance.UnitMoveGridPostion(unit, oldGridPosition, newgridPosition);
            }

            targetUnit.GetHealthSystem().isAssasin = true;
            targetUnit.SetHealth(-100);


            StartCoroutine(DelayBlood());

            Destroy(targetUnit.gameObject, 4f);

            OnAssassination?.Invoke(this, EventArgs.Empty);
        }

        else if(unit.isAchor)
        {
            targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);

            StartCoroutine(DelayTrap());

            OnTrap?.Invoke(this, EventArgs.Empty);
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
            return "Assassin";
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

                    Trap trap = LevelGrid.Instance.GetTrapAtGridPosition(testGridPosition);
                    if(trap != null)
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

                    if(targetUnit.GetHealthSystem().Gethealth() > 100)
                    {
                        continue;
                    }

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


    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 0,
        };
    }

    public override int GetMaxDistance()
    {
        return maxDistance;
    }
}
