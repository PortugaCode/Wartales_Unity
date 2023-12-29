using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance = null;

    private List<Unit> unitList;
    [SerializeField] private List<Unit> friendlyUnitList;
    [SerializeField] private List<Unit> enemyUnitList;


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

        unitList = new List<Unit>();
        friendlyUnitList = new List<Unit>();
        enemyUnitList = new List<Unit>();
    }

    private void Start()
    {
        Unit.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned;
        Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;
    }


    private void Unit_OnAnyUnitSpawned(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;

        if(unit != null)
        {
            unitList.Add(unit);

            if (unit.IsEnemy())
            {
                enemyUnitList.Add(unit);
            }
            else
            {
                friendlyUnitList.Add(unit);
            }
        }
    }

    private void Unit_OnAnyUnitDead(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;

        if (unit != null)
        {
            unitList.Remove(unit);

            if (unit.IsEnemy())
            {
                enemyUnitList.Remove(unit);
            }
            else
            {
                friendlyUnitList.Remove(unit);
            }
        }
    }

    public List<Unit> GetUnitList()
    {
        return unitList;
    }

    public List<Unit> GetFriendlyUnitList()
    {
        return friendlyUnitList;
    }
    public List<Unit> GetEnemyUnitList()
    {
        return enemyUnitList;
    }

}
