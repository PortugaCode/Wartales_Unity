using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private GridPosition gridPosition;
    private MoveAction moveAction;
    private SpinAction spinAction;
    private BaseAction[] baseActionArray;

    [Header("ActionCount")]
    [SerializeField] private int actionPoints = 3;

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
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
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
    }
}
