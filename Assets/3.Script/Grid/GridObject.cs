using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    private GridSystem<GridObject> gridSystem;
    private GridPosition gridPosition;
    private List<Unit> units;
    private Door door;
    private DestructibleCrate crate;
    private Trap trap;

    public GridObject(GridSystem<GridObject> gridSystem, GridPosition gridPosition)
    {
        this.gridSystem = gridSystem;
        this.gridPosition = gridPosition;
        units = new List<Unit>();
    }

    public override string ToString()
    {
        string unitstrings = string.Empty;
        foreach(Unit unit in units)
        {
            unitstrings += unit.name + "\n";
        }
        return gridPosition.ToString() + "\n" + unitstrings;
    }

    public void AddUnit(Unit unit)
    {
        units.Add(unit);
    }

    public void RemoveUnit(Unit unit)
    {
        units.Remove(unit);
    }

    public List<Unit> GetUnit()
    {
        return units;
    }

    public bool HasAnyUnit()
    {
        return units.Count > 0;
    }

    public Unit HasGetUnit()
    {
        if(HasAnyUnit())
        {
            return units[0];
        }
        else
        {
            return null;
        }
    }

    public Door GetDoor()
    {
        return door;
    }

    public void SetDoor(Door door)
    {
        this.door = door;
    }

    public DestructibleCrate GetCrate()
    {
        return crate;
    }

    public void SetCrate(DestructibleCrate crate)
    {
        this.crate = crate;
    }

    public Trap GetTrap()
    {
        return trap;
    }

    public void SetTrap(Trap trap)
    {
        this.trap = trap;
    }
}
