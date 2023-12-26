using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UnitWorldUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI actionPointsText;
    [SerializeField] private Unit unit;
    [SerializeField] private Image healthBar;
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private Image enemyIcon;

    private void Start()
    {
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
        enemyIcon.enabled = unit.IsEnemy();
        unit.OnDamage += Unit_OnDamage;
        unit.OnDie += Unit_OnDamage;
        UpdateHealthBar();
        UpdateActionPoitsText();
    }

    private void Unit_OnDamage(object sender, EventArgs e)
    {
        UpdateHealthBar();
    }

    private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e)
    {
        UpdateActionPoitsText();
    }

    private void UpdateActionPoitsText()
    {
        actionPointsText.text = unit.GetActionPoints().ToString();
    }

    private void UpdateHealthBar()
    {
        healthBar.fillAmount = healthSystem.GetHealthNormalized();
    }


}
