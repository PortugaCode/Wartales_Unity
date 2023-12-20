using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }


    public event EventHandler OnSelectedUnitChanged;

    [SerializeField] private Unit selectUnit;
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
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.Instance.GetPoint());
            if(selectUnit.GetMoveAction().isValidActionGridPosition(mouseGridPosition))
            {
                selectUnit.GetMoveAction().Move(mouseGridPosition);
            }
        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            selectUnit.GetSpinAction().Spin();
        }
    }

    private bool TryHandleUnitSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, UnitLayer))
        {
            if(hit.transform.TryGetComponent<Unit>(out Unit Unit))
            {
                SetSelectUnit(Unit);
                return true;
            }
        }
        return false;
    }


    private void SetSelectUnit(Unit unit)
    {
        selectUnit = unit;
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetSelectedUnit()
    {
        return selectUnit;
    }
}
