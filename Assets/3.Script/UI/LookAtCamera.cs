using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LookAtCamera : MonoBehaviour
{
    #region
/*
    [SerializeField] private bool invert;
    private Transform cameraTransform;

    private void Awake()
    {
        cameraTransform = Camera.main.transform;
    }

    private void LateUpdate()
    {
        if (invert)
        {
            Vector3 direction = (cameraTransform.position - transform.position).normalized;
            transform.LookAt(transform.position + direction * -1);
        }
        else
        {
            transform.LookAt(cameraTransform);
        }
    }*/
    #endregion

        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset;
        [SerializeField] private LayerMask WallLayer;

        [SerializeField] private Image foreGround;
        [SerializeField] private Image backGround;


        private void LateUpdate()
        {
            BehindHpBar();

            transform.position = Camera.main.WorldToScreenPoint(target.position + offset);
        }

        private void BehindHpBar()
        {
            Vector3 direction = (target.position - Camera.main.transform.position).normalized;
            bool isBehind = Vector3.Dot(direction, Camera.main.transform.forward) <= 0.0f;

            foreGround.enabled = !isBehind;
            backGround.enabled = !isBehind;
        }

        private void BehindWall()
        {
            Vector3 direction = (target.position - Camera.main.transform.position).normalized;
            bool isBehind;

            if (Physics.Raycast(Camera.main.transform.position, direction, Vector3.Distance(target.position, Camera.main.transform.position), WallLayer))
            {
                isBehind = true;
            }
            else
            {
                isBehind = false;
            }

            foreGround.enabled = !isBehind;
            backGround.enabled = !isBehind;
        }

/*        public void SetHealthBar(float amount)
        {
            float parentWidth = GetComponent<RectTransform>().rect.width;
            float width = parentWidth * amount;
            foreGround.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        }*/
}
