using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{



    [Header("Animator")]
    [SerializeField] private Animator animator;

    [Header("Target Position")]
    [SerializeField] private Vector3 targetPosition;

    [Header("Movefloat")]
    [SerializeField] private float speed = 4f;
    [SerializeField] private float rotationSpeed = 8f;
    private float stopDistance = .1f;

    [Header("MaxMoveDistance")]
    [SerializeField] private int maxMoveDistance = 4;

    [Header("Character State")]
    public bool isWalking = false;

    [Header("Image")]
    public Sprite sprite;

    protected override void Awake()
    {
        base.Awake();
        targetPosition = transform.position;
    }

    private void Update()
    {
        animator.SetBool("isWalking", isWalking);
        if (!isActive) return;
        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        if (Vector3.Distance(transform.position, targetPosition) > stopDistance)
        {
            transform.position += moveDirection * speed * Time.deltaTime;
            isWalking = true;
        }
        else
        {
            isActive = false;
            onActionComplete();
            isWalking = false;
        }

        transform.forward = Vector3.Slerp(transform.forward, moveDirection, rotationSpeed * Time.deltaTime);
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        this.onActionComplete = onActionComplete;
        targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
        isActive = true;
    }


    public override List<GridPosition> GetValidGridPostionList()
    {
        List<GridPosition> validGridPostionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPostion();

        for(int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for(int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                if(!LevelGrid.Instance.isValidGridPosition(testGridPosition))
                {
                    //�ش� unit�� �ִ� �Ÿ���ŭ�� �����̰�
                    continue;
                }
                if(unitGridPosition == testGridPosition)
                {
                    //�ڽ��� �׸���� �����Ѵ�.
                    continue;
                }
                if(LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    //�ش� ��ġ �׸��忡 Unit�� �ִٸ�?
                    continue;
                }

                validGridPostionList.Add(testGridPosition);
            }
        }
        return validGridPostionList;
    }

    public override int GetActionPointCost()
    {
        return base.GetActionPointCost();
    }





    public override string GetActionName()
    {
        return "Move";
    }

    public override Sprite GetActionImage()
    {
        return sprite;
    }
}
