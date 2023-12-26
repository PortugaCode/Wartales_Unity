using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamManager : MonoBehaviour
{
    [SerializeField] private GameObject actionCameraGameObject;

    private void Start()
    {
        BaseAction.OnAnyActionStarted += BaseAction_OnAnyActionStarted;
        BaseAction.OnAnyActionCompleted += BaseAction_OnAnyActionCompleted;

        HideActionCam();
    }

    private void BaseAction_OnAnyActionCompleted(object sender, EventArgs e)
    {
        switch (sender)
        {
            case ShootAction shootAction:
                HideActionCam();
                break;
        }
    }

    private void BaseAction_OnAnyActionStarted(object sender, EventArgs e)
    {
        switch (sender)
        {
            case ShootAction shootAction:
                Unit shooterUnit = shootAction.GetUnit();
                Unit targetUnit = shootAction.GetTargetUnit();

                Vector3 CamHeight = Vector3.up * 1.7f;

                Vector3 shootdir = (targetUnit.GetWorldPosition() - shooterUnit.GetWorldPosition()).normalized;
                float shoulderOffsetAmount = 0.7f;
                Vector3 shoulderOffset = Quaternion.Euler(0, 90f, 0f) * shootdir * shoulderOffsetAmount;

                Vector3 actionCamPosition =
                shooterUnit.GetWorldPosition() + CamHeight + shoulderOffset + (shootdir * -4);

                actionCameraGameObject.transform.position = actionCamPosition;
                actionCameraGameObject.transform.LookAt(targetUnit.GetWorldPosition() + CamHeight);
                ShowActionCam();
                break;
        }
    }

    private void ShowActionCam()
    {
        actionCameraGameObject.SetActive(true);
    }

    private void HideActionCam()
    {
        actionCameraGameObject.SetActive(false);
    }
}
