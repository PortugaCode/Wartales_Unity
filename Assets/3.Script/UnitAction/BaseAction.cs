using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseAction : MonoBehaviour
{
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
}
