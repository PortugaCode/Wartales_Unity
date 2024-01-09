using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public static Pathfinding Instance;

    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    [SerializeField] private Transform debugGridPrefab;
    [SerializeField] private LayerMask obstaclesLayerMask;
    [SerializeField] private LayerMask unitLayerMask;
    private LayerMask cannotWalkLayerMasks;

    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float cellSize;

    private GridSystem<PathNode> gridSystem;

    private void Awake()
    {
        #region[½Ì±ÛÅæ]
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        #endregion
    }


    public void SetUp(int width, int height, float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridSystem = new GridSystem<PathNode>(width, height, cellSize,
                    (GridSystem<PathNode> g, GridPosition gridPosition) => new PathNode(gridPosition));

        //gridSystem.CreateDebugObjects(debugGridPrefab);


        cannotWalkLayerMasks = obstaclesLayerMask | unitLayerMask;

        SetWalkablePathNode();
        SetWalkablePathNode_Obstacles();
    }

    private void SetWalkablePathNode()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GetNode(x, z).SetWalkable(true);
                GridPosition gridPosition = new GridPosition(x, z);
                Vector3 worldPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
                float raycastOffsetDistance = 5.0f;

                if (Physics.Raycast(worldPosition + Vector3.down * raycastOffsetDistance,
                                Vector3.up, raycastOffsetDistance * 2f,
                                cannotWalkLayerMasks))
                {
                    GetNode(x, z).SetWalkable(false);
                }
            }
        }
    }

    private void SetWalkablePathNode_Obstacles()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GetNode(x, z).SetWalkable_ObstaclesLayerMask(true);
                GridPosition gridPosition = new GridPosition(x, z);
                Vector3 worldPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
                float raycastOffsetDistance = 5.0f;

                if (Physics.Raycast(worldPosition + Vector3.down * raycastOffsetDistance,
                                Vector3.up, raycastOffsetDistance * 2f,
                                obstaclesLayerMask))
                {
                    GetNode(x, z).SetWalkable_ObstaclesLayerMask(false);
                }
            }
        }
    }


    public List<GridPosition> FindPath(GridPosition startGridPosition, GridPosition endGridPosition, out int pathLegth)
    {
        //SetWalkablePathNode();
        //SetWalkablePathNode_Obstacles();
        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closedList = new List<PathNode>();

        PathNode startNode = gridSystem.GetGridObjectArray(startGridPosition);
        PathNode endNode = gridSystem.GetGridObjectArray(endGridPosition);
        openList.Add(startNode);

        for(int x = 0; x < gridSystem.GetWidth(); x++)
        {
            for(int z = 0; z < gridSystem.GetHeight(); z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                PathNode pathNode = gridSystem.GetGridObjectArray(gridPosition);

                pathNode.SetGCost(int.MaxValue);
                pathNode.SetHCost(0);
                pathNode.CalculateFCost();
                pathNode.ResetCameFromPathNode();
            }
        }

        startNode.SetGCost(0);
        startNode.SetHCost(CalculateDistance(startGridPosition, endGridPosition));
        startNode.CalculateFCost();

        while(openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostPathNode(openList);

            if(currentNode == endNode)
            {
                //¸¶Áö¸· ³ëµå Ã£À½
                pathLegth = endNode.GetFCost();
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach(PathNode neighbourNode in GetNeighbourList(currentNode))
            {
                if(closedList.Contains(neighbourNode))
                {
                    continue;
                }

                if(!neighbourNode.IsWalkable())
                {
                    closedList.Add(neighbourNode);
                    continue;
                }

                if (!DiagonalWalkable(currentNode, neighbourNode))
                {
                    continue;
                }

                int tentativeGCost = 
                    currentNode.GetGCost() 
                    + CalculateDistance(currentNode.GetGridPosition(),
                    neighbourNode.GetGridPosition());

                if(tentativeGCost < neighbourNode.GetGCost())
                {
                    neighbourNode.SetCameFromPathNode(currentNode);
                    neighbourNode.SetGCost(tentativeGCost);
                    neighbourNode.SetHCost(CalculateDistance(neighbourNode.GetGridPosition(), endGridPosition));
                    neighbourNode.CalculateFCost();

                    if(!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }
        pathLegth = 0;
        return null;
    }



    private bool DiagonalWalkable(PathNode curnode, PathNode neighbourNode)
    {
        GridPosition CurNodeGridPosition = curnode.GetGridPosition();
        GridPosition NeighbourNodeGridPosition = neighbourNode.GetGridPosition();

        if (!GetNode(CurNodeGridPosition.x, NeighbourNodeGridPosition.z).IsWalkable_ObstaclesLayerMask()) return false;

        if (!GetNode(NeighbourNodeGridPosition.x, CurNodeGridPosition.z).IsWalkable_ObstaclesLayerMask()) return false;



        return true;
    }


    private List<GridPosition> CalculatePath(PathNode endNode)
    {
        List<PathNode> pathNodeList = new List<PathNode>();
        pathNodeList.Add(endNode);
        PathNode currentNode = endNode;

        while(currentNode.GetCameFromPathNode() != null)
        {
            pathNodeList.Add(currentNode.GetCameFromPathNode());
            currentNode = currentNode.GetCameFromPathNode();
        }

        pathNodeList.Reverse();

        List<GridPosition> gridPositionList = new List<GridPosition>();
        foreach(PathNode pathNode in pathNodeList)
        {
            gridPositionList.Add(pathNode.GetGridPosition());
        }

        return gridPositionList;
    }

    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();

        GridPosition gridPosition = currentNode.GetGridPosition();

        #region [Left]
        if (gridPosition.x - 1 >= 0)
        {
            //Left
            neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 0));

            if(gridPosition.z + 1 < gridSystem.GetHeight())
            {
                //Left Up
                neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 1));
            }

            if(gridPosition.z - 1 >= 0)
            {
                //Left Down
                neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z - 1));
            }
        }
        #endregion

        #region[Right]
        if (gridPosition.x + 1 < gridSystem.GetWidth())
        {
            //Right
            neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 0));

            if (gridPosition.z + 1 < gridSystem.GetHeight())
            {
                //Right Up
                neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 1));
            }

            if (gridPosition.z - 1 >= 0)
            {
                //Right Down
                neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z - 1));
            }
        }
        #endregion

        if (gridPosition.z + 1 < gridSystem.GetHeight())
        {
            //Up
            neighbourList.Add(GetNode(gridPosition.x + 0, gridPosition.z + 1));
        }

        if (gridPosition.z - 1 >= 0)
        {
            //Down
            neighbourList.Add(GetNode(gridPosition.x + 0, gridPosition.z - 1));
        }

        return neighbourList;
    }

    private PathNode GetNode(int x, int z)
    {
        return gridSystem.GetGridObjectArray(new GridPosition(x, z));
    }

    public int CalculateDistance(GridPosition gridPositionA, GridPosition gridPositionB)
    {
        #region [ÈÞ¸®½ºÆ½ °è»ê¹ý]
        //ÈÞ¸®½ºÆ½ °è»ê ¹æ¹ý
        GridPosition gridPositionDistance = gridPositionA - gridPositionB;
        //int distance = Mathf.Abs(gridPositionDistance.x) + Mathf.Abs(gridPositionDistance.z);


        int xDistance = Mathf.Abs(gridPositionDistance.x);
        int zDistance = Mathf.Abs(gridPositionDistance.z);
        int remaining = Math.Abs(xDistance - zDistance);

        // HCost = MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remaining
        #endregion

        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remaining;

    }

    private PathNode GetLowestFCostPathNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCostPathNode = pathNodeList[0];
        for(int i = 0; i < pathNodeList.Count; i++)
        {
            if(pathNodeList[i].GetFCost() < lowestFCostPathNode.GetFCost())
            {
                lowestFCostPathNode = pathNodeList[i];
            }
        }
        return lowestFCostPathNode;
    }

    public bool IsWalkableGridPosition(GridPosition gridPosition)
    {
        return gridSystem.GetGridObjectArray(gridPosition).IsWalkable();
    }

    public bool HasPath(GridPosition startGridPosition, GridPosition endGridPosition)
    {
        return FindPath(startGridPosition, endGridPosition, out int pathLegth) != null;
    }

    public int PathLength(GridPosition startGridPosition, GridPosition endGridPosition)
    {
        FindPath(startGridPosition, endGridPosition, out int pathLegth);
        return pathLegth;
    }


    public LayerMask GetCannotWalkLayerMasks()
    {
        return cannotWalkLayerMasks;
    }

    public void SetIsWalkableGridPosition(GridPosition gridPosition, bool value)
    {
        gridSystem.GetGridObjectArray(gridPosition).SetWalkable(value);
    }
}
