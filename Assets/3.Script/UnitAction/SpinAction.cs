using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpinAction : BaseAction
{
    public delegate void SpinCompleteDelegate();


    [Header("Image")]
    public Sprite sprite;

    private float totalSpin;
    private void Update()
    {
        if (!isActive) return;

        float spinAddAmount = 360.0f * Time.deltaTime;
        transform.eulerAngles += new Vector3(0, spinAddAmount, 0);

        totalSpin += spinAddAmount;
        if (totalSpin >= 360f)
        {
            ActionComplete();
        }
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        ActionStart(onActionComplete);
        totalSpin = 0;
    }


    public override string GetActionName()
    {
        return "Spin";
    }

    public override Sprite GetActionImage()
    {
        return sprite;
    }

    public override List<GridPosition> GetValidGridPostionList()
    {
        GridPosition unitGridPosition = unit.GetGridPostion();

        return new List<GridPosition>
        {
            unitGridPosition
        };
    }

    public override int GetActionPointCost()
    {
        return 2;
    }
}
