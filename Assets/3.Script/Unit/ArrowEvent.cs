using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowEvent : MonoBehaviour
{
    public GameObject arrow;
    public ShootAction shootAction;
    public Transform arrowPivot;

    public void CreateArro()
    {
        Vector3 direction = (shootAction.GetTargetUnit().GetWorldPosition() - shootAction.GetUnit().GetWorldPosition()).normalized;
        GameObject arrowClone = Instantiate(arrow, arrowPivot.position, Quaternion.LookRotation(direction));

    }

}
