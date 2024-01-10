using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }



    //[범위]
    [SerializeField] private GameObject range;


    //[이벤트 핸들러]
    public event EventHandler OnSelectedUnitChanged;
    public event EventHandler OnSelectedActionChanged;
    public event EventHandler OnActionStarted;
    public event EventHandler OnActionActive;

    //[Unit관련]
    [SerializeField] private Unit selectUnit;
    [SerializeField] private LayerMask UnitLayer;
    private BaseAction selectedAction;

    [SerializeField] private GameObject mousePosition;
    public GameObject MousePosition => mousePosition;
    [SerializeField] private Transform cameraTarget;
    public bool isCamMove;
    public bool needSetPosition;

    //[현재 Action중인지?]
    private bool isBusy;

    private Vector3 offset;
    private Vector3 targetPosition;
    private Vector3 currentVelocity = Vector3.zero;


    [Header("LineRenderer")]
    public LineRenderer lineRenderer;


    private void Awake()
    {
        #region [싱글톤]
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        #endregion

        
    }
    private void Start()
    {
        IsPlayerUnit[] select = FindObjectsOfType<IsPlayerUnit>();
        int rand = UnityEngine.Random.Range(0, select.Length);

        selectUnit = select[rand].GetComponent<Unit>();
        SetSelectUnit(selectUnit);
        ChangeUI();

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        ChangeUI();
    }

    private void FixedUpdate()
    {
        if (selectUnit == null) return;
        if (TurnSystem.Instance.IsPlayerTurn() && isCamMove)
        {
            if(needSetPosition)
            {
                targetPosition = selectUnit.GetWorldPosition();
                offset = targetPosition - selectUnit.GetWorldPosition();

                targetPosition = selectUnit.GetWorldPosition() + offset;

                needSetPosition = false;
            }

            cameraTarget.position = Vector3.SmoothDamp(cameraTarget.position, targetPosition, ref currentVelocity, 0.25f);
            if(Vector3.Distance(cameraTarget.position, targetPosition) <= 0.2f)
            {
                isCamMove = false;
            }
        }
    }

    private void Update()
    {
        if (isBusy) return;

        if (!TurnSystem.Instance.IsPlayerTurn()) return;

        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (TryHandleUnitSelection()) return;

        if (selectUnit == null) return;

        if (MouseWorld.Instance.isChangePosition())
        {
            SelectUnitMouseVisual();
        }




        if (selectedAction == selectUnit.GetAction<FireBallAction>())
        {
            range.SetActive(true);
            range.transform.position = MouseWorld.Instance.GetPoint() + Vector3.up * 0.005f;
        }
        else
        {
            range.SetActive(false);
        }

        HandleSelectAction();
    }

    public void SelectUnitMouseVisual()
    {
        GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.Instance.GetPoint());

        List<GridPosition> validGridPostionList = new List<GridPosition>();
        GridPosition unitGridPosition = selectUnit.GetGridPostion();
        int maxDistance = selectedAction.GetMaxDistance();

        for (int x = -maxDistance; x <= maxDistance; x++)
        {
            for (int z = -maxDistance; z <= maxDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                if (!LevelGrid.Instance.isValidGridPosition(testGridPosition))
                {
                    //그리드 안에서만 움직이게끔
                    continue;
                }

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > maxDistance && selectedAction != selectUnit.GetAction<SwordAction>())
                {
                    continue;
                }
                validGridPostionList.Add(testGridPosition);
            }
        }

        if (!validGridPostionList.Contains(mouseGridPosition))
        {
            lineRenderer.enabled = false;
            mousePosition.SetActive(false);
            return;
        }

        if (selectedAction.isValidActionGridPosition(mouseGridPosition))
        {
            mousePosition.SetActive(true);
            mousePosition.transform.position = LevelGrid.Instance.GetWorldPosition(mouseGridPosition) + Vector3.up * 0.02f;
            if (selectedAction == selectUnit.GetAction<MoveAction>())
            {
                DrawLineRenderer(mouseGridPosition);
            }
        }
    }

    private void DrawLineRenderer(GridPosition gridPosition)
    {
        if (selectUnit.isDie) return;

        lineRenderer.enabled = true;
        List<GridPosition> pathGridPositionList = Pathfinding.Instance.FindPath(selectUnit.GetGridPostion(), gridPosition, out int pathLegth);
        Vector3[] pathVectorPositionList = new Vector3[pathGridPositionList.Count];
        

        for (int i = 0; i < pathGridPositionList.Count; i++)
        {
            pathVectorPositionList[i] = LevelGrid.Instance.GetWorldPosition(pathGridPositionList[i]) + Vector3.up * 0.02f;
        }
        //=======================================================================
        var pointList = new List<Vector3>();
       
        float vertexCount = 12f;
        Vector3 point1;
        Vector3 point2;
        Vector3 point3;
        
        if(pathVectorPositionList.Length <= 2)
        {
            lineRenderer.positionCount = pathVectorPositionList.Length;
            lineRenderer.SetPositions(pathVectorPositionList);
            return;
        }

        pointList.Add(pathVectorPositionList[0]);
        for (int i = 0; i < pathVectorPositionList.Length -2; i++)
        {
            point1 = pointList[pointList.Count-1];
            point2 = pathVectorPositionList[i+1];
            point3 = pathVectorPositionList[i+2];

            for (float ratio = 0f; ratio < 1f; ratio += 1f / vertexCount)
            {
                if (ratio >= 0.8f && i < pathVectorPositionList.Length - 3) {
                    break;
                }
                var tangent1 = Vector3.Lerp(point1, point2, ratio);
                var tangent2 = Vector3.Lerp(point2, point3, ratio);
                var curve = Vector3.Lerp(tangent1, tangent2, ratio);
                pointList.Add(curve);
            }
        }
        pointList.Add(pathVectorPositionList[pathVectorPositionList.Length - 1]);
        lineRenderer.positionCount = pointList.Count;
        lineRenderer.SetPositions(pointList.ToArray());
    }

    private void HandleSelectAction()
    {
        if(Input.GetMouseButtonDown(0))
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.Instance.GetPoint());
            if (!selectedAction.isValidActionGridPosition(mouseGridPosition)) return;

            if (!selectUnit.TrySpendActionPointsToTakeAction(selectedAction)) return;

            mousePosition.SetActive(false);

            SetBusy();
            selectedAction.TakeAction(mouseGridPosition, ClearBusy);

            OnActionActive?.Invoke(this, EventArgs.Empty);
            OnActionStarted?.Invoke(this, EventArgs.Empty);
        }
    }
    private void SetBusy()
    {
        isBusy = true;
        //Debug.Log("NowBusy!");
    }

    private void ClearBusy()
    {
        isBusy = false;
        //Debug.Log("ClearBusy!");
    }

    private bool TryHandleUnitSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, UnitLayer))
            {
                if (hit.transform.TryGetComponent<Unit>(out Unit Unit))
                {
                    if(Unit == selectUnit)
                    {
                        return false;
                    }

                    if(Unit.IsEnemy())
                    {
                        return false;
                    }

                    SetSelectUnit(Unit);
                    return true;
                }
            }
        }
        return false;
    }


    public void SetSelectUnit(Unit unit)
    {
        if (isBusy) return;
        selectUnit = unit;
        SetSelectAction(unit.GetAction<MoveAction>());
        ChangeUI();
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetSelectAction(BaseAction baseAction)
    {
        if (isBusy) return;
        selectedAction = baseAction;


        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetSelectedUnit()
    {
        return selectUnit;
    }
    public BaseAction GetSelectAction()
    {
        return selectedAction;
    }

    private void ChangeUI()
    {

        if(!TurnSystem.Instance.IsPlayerTurn())
        {
            for (int i = 0; i < selectUnit.GetUiObject().transform.childCount; i++)
            {
                selectUnit.GetUiObject().transform.GetChild(i).transform.gameObject.SetActive(false);
            }
            selectUnit.GetUiObject().transform.GetChild(4).transform.gameObject.SetActive(true);

            lineRenderer.enabled = false;
            mousePosition.SetActive(false);

            return;
        }


        #region [UI SetActive]
        if (selectUnit.isAchor)
        {
            for (int i = 0; i < selectUnit.GetUiObject().transform.childCount; i++)
            {
                selectUnit.GetUiObject().transform.GetChild(i).transform.gameObject.SetActive(false);
            }
            selectUnit.GetUiObject().transform.GetChild(0).transform.gameObject.SetActive(true);
        }
        else if (selectUnit.isWarrior)
        {
            for (int i = 0; i < selectUnit.GetUiObject().transform.childCount; i++)
            {
                selectUnit.GetUiObject().transform.GetChild(i).transform.gameObject.SetActive(false);
            }
            selectUnit.GetUiObject().transform.GetChild(1).transform.gameObject.SetActive(true);
        }
        else if (selectUnit.isWizard)
        {
            for (int i = 0; i < selectUnit.GetUiObject().transform.childCount; i++)
            {
                selectUnit.GetUiObject().transform.GetChild(i).transform.gameObject.SetActive(false);
            }
            selectUnit.GetUiObject().transform.GetChild(2).transform.gameObject.SetActive(true);
        }
        else if (selectUnit.isRogue)
        {
            for (int i = 0; i < selectUnit.GetUiObject().transform.childCount; i++)
            {
                selectUnit.GetUiObject().transform.GetChild(i).transform.gameObject.SetActive(false);
            }
            selectUnit.GetUiObject().transform.GetChild(3).transform.gameObject.SetActive(true);
        }
        #endregion
    }

    public void UndateSelectVisual() => OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
}
