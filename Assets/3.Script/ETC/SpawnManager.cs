using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance = null;
    public enum CharectarClass
    {
        Warrior,
        Achor,
        Rogue,
        Wizzard
    }

    [SerializeField] private List<CharectarClass> charectarClasseList;

    [SerializeField] private GameObject warrior;
    [SerializeField] private GameObject achor;
    [SerializeField] private GameObject wizzard;
    [SerializeField] private GameObject rogue;


    private void Awake()
    {
        #region [ΩÃ±€≈Ê]
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        #endregion

        for(int i = 0; i < 4; i++)
        {
            charectarClasseList.Add(CharectarClass.Warrior);
        }
    }


    public void AddUnitList(int index, CharectarClass value)
    {
        charectarClasseList[index] = value;
    }


    public void SpawnUnitFin()
    {
        for(int i = 0; i < charectarClasseList.Count; i++)
        {
            SpawnUnits(i);
        }
    }

    public void SpawnUnits(int index)
    {
        if(charectarClasseList[index] == CharectarClass.Warrior)
        {
            GameObject warriorClone = Instantiate(warrior, SpawnUnitIndex(index), Quaternion.identity);
        }
        else if(charectarClasseList[index] == CharectarClass.Achor)
        {
            GameObject achorClone = Instantiate(achor, SpawnUnitIndex(index), Quaternion.identity);
        }
        else if (charectarClasseList[index] == CharectarClass.Rogue)
        {
            GameObject rogueClone = Instantiate(rogue, SpawnUnitIndex(index), Quaternion.identity);
        }
        else if (charectarClasseList[index] == CharectarClass.Wizzard)
        {
            GameObject wizzardClone = Instantiate(wizzard, SpawnUnitIndex(index), Quaternion.identity);
        }
    }

    public Vector3 SpawnUnitIndex(int index)
    {
        if(index == 0)
        {
            return new Vector3(4, 0, 4);
        }
        else if (index == 1)
        {
            return new Vector3(4, 0, 2);
        }
        else if (index == 2)
        {
            return new Vector3(8, 0, 4);
        }
        else if (index == 3)
        {
            return new Vector3(10, 0, 2);
        }
        else
        {
            return Vector3.zero;
        }
    }
}
