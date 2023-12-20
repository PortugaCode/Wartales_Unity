using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    private GridSystem gridSystem;
    private GridPosition gridPosition;
    private List<UnitMove> units;

    public GridObject(GridSystem gridSystem, GridPosition gridPosition)
    {
        this.gridSystem = gridSystem;
        this.gridPosition = gridPosition;
        units = new List<UnitMove>();
    }

    public override string ToString()
    {
        string unitstrings = string.Empty;
        foreach(UnitMove unit in units)
        {
            unitstrings += unit + "\n";
        }
        return gridPosition.ToString() + "\n" + unitstrings;
    }

    public void AddUnit(UnitMove unit)
    {
        units.Add(unit);
    }

    public void RemoveUnit(UnitMove unit)
    {
        units.Remove(unit);
    }

    public List<UnitMove> GetUnit()
    {
        return units;
    }
}
