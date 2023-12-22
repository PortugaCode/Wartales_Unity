using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private GridPosition gridPosition;
    private MoveAction moveAction;
    private SpinAction spinAction;
    private BaseAction[] baseActionArray;

    public static event EventHandler OnAnyActionPointsChanged;

    [Header("ActionCount")]
    public int BaseActionPoints = 3;
    [SerializeField] private int actionPoints;

    [Header("UnitClass")]
    public bool isWarrior;
    public bool isAchor;


    private GameObject uiObject;

    private void Awake()
    {
        TryGetComponent(out moveAction);
        TryGetComponent(out spinAction);
        baseActionArray = GetComponents<BaseAction>();
        uiObject = GameObject.FindGameObjectWithTag("UI");
        actionPoints = BaseActionPoints;
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    private void Update()
    {
        GridPosition newgridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newgridPosition != gridPosition)
        {
            LevelGrid.Instance.UnitMoveGridPostion(this, gridPosition, newgridPosition);
            gridPosition = newgridPosition;
        }
    }

    public GameObject GetUiObject()
    {
        return uiObject;
    }    

    public MoveAction GetMoveAction()
    {
        return moveAction;
    }

    public SpinAction GetSpinAction()
    {
        return spinAction;
    }

    public GridPosition GetGridPostion()
    {
        return gridPosition;
    }

    public BaseAction[] GetBaseActionsArray()
    {
        return baseActionArray;
    }

    public int GetActionPoints()
    {
        return actionPoints;
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


    private bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if (actionPoints >= baseAction.GetActionPointCost()) return true;
        else return false;
    }

    private void SpendAcionPoint(int amount)
    {
        actionPoints -= amount;

        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        actionPoints = BaseActionPoints;

        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }
}
