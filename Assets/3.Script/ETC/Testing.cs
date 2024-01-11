using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Testing : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.Instance.UIMouseClcikSoundPlay();
        Debug.Log("Click");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Instance.UIMouseOnSoundPlay();
        Debug.Log("Enter");
    }
}


