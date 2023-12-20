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

    private bool isBusy;


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

    private void Start()
    {
        ChangeUI();
    }
    private void Update()
    {

        if (isBusy) return;

        if(Input.GetMouseButtonDown(0))
        {
            if (TryHandleUnitSelection()) return;
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.Instance.GetPoint());
            if(selectUnit.GetMoveAction().isValidActionGridPosition(mouseGridPosition))
            {
                SetBusy();
                selectUnit.GetMoveAction().Move(mouseGridPosition, ClearBusy);
            }
        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            SetBusy();
            selectUnit.GetSpinAction().Spin(ClearBusy);
        }
    }

    private void SetBusy()
    {
        isBusy = true;
        Debug.Log("NowBusy!");
    }

    private void ClearBusy()
    {
        isBusy = false;
        Debug.Log("ClearBusy!");
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
        ChangeUI();
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetSelectedUnit()
    {
        return selectUnit;
    }

    private void ChangeUI()
    {
        #region [UI SetActive]
        if (selectUnit.isAchor)
        {
            for (int i = 0; i < selectUnit.UI.transform.childCount; i++)
            {
                selectUnit.UI.transform.GetChild(i).transform.gameObject.SetActive(false);
            }
            selectUnit.UI.transform.GetChild(0).transform.gameObject.SetActive(true);
        }
        else if (selectUnit.isWarrior)
        {
            for (int i = 0; i < selectUnit.UI.transform.childCount; i++)
            {
                selectUnit.UI.transform.GetChild(i).transform.gameObject.SetActive(false);
            }
            selectUnit.UI.transform.GetChild(1).transform.gameObject.SetActive(true);
        }
        #endregion
    }
}
