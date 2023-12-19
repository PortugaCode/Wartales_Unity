using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridDebutObject : MonoBehaviour
{
    [SerializeField] private TextMeshPro debugText;
    private GridObject gridObject;

    public void SetGridObject(GridObject gridObject)
    {
        this.gridObject = gridObject;
    }

    private void Update()
    {
        debugText.text = gridObject.ToString();
    }
}
