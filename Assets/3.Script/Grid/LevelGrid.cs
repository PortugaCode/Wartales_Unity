using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    public static LevelGrid Instance { get; private set; }

    public event EventHandler OnAnyUnitMovedGridPosition;

    [SerializeField] private Transform debugGridPrefab;
    private GridSystem<GridObject> gridSystem;


    [SerializeField] private int width = 10;
    [SerializeField] private int height = 10;
    [SerializeField] private float cellSize = 2.0f;

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

        gridSystem = new GridSystem<GridObject>(width, height, cellSize, 
            (GridSystem<GridObject> g, GridPosition gridPosition) => new GridObject(g, gridPosition));
        //gridSystem.CreateDebugObjects(debugGridPrefab);
    }

    private void Start()
    {
        Pathfinding.Instance.SetUp(width, height, cellSize);
    }

    public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObjectArray(gridPosition);
        gridObject.AddUnit(unit);
    }

    public List<Unit> GetUnitAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObjectArray(gridPosition);
        return gridObject.GetUnit();
    }

    public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObjectArray(gridPosition);
        gridObject.RemoveUnit(unit);
    }

    public void UnitMoveGridPostion(Unit unit, GridPosition fromgridposition, GridPosition togridposition)
    {
        OnAnyUnitMovedGridPosition?.Invoke(this, EventArgs.Empty);
        RemoveUnitAtGridPosition(fromgridposition, unit);
        AddUnitAtGridPosition(togridposition, unit);
        OnAnyUnitMovedGridPosition?.Invoke(this, EventArgs.Empty);
    }

    public GridPosition GetGridPosition(Vector3 WorldPosition) => gridSystem.GetGridPosition(WorldPosition);
    public Vector3 GetWorldPosition(GridPosition gridPosition) => gridSystem.GetWorldPosition(gridPosition);
    public bool isValidGridPosition(GridPosition gridPosition) => gridSystem.isValidGridPosition(gridPosition);

    public int GetWidth() => gridSystem.GetWidth();
    public int GetHeight() => gridSystem.GetHeight();



    public bool HasAnyUnitOnGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObjectArray(gridPosition);
        return gridObject.HasAnyUnit();
    }

    public Unit GetAnyUnitOnGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObjectArray(gridPosition);
        return gridObject.HasGetUnit();
    }

    public Door GetDoorAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObjectArray(gridPosition);
        return gridObject.GetDoor();
    }

    public void SetDoorAtGridPosition(GridPosition gridPosition, Door door)
    {
        GridObject gridObject = gridSystem.GetGridObjectArray(gridPosition);
        gridObject.SetDoor(door);
    }

    public DestructibleCrate GetCrateAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObjectArray(gridPosition);
        return gridObject.GetCrate();
    }

    public void SetCrateAtGridPosition(GridPosition gridPosition, DestructibleCrate crate)
    {
        GridObject gridObject = gridSystem.GetGridObjectArray(gridPosition);
        gridObject.SetCrate(crate);
    }

    public Trap GetTrapAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObjectArray(gridPosition);
        return gridObject.GetTrap();
    }

    public void SetTrapAtGridPosition(GridPosition gridPosition, Trap trap)
    {
        GridObject gridObject = gridSystem.GetGridObjectArray(gridPosition);
        gridObject.SetTrap(trap);
    }
}
