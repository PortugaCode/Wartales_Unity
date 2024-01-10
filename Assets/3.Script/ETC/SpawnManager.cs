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
    }
}
