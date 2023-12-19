using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance;


    public event EventHandler OnSelectedUnitChanged;

    [SerializeField] public UnitMove selectUnit;
    [SerializeField] private LayerMask UnitLayer;

    private void Awake()
    {
        #region [ΩÃ±€≈Ê]
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        #endregion
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (TryHandleUnitSelection()) return;
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
                SetSelectUnit(Unit);
                return true;
            }
        }
        return false;
    }


    private void SetSelectUnit(UnitMove unit)
    {
        selectUnit = unit;
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public UnitMove GetSelectedUnit()
    {
        return selectUnit;
    }
}
