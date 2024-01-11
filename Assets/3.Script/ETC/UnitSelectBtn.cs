using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectBtn : MonoBehaviour
{
    //0 ������, 1 ��ó, 2 ���ڵ�, 3 �α�
    [SerializeField] private List<GameObject> selectUnit;
    [SerializeField] private int nowIndex = 0;


    public void RightBtnClick(int index)
    {
        if(nowIndex == 0)
        {
            selectUnit[nowIndex].SetActive(false);
            nowIndex += 1;
            selectUnit[nowIndex].SetActive(true);
            SpawnManager.Instance.AddUnitList(index, SpawnManager.CharectarClass.Achor);
        }
        else if(nowIndex == 1)
        {
            selectUnit[nowIndex].SetActive(false);
            nowIndex += 1;
            selectUnit[nowIndex].SetActive(true);
            SpawnManager.Instance.AddUnitList(index, SpawnManager.CharectarClass.Wizzard);
        }
        else if (nowIndex == 2)
        {
            selectUnit[nowIndex].SetActive(false);
            nowIndex += 1;
            selectUnit[nowIndex].SetActive(true);
            SpawnManager.Instance.AddUnitList(index, SpawnManager.CharectarClass.Rogue);
        }
        else if (nowIndex == 3)
        {
            selectUnit[nowIndex].SetActive(false);
            nowIndex = 0;
            selectUnit[nowIndex].SetActive(true);
            SpawnManager.Instance.AddUnitList(index, SpawnManager.CharectarClass.Warrior);
        }
    }

    public void LeftBtnClick(int index)
    {
        if (nowIndex == 0)
        {
            selectUnit[nowIndex].SetActive(false);
            nowIndex = 3;
            selectUnit[nowIndex].SetActive(true);
            SpawnManager.Instance.AddUnitList(index, SpawnManager.CharectarClass.Rogue);
        }
        else if (nowIndex == 3)
        {
            selectUnit[nowIndex].SetActive(false);
            nowIndex -= 1;
            selectUnit[nowIndex].SetActive(true);
            SpawnManager.Instance.AddUnitList(index, SpawnManager.CharectarClass.Wizzard);
        }
        else if (nowIndex == 2)
        {
            selectUnit[nowIndex].SetActive(false);
            nowIndex -= 1;
            selectUnit[nowIndex].SetActive(true);
            SpawnManager.Instance.AddUnitList(index, SpawnManager.CharectarClass.Achor);
        }
        else if (nowIndex == 1)
        {
            selectUnit[nowIndex].SetActive(false);
            nowIndex -= 1;
            selectUnit[nowIndex].SetActive(true);
            SpawnManager.Instance.AddUnitList(index, SpawnManager.CharectarClass.Warrior);
        }
    }
}
