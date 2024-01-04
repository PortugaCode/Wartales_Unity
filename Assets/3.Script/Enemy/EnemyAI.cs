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


    [SerializeField] private State state;
    [SerializeField] private float timer;

    [SerializeField] private Transform cameraTarget;
    [SerializeField] private Unit target;
    private Vector3 offset;
    private Vector3 currentVelocity = Vector3.zero;

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

        switch (state)
        {
            case State.WaitingforEnemyTurn:
                break;
            case State.TakingTurn:
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    if (TryTakeEnemyAIAciton(SetStateTakingTurn))
                    {
                        state = State.Busy;
                    }
                    else
                    {
                        // 적이 코스트가 더이상 없을 때
                        target = null;
                        TurnSystem.Instance.NextTurn();
                    }
                }
                break;
            case State.Busy:
                break;
        }
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 targetposition = target.GetWorldPosition() + offset;
            cameraTarget.position = Vector3.SmoothDamp(cameraTarget.position, targetposition, ref currentVelocity, 0.25f);
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
            timer = 2.0f;
            state = State.TakingTurn;
        }
        else
        {
            UnitActionSystem.Instance.isCamMove = true;
            UnitActionSystem.Instance.needSetPosition = true;
        }
    }

    private bool TryTakeEnemyAIAciton(Action OnEnemyAIActionComplete)
    {
        foreach(Unit enemyUnit in UnitManager.Instance.GetEnemyUnitList())
        {
            if(TryTakeEnemyAIAciton(enemyUnit, OnEnemyAIActionComplete))
            {
                target = enemyUnit;
                Vector3 targetPosition = target.GetWorldPosition();
                offset = targetPosition - target.GetWorldPosition();
                return true;
            }
        }
        return false;
    }

    private bool TryTakeEnemyAIAciton(Unit enemyUnit, Action OnEnemyAIActionComplete)
    {
        EnemyAIAction bestEnemyAiAction = null;
        BaseAction bestBaseAction = null;

        foreach(BaseAction baseAction in enemyUnit.GetBaseActionsArray())
        {
            if(!enemyUnit.CanSpendActionPointsToTakeAction(baseAction))
            {
                //적이 해당 액션을 할 코스트가 없을 경우
                continue;
            }

            if(bestEnemyAiAction == null)
            {
                bestEnemyAiAction = baseAction.GetBestEnemyAIAction();
                bestBaseAction = baseAction;
            }

            else
            {
                EnemyAIAction testEnemyAIAction = baseAction.GetBestEnemyAIAction();
                if(testEnemyAIAction != null && testEnemyAIAction.actionValue > bestEnemyAiAction.actionValue)
                {
                    bestEnemyAiAction = testEnemyAIAction;
                    bestBaseAction = baseAction;
                }
            }
        }

        if(bestEnemyAiAction != null && enemyUnit.TrySpendActionPointsToTakeAction(bestBaseAction))
        {
            bestBaseAction.TakeAction(bestEnemyAiAction.gridPosition, OnEnemyAIActionComplete);
            return true;
        }
        else
        {
            return false;
        }
    }
}
