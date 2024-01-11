using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;


    [SerializeField] private GameObject winUI;
    [SerializeField] private GameObject loseUI;
    [SerializeField] private GameObject pauseUI;

    [HideInInspector]
    public bool isPause = false;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !isPause)
        {
            Time.timeScale = 0;
            isPause = true;
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && isPause)
        {
            Time.timeScale = 1;
            isPause = false;
        }

        pauseUI.SetActive(isPause);
    }


    public void Win()
    {
        StartCoroutine(Win_Co());
    }

    public void Lose()
    {
        StartCoroutine(Lose_Co());
    }

    private IEnumerator Win_Co()
    {
        yield return new WaitForSeconds(1f);
        winUI.SetActive(true);
    }

    private IEnumerator Lose_Co()
    {
        yield return new WaitForSeconds(1f);
        loseUI.SetActive(true);
    }
}
