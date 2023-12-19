using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionSystem : MonoBehaviour
{
    [SerializeField] private UnitMove selectUnit;
    [SerializeField] private LayerMask UnitLayer;

    private void Update()
    {
        if (TryHandleUnitSelection()) return;

        if(Input.GetMouseButtonDown(0))
        {
            selectUnit.Move(MouseWorld.Instance.GetPoint());
        }
    }

    private bool TryHandleUnitSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, UnitLayer))
        {
            if(hit.transform.TryGetComponent<UnitMove>(out UnitMove Unit))
            {
                selectUnit = Unit;
                return true;
            }
        }
        return false;
    }
}
