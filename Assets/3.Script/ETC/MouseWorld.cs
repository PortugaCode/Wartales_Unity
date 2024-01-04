using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    private Ray ray;
    private RaycastHit hit;

    [SerializeField] private LayerMask plannLayer;

    [SerializeField] private Texture2D normal;
    [SerializeField] private Texture2D select;
    [SerializeField] private Texture2D attack;

    public static MouseWorld Instance;

    private void Awake()
    {
        #region [½Ì±ÛÅæ]
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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
        GetCursor();
    }

    public Vector3 GetPoint() //¸¶¿ì½º ½ºÅ©¸° Æ÷ÀÎÆ® Âï±â
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit, float.MaxValue, plannLayer);
        return hit.point;
    }

    private void GetCursor()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit, float.MaxValue);

        if (hit.collider == null) return;

        if(hit.collider.CompareTag("Player"))
        {
            Cursor.SetCursor(select, new Vector2(3, 3), CursorMode.Auto);
        }
        else if(hit.collider.CompareTag("Enemy"))
        {
            Cursor.SetCursor(attack, new Vector2(3, 3), CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(normal, new Vector2(2, 2), CursorMode.Auto);
        }
    }
}
