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


    private GridPosition gridPosition;

    [SerializeField] private ShowAndHide showAndHide;
    [SerializeField] private int stage;

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

        AudioManager.Instance.OpenDoorSoundPlay();

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
        showAndHide.ShowStage(stage);
        gameObject.layer = 0;
        Pathfinding.Instance.SetISWalkableGridPosition_Crate();
    }

    private void CloseDoor()
    {
        isOpen = false;
        showAndHide.HideStage(stage);
        gameObject.layer = 11;
        Pathfinding.Instance.SetISWalkableGridPosition_Crate();
    }

}
