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
    [SerializeField] private Image warriorIcon;
    [SerializeField] private Image achorIcon;
    [SerializeField] private Image wizardIcon;
    [SerializeField] private Image rogueIcon;

    [SerializeField] private Color enemy;

    private void Start()
    {
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
        if (unit.IsEnemy())
        {
            enemyIcon.enabled = unit.IsEnemy();
            healthBar.color = enemy;
        }
        else
        {
            warriorIcon.enabled = unit.isWarrior;
            achorIcon.enabled = unit.isAchor;
            wizardIcon.enabled = unit.isWizard;
            rogueIcon.enabled = unit.isRogue;
        }
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
