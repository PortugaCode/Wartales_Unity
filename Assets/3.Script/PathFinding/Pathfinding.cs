using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{

    [SerializeField] private Transform debugGridPrefab;

    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float cellSize;

    private GridSystem<PathNode> gridSystem;

    private void Awake()
    {
        gridSystem = new GridSystem<PathNode>(width, height, cellSize, 
            (GridSystem<PathNode> g, GridPosition gridPosition) => new PathNode(gridPosition));

        gridSystem.CreateDebugObjects(debugGridPrefab);
    }
}
