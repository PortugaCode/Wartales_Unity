using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private GameObject MainIntro;
    [SerializeField] private GameObject Option;
    [SerializeField] private GameObject SelectUnit;

    public void GoToOption()
    {
        Option.SetActive(true);
        MainIntro.SetActive(false);
        SelectUnit.SetActive(false);
    }

    public void GoToSelectUnit()
    {
        Option.SetActive(false);
        MainIntro.SetActive(false);
        SelectUnit.SetActive(true);
    }

    public void GoToMainIntro()
    {
        Option.SetActive(false);
        MainIntro.SetActive(true);
        SelectUnit.SetActive(false);
    }

    public void LoadStingScene(string n)
    {
        SceneManager.LoadScene(n);
    }

    public void GameExit()
    {
        Application.Quit();
    }
}
