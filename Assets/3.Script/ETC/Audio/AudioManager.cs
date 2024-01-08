using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip[] walkSoundClip;
    [SerializeField] private AudioClip[] walkSoundClip_;

    [SerializeField] private AudioClip berserkSound;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void FootStepSoundPlay_Warrior()
    {
        audioSource.PlayOneShot(walkSoundClip[Random.Range(0, walkSoundClip.Length)]);
    }
    public void FootStepSoundPlay()
    {
        audioSource.PlayOneShot(walkSoundClip_[Random.Range(0, walkSoundClip_.Length)]);
    }

    public void BerserkSoundPlay()
    {
        audioSource.PlayOneShot(berserkSound);
    }
}
