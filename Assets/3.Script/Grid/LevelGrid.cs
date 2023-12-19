using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    [SerializeField] private Transform debugGridPrefab;
    private GridSystem gridSystem;

    [SerializeField] private int width = 10;
    [SerializeField] private int height = 10;
    [SerializeField] private float cellSize = 2.0f;

    private void Awake()
    {
        gridSystem = new GridSystem(width, height, cellSize);
        gridSystem.CreateDebugObjects(debugGridPrefab);
    }

    public void SetUnitAtGridPosition(GridPosition gridPosition, UnitMove unit)
    {
        //GridObject gridObject = gridSystem.
    }

/*    public UnitMove GetUnitAtGridPosition(GridPosition gridPosition)
    {
        
    }*/

    public void ClearUnitAtGridPosition(GridPosition gridPosition)
    {

    }
}
