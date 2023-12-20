using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    private GridSystem gridSystem;
    private GridPosition gridPosition;
    private UnitMove unitMove;

    public GridObject(GridSystem gridSystem, GridPosition gridPosition)
    {
        this.gridSystem = gridSystem;
        this.gridPosition = gridPosition;
    }

    public override string ToString()
    {
        return gridPosition.ToString() + "\n" + unitMove;
    }

    public void SetUnit(UnitMove unit)
    {
        unitMove = unit;
    }

    public UnitMove GetUnit()
    {
        return unitMove;
    }
}
