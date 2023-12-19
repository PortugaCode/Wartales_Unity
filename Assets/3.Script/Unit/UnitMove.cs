using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMove : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] private Animator animator;

    [Header("Target Position")]
    [SerializeField] private Vector3 targetPosition;

    [Header("Movefloat")]
    [SerializeField] private float speed = 4f;
    [SerializeField] private float rotationSpeed = 8f;
    private float stopDistance = .1f;

    [Header("Character State")]
    public bool isWalking = false;


    private void Awake()
    {
        targetPosition = transform.position;
    }

    private void Update()
    {
        animator.SetBool("isWalking", isWalking);

        if(Vector3.Distance(transform.position, targetPosition) > stopDistance)
        {
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            transform.position += moveDirection * speed * Time.deltaTime;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, rotationSpeed * Time.deltaTime);
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
    }

    public void Move(Vector3 target)
    {
        targetPosition = target;
    }
}
