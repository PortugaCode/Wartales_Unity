using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    private Ray ray;
    private RaycastHit hit;

    [SerializeField] private LayerMask plannLayer;

    public static MouseWorld Instance;
    private void Awake()
    {
        #region [ΩÃ±€≈Ê]
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        #endregion
    }

    private void Update()
    {
        transform.position = GetPoint();
    }

    public Vector3 GetPoint()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit, float.MaxValue, plannLayer);
        return hit.point;
    }
}
