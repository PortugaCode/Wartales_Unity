using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private GridPosition gridPosition;
    private BaseAction[] baseActionArray;
    private HealthSystem healthSystem;


    //EventHandler==========================================
    public static event EventHandler OnAnyActionPointsChanged;
    public event EventHandler OnDamage;
    public event EventHandler OnSetHealth;
    public event EventHandler OnDie;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;
    //======================================================

    [Header("UnitUI")]
    public GameObject UnitUI;

    [Header("isEnemy")]
    [SerializeField] private bool isEnemy;

    [Header("ActionCount")]
    public int BaseActionPoints = 3;
    [SerializeField] private int actionPoints;

    [Header("UnitClass")]
    public bool isWarrior;
    public bool isAchor;
    public bool isWizard;
    public bool isRogue;

    [Header("LayerMasks")]
    public LayerMask unitLayer;
    public LayerMask wallLayer;

    [Header("SpecialWeapon")]
    [SerializeField] private GameObject normalWeapon;
    [SerializeField] private GameObject specialWeapon;

    public void SetWeapon()
    {
        if(isWizard)
        {
            specialWeapon.SetActive(true);
            return;
        }
        else if(isAchor)
        {
            specialWeapon.GetComponent<ParticleSystem>().Play();
            return;
        }
        normalWeapon.SetActive(false);
        specialWeapon.SetActive(true);
    }

    [HideInInspector]
    public bool isDie;

    private GameObject uiObject;

    private void Awake()
    {
        TryGetComponent(out healthSystem);
        baseActionArray = GetComponents<BaseAction>();
        uiObject = GameObject.FindGameObjectWithTag("UI");
        actionPoints = BaseActionPoints;
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        healthSystem.OnDead += HealthSystem_OnDead;
        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);
    }


    private void Update()
    {
        isDie = healthSystem.IsDie;

        GridPosition newgridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newgridPosition != gridPosition)
        {
            //������ ���� �׸��� �����ǿ��� ��ġ�� �ٲ���ٸ�
            GridPosition oldGridPosition = gridPosition;
            gridPosition = newgridPosition;
            LevelGrid.Instance.UnitMoveGridPostion(this, oldGridPosition, newgridPosition);
        }
    }

    public void SetbaseActionArray()
    {
        baseActionArray = GetComponents<BaseAction>();
    }

    public GameObject GetUiObject()
    {
        return uiObject;
    }    


    public T GetAction<T>() where T : BaseAction
    {
        foreach(BaseAction baseAction in baseActionArray)
        {
            if(baseAction is T)
            {
                return (T)baseAction;
            }
        }
        return null;
    }

    public GridPosition GetGridPostion()
    {
        return gridPosition;
    }

    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }

    public BaseAction[] GetBaseActionsArray()
    {
        return baseActionArray;
    }

    public int GetActionPoints()
    {
        return actionPoints;
    }

    public bool IsEnemy()
    {
        return isEnemy;
    }


    public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if (CanSpendActionPointsToTakeAction(baseAction))
        {
            SpendAcionPoint(baseAction.GetActionPointCost());
            return true;
        }
        else return false;
    }


    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if (actionPoints >= baseAction.GetActionPointCost()) return true;
        else return false;
    }

    public void SpendAcionPoint(int amount)
    {
        actionPoints -= amount;

        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if((IsEnemy() && !TurnSystem.Instance.IsPlayerTurn()) || (!IsEnemy() && TurnSystem.Instance.IsPlayerTurn()))
        {
            actionPoints = BaseActionPoints;

            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public HealthSystem GetHealthSystem()
    {
        return healthSystem;
    }

    public void Damage(int amount)
    {
        AudioManager.Instance.TakeDamageSoundPlay();
        healthSystem.Damage(amount);
        if(healthSystem.Gethealth() != 0)
        {
            OnDamage?.Invoke(this, EventArgs.Empty);
        }
    }

    public void SetHealth(int a)
    {
        healthSystem.Sethealth(a);
        OnSetHealth?.Invoke(this, EventArgs.Empty);
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        gameObject.layer = 13;
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);
        
        OnDie?.Invoke(this, EventArgs.Empty);
        Destroy(gameObject, 4f);
        OnAnyUnitDead?.Invoke(this, EventArgs.Empty);

        if (!isEnemy)
        {
            int r = UnityEngine.Random.Range(0, UnitManager.Instance.GetFriendlyUnitList().Count);
            UnitActionSystem.Instance.SetSelectUnit(UnitManager.Instance.GetFriendlyUnitList()[r]);
        }
    }
}
