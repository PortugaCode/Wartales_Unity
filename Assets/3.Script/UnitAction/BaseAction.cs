using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseAction : MonoBehaviour
{
    public static EventHandler OnAnyActionStarted;
    public static EventHandler OnAnyActionCompleted;

    protected Unit unit;
    protected bool isActive;
    protected Action onActionComplete;

    protected virtual void Awake()
    {
        TryGetComponent(out unit);
    }

    public abstract string GetActionName();
    public abstract Sprite GetActionImage();

    public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);

    public virtual bool isValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidGridPostionList();
        return validGridPositionList.Contains(gridPosition);
    }

    public abstract List<GridPosition> GetValidGridPostionList();


    public virtual int GetActionPointCost()
    {
        return 1;
    }

    protected void ActionStart(Action onActionComplete)
    {
        isActive = true;
        this.onActionComplete = onActionComplete;

        OnAnyActionStarted?.Invoke(this, EventArgs.Empty);
    }

    protected void ActionComplete()
    {
        isActive = false;
        onActionComplete();

        GridSystemVisual.Instance.UpdateGridVisualPublic();

        OnAnyActionCompleted?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetUnit()
    {
        return unit;
    }

    public EnemyAIAction GetBestEnemyAIAction()
    {
        List<EnemyAIAction> enemyAIActionList = new List<EnemyAIAction>();

        List<GridPosition> GetValidActionGridPositionList = GetValidGridPostionList();

        foreach(GridPosition gridPosition in GetValidActionGridPositionList)
        {
            EnemyAIAction enemyAIAction = GetEnemyAIAction(gridPosition);
            enemyAIActionList.Add(enemyAIAction);
        }
        if(enemyAIActionList.Count > 0)
        {
            enemyAIActionList.Sort((EnemyAIAction a, EnemyAIAction b) => b.actionValue - a.actionValue);
            return enemyAIActionList[0];
        }
        else
        {
            //할 수 있는 Action이 없다.
            return null;
        }
        
    }

    public abstract EnemyAIAction GetEnemyAIAction(GridPosition gridPosition);
}
