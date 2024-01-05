using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private bool isOpen;
    [SerializeField] private Animator animator;

    private Action onInteractComplete;
    private float timer;
    private bool isActive;

    [SerializeField] private LayerMask Open;
    [SerializeField] private LayerMask Close;

    private GridPosition gridPosition;


    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetDoorAtGridPosition(gridPosition, this);
    }



    public void Interact(Action onInteractComplete)
    {
        this.onInteractComplete = onInteractComplete;
        isActive = true;
        timer = 0.8f;

        if (isOpen)
        {
            CloseDoor();
            animator.SetBool("isOpen", isOpen);
        }
        else
        {
            OpenDoor();
            animator.SetBool("isOpen", isOpen);
        }
    }

    private void Update()
    {
        if (!isActive) return;
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            isActive = false;
            onInteractComplete();
        }
    }



    private void OpenDoor()
    {
        isOpen = true;
        gameObject.layer = (int)Open;
    }

    private void CloseDoor()
    {
        isOpen = false;
        gameObject.layer = 11;
    }

}
