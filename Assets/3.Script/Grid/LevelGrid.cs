using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    public static LevelGrid Instance { get; private set; }


    [SerializeField] private Transform debugGridPrefab;
    private GridSystem gridSystem;


    [SerializeField] private int width = 10;
    [SerializeField] private int height = 10;
    [SerializeField] private float cellSize = 2.0f;

    private void Awake()
    {
        #region [½Ì±ÛÅæ]
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        #endregion

        gridSystem = new GridSystem(width, height, cellSize);
        gridSystem.CreateDebugObjects(debugGridPrefab);
    }

    public void SetUnitAtGridPosition(GridPosition gridPosition, UnitMove unit)
    {
        GridObject gridObject = gridSystem.GetGridObjectArray(gridPosition);
        gridObject.SetUnit(unit);
    }

    public UnitMove GetUnitAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObjectArray(gridPosition);
        return gridObject.GetUnit();
    }

    public void ClearUnitAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObjectArray(gridPosition);
        gridObject.SetUnit(null);
    }

    public GridPosition GetGridPosition(Vector3 WorldPosition) => gridSystem.GetGridPosition(WorldPosition);
}
