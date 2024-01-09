using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    private const float MIN_FOLLOW_Y_OFFSET = 2f;
    private const float MAX_FOLLOW_Y_OFFSET = 14f;


    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    private CinemachineTransposer cinemachineTransposer;
    private Vector3 followOffset;
    [SerializeField] private float zoomPower;

    //==============================================================
    [SerializeField] private float cameraSpeed;
    [SerializeField] private float rotationSpeed;


    private float x;
    private float z;

    private Vector3 rotationVector;

    private void Awake()
    {
        cinemachineTransposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        followOffset = cinemachineTransposer.m_FollowOffset;
    }

    private void Start()
    {
        int r = Random.Range(0, UnitManager.Instance.GetFriendlyUnitList().Count);
        transform.position = UnitManager.Instance.GetFriendlyUnitList()[r].GetWorldPosition();
    }


    private void Update()
    {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
        rotationVector = new Vector3(0, 0, 0);


        if (Input.GetMouseButton(1))
        {
            RotationCam();
        }
        ZoomCam();
    }

    private void FixedUpdate()
    {
        //카메라 움직임
        transform.position += transform.forward * z * cameraSpeed * Time.deltaTime 
            + transform.right * x * cameraSpeed * Time.deltaTime;

        //카메라 로테이션
        transform.eulerAngles += rotationVector * rotationSpeed * Time.deltaTime;

        //카메라 줌
        followOffset.y = Mathf.Clamp(followOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);
        cinemachineTransposer.m_FollowOffset = Vector3.Lerp(cinemachineTransposer.m_FollowOffset, followOffset, zoomPower * Time.deltaTime);
    }

    private void RotationCam()
    {
        rotationVector = new Vector3(0, 0, 0);
        rotationVector.y += x;
    }

    private void ZoomCam()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            followOffset.y -= zoomPower;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            followOffset.y += zoomPower;
        }
    }
}
