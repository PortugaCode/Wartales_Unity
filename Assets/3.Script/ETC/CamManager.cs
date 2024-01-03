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

            case SwordAction swordAction:
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

                if(targetUnit.GetHealthSystem().Gethealth() <= shootAction.GetDamage())
                {
                    Vector3 CamHeight = Vector3.up * 1.7f;

                    Vector3 shootdir = (targetUnit.GetWorldPosition() - shooterUnit.GetWorldPosition()).normalized;
                    float shoulderOffsetAmount = 0.7f;
                    Vector3 shoulderOffset = Quaternion.Euler(0, 90f, 0f) * shootdir * shoulderOffsetAmount;

                    Vector3 actionCamPosition =
                    shooterUnit.GetWorldPosition() + CamHeight + shoulderOffset + (shootdir * -2);

                    actionCameraGameObject.transform.position = actionCamPosition;
                    actionCameraGameObject.transform.LookAt(targetUnit.GetWorldPosition() + CamHeight);
                    ShowActionCam();
                }
                break;

            case SwordAction swordAction:
                Unit attackUnit = swordAction.GetUnit();
                Unit targetUnit_Sword = swordAction.GetTargetUnit();

                if (targetUnit_Sword.GetHealthSystem().Gethealth() <= swordAction.intdamage)
                {
                    Vector3 CamHeight = Vector3.up * 1.7f;

                    Vector3 dir = (targetUnit_Sword.GetWorldPosition() - attackUnit.GetWorldPosition()).normalized;
                    float shoulderOffsetAmount = 0.7f;
                    Vector3 shoulderOffset = Quaternion.Euler(0, 90f, 0f) * dir * shoulderOffsetAmount;

                    Vector3 actionCamPosition =
                    attackUnit.GetWorldPosition() + CamHeight + shoulderOffset + (dir * -2);

                    actionCameraGameObject.transform.position = actionCamPosition;
                    actionCameraGameObject.transform.LookAt(targetUnit_Sword.GetWorldPosition() + CamHeight);
                    ShowActionCam();
                }
                break;
        }
    }

    private void ShowActionCam()
    {
        actionCameraGameObject.SetActive(true);
        Unit[] units = FindObjectsOfType<Unit>();
        foreach(Unit unit in units)
        {
            unit.UnitUI.SetActive(false);
        }
    }

/*    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.7f);

        Unit[] units = FindObjectsOfType<Unit>();
        foreach (Unit unit in units)
        {
            unit.UnitUI.SetActive(true);
        }
    }*/

    private void HideActionCam()
    {
        actionCameraGameObject.SetActive(false);
        Unit[] units = FindObjectsOfType<Unit>();
        foreach (Unit unit in units)
        {
            unit.UnitUI.SetActive(true);
        }
    }
}
