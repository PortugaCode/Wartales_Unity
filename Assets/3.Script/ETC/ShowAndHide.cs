using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ShowAndHide : MonoBehaviour
{
    [SerializeField] private MeshRenderer[] meshRendererArray;
    [SerializeField] private MeshRenderer[] meshRendererOpa;
    [SerializeField] private GameObject[] enemySpawn;

    float time = 0f;
    float F_time = 1f;

    public void ShowStage(int index)
    {
        meshRendererArray[index].enabled = false;
        Fade_Out(index);
        enemySpawn[index].SetActive(true);
    }

    public void HideStage(int index)
    {
        Fade_in(index);
    }


    private void Fade_Out(int index)
    {
        StartCoroutine(FadeOut(index));
    }

    private void Fade_in(int index)
    {
        StartCoroutine(Fadein(index));
    }

    private IEnumerator FadeOut(int index)
    {
        Color alpha = meshRendererOpa[index].material.color;
        time = 0f;
        while(alpha.a > 0f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(1, 0, time);
            meshRendererOpa[index].material.color = alpha;
            yield return null;
        }

        yield return null;
    }

    private IEnumerator Fadein(int index)
    {
        Color alpha = meshRendererOpa[index].material.color;
        time = 0f;
        while (alpha.a < 1f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(0, 1, time);
            meshRendererOpa[index].material.color = alpha;
            yield return null;
        }
        meshRendererArray[index].enabled = true;
        yield return null;
    }
}
