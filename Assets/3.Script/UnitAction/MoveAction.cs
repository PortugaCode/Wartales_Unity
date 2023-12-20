using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : MonoBehaviour
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

    private Unit unit;

    private void Awake()
    {
        TryGetComponent(out unit);
        targetPosition = transform.position;
    }

    private void Update()
    {
        animator.SetBool("isWalking", isWalking);

        if (Vector3.Distance(transform.position, targetPosition) > stopDistance)
        {
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            transform.position += moveDirection * speed * Time.deltaTime;
            transform.forward = Vector3.Slerp(transform.forward, moveDirection, rotationSpeed * Time.deltaTime);
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
    }

    public void Move(GridPosition gridPosition)
    {
        targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
    }

    public bool isValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPostionList = GetValidGridPostionList();
        return validGridPostionList.Contains(gridPosition);
    }

    public List<GridPosition> GetValidGridPostionList()
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
}
