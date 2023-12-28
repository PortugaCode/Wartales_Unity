using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.Instance.GetPoint());
            GridPosition startGridPosition = new GridPosition(0, 0);

            List<GridPosition> gridPositionList = Pathfinding.Instance.FindPath(startGridPosition, mouseGridPosition);

            if(gridPositionList == null)
            {
                Debug.Log("gridPositionList null");
            }
            else
            {
                for (int i = 0; i < gridPositionList.Count - 1; i++)
                {
                    Debug.DrawLine(
                        LevelGrid.Instance.GetWorldPosition(gridPositionList[i]),
                        LevelGrid.Instance.GetWorldPosition(gridPositionList[i + 1]),
                        Color.white,
                        10f
                        );
                }
            }


        }
    }
}


