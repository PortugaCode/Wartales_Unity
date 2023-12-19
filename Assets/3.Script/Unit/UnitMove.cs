using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMove : MonoBehaviour
{
    [Header("Target Position")]
    [SerializeField] private Vector3 targetPosition;

    [Header("MoveSpeed [Default = 4.0]")]
    [SerializeField] private float speed = 4f;
    private float stopDistance = .1f;


    private void Update()
    {
        if(Vector3.Distance(transform.position, targetPosition) > stopDistance)
        {
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            transform.position += moveDirection * speed * Time.deltaTime;
        }

        if(Input.GetMouseButtonDown(0))
        {
            Move(MouseWorld.Instance.GetPoint());
        }

    }

    private void Move(Vector3 target)
    {
        targetPosition = target;
    }
}
