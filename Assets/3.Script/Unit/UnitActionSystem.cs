using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }
    
    //[이벤트 핸들러]
    public event EventHandler OnSelectedUnitChanged;
    public event EventHandler OnSelectedActionChanged;
    public event EventHandler OnActionStarted;

    //[Unit관련]
    [SerializeField] private Unit selectUnit;
    [SerializeField] private LayerMask UnitLayer;
    private BaseAction selectedAction;



    //[현재 Action중인지?]
    private bool isBusy;


    private void Awake()
    {
        #region [싱글톤]
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
        SetSelectUnit(selectUnit);
        ChangeUI();
    }

    private void Update()
    {
        if (isBusy) return;

        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (TryHandleUnitSelection()) return;

        HandleSelectAction();
    }

    private void HandleSelectAction()
    {
        if(Input.GetMouseButtonDown(0))
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.Instance.GetPoint());
            if (!selectedAction.isValidActionGridPosition(mouseGridPosition)) return;

            if (!selectUnit.TrySpendActionPointsToTakeAction(selectedAction)) return;

            SetBusy();
            selectedAction.TakeAction(mouseGridPosition, ClearBusy);

            OnActionStarted?.Invoke(this, EventArgs.Empty);
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
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, UnitLayer))
            {
                if (hit.transform.TryGetComponent<Unit>(out Unit Unit))
                {
                    SetSelectUnit(Unit);
                    return true;
                }
            }
        }
        return false;
    }


    private void SetSelectUnit(Unit unit)
    {
        if (isBusy) return;
        selectUnit = unit;
        SetSelectAction(unit.GetMoveAction());
        ChangeUI();
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetSelectAction(BaseAction baseAction)
    {
        if (isBusy) return;
        selectedAction = baseAction;


        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetSelectedUnit()
    {
        return selectUnit;
    }
    public BaseAction GetSelectAction()
    {
        return selectedAction;
    }

    private void ChangeUI()
    {
        #region [UI SetActive]
        if (selectUnit.isAchor)
        {
            for (int i = 0; i < selectUnit.GetUiObject().transform.childCount; i++)
            {
                selectUnit.GetUiObject().transform.GetChild(i).transform.gameObject.SetActive(false);
            }
            selectUnit.GetUiObject().transform.GetChild(0).transform.gameObject.SetActive(true);
        }
        else if (selectUnit.isWarrior)
        {
            for (int i = 0; i < selectUnit.GetUiObject().transform.childCount; i++)
            {
                selectUnit.GetUiObject().transform.GetChild(i).transform.gameObject.SetActive(false);
            }
            selectUnit.GetUiObject().transform.GetChild(1).transform.gameObject.SetActive(true);
        }
        #endregion
    }
}
