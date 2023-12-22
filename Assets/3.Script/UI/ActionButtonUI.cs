using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private Button button;
    [SerializeField] private Image img;
    [SerializeField] private GameObject selectGameObject;

    private BaseAction baseAction;

    public void SetBaseAction(BaseAction baseAction)
    {
        this.baseAction = baseAction;
        textMeshPro.text = baseAction.GetActionName().ToUpper();
        img.sprite = baseAction.GetActionImage();

        button.onClick.AddListener(() => {

            UnitActionSystem.Instance.SetSelectAction(baseAction);
        
        });
    }

    public void UpdateSelectVisual()
    {
        BaseAction selectBaseAction = UnitActionSystem.Instance.GetSelectAction();
        selectGameObject.SetActive(selectBaseAction == baseAction);
    }


}
