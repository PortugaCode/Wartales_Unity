using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        WaitingforEnemyTurn,
        TakingTurn,
        Busy
    }


    private State state;
    private float timer;

    private void Awake()
    {
        state = State.WaitingforEnemyTurn;
    }

    private void Start()
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    private void Update()
    {
        if (TurnSystem.Instance.IsPlayerTurn()) return;

        switch(state)
        {
            case State.WaitingforEnemyTurn:
                break;
            case State.TakingTurn:
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    if(TryTakeEnemyAIAciton(SetStateTakingTurn))
                    {
                        state = State.Busy;
                    }
                    else
                    {
                        // 적이 코스트가 더이상 없을 때
                        TurnSystem.Instance.NextTurn();
                    }
                }
                break;
            case State.Busy:
                break;
        }
    }

    private void SetStateTakingTurn()
    {
        timer = 0.5f;
        state = State.TakingTurn;
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if(!TurnSystem.Instance.IsPlayerTurn())
        {
            state = State.TakingTurn;
            timer = 2.0f;
        }
    }

    private bool TryTakeEnemyAIAciton(Action OnEnemyAIActionComplete)
    {
        Debug.Log("Take Enemy AI Action!");

        foreach(Unit enemyUnit in UnitManager.Instance.GetEnemyUnitList())
        {
            if(TryTakeEnemyAIAciton(enemyUnit, OnEnemyAIActionComplete))
            {
                return true;
            }
        }
        return false;
    }

    private bool TryTakeEnemyAIAciton(Unit enemyUnit, Action OnEnemyAIActionComplete)
    {
        SpinAction spinAction = enemyUnit.GetSpinAction();

        GridPosition actionGridPosition = enemyUnit.GetGridPostion();
        if (!spinAction.isValidActionGridPosition(actionGridPosition)) return false;

        if (!enemyUnit.TrySpendActionPointsToTakeAction(spinAction)) return false;

        Debug.Log(enemyUnit + "Spin Aciton!");
        spinAction.TakeAction(actionGridPosition, OnEnemyAIActionComplete);
        return true;
    }
}
