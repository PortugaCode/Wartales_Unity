using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainerTransform;
    [SerializeField] private TextMeshProUGUI textMeshPro;

    private List<ActionButtonUI> actionButtonUIList;

    private void Awake()
    {
        actionButtonUIList = new List<ActionButtonUI>();
    }


    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectUnitChange;
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectActionChange;
        UnitActionSystem.Instance.OnActionStarted += UpdateActionPoints_OnActionChanged;
        UnitActionSystem.Instance.OnActionActive += UpdateActionPoints_OnActionActive;
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;

        CreateUnitActionButton();
        UndateSelectVisual();
        UpdateActionPoints();
    }

    private void UpdateActionPoints_OnActionActive(object sender, EventArgs e)
    {
        UndateSelectVisual();
        UnitActionSystem.Instance.lineRenderer.enabled = false;
    }

    private void CreateUnitActionButton()
    {
        foreach(Transform button in actionButtonContainerTransform)
        {
            Destroy(button.gameObject);
        }
        actionButtonUIList.Clear();
        

        Unit selectUnit = UnitActionSystem.Instance.GetSelectedUnit();

        foreach(BaseAction baseAction in selectUnit.GetBaseActionsArray())
        {
            Transform actionButtonTransform = Instantiate(actionButtonPrefab, actionButtonContainerTransform);
            ActionButtonUI actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI>();
            actionButtonUI.SetBaseAction(baseAction);

            actionButtonUIList.Add(actionButtonUI);
        }
    }

    private void UnitActionSystem_OnSelectUnitChange(object sender, EventArgs e)
    {
        CreateUnitActionButton();
        UpdateActionPoints();
        UndateSelectVisual();
    }

    private void UnitActionSystem_OnSelectActionChange(object sender, EventArgs e)
    {
        UndateSelectVisual();
        UnitActionSystem.Instance.lineRenderer.enabled = false;
    }

    private void UndateSelectVisual()
    {
        foreach(ActionButtonUI actionButtonUI in actionButtonUIList)
        {
            actionButtonUI.UpdateSelectVisual();
        }
    }

    public void UpdateActionPoints_OnActionChanged(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }

    private void UpdateActionPoints()
    {
        Unit selectUnit = UnitActionSystem.Instance.GetSelectedUnit();
        textMeshPro.text = $"Action Points : {selectUnit.GetActionPoints()}";
    }
    
    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if (TurnSystem.Instance.IsPlayerTurn())
        {
            textMeshPro.gameObject.SetActive(true);
        }
        else
        {
            textMeshPro.gameObject.SetActive(false);
        }
        UpdateActionPoints();
    }

    private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }
}
