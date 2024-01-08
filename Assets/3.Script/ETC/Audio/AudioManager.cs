using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip[] walkSoundClip;
    [SerializeField] private AudioClip[] walkSoundClip_;

    [SerializeField] private AudioClip[] axeSwingClip;

    [SerializeField] private AudioClip berserkClip;

    [SerializeField] private AudioClip[] damageClip;


    [SerializeField] private AudioClip[] voiceUnit;

    [SerializeField] private AudioClip[] bodyFalls;

    [SerializeField] private AudioClip[] breakingCrate;

    [SerializeField] private AudioClip openDoorClip;

    [SerializeField] private AudioClip[] itemSound;

    [SerializeField] private AudioClip[] assasinAttackClip;

    [SerializeField] private AudioClip smokeClip;


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

    public void SmokeSoundPlay()
    {
        audioSource.PlayOneShot(smokeClip);
    }

    public void ItemSoundPlay(bool a)
    {
        if (a)
        {
            audioSource.PlayOneShot(itemSound[0]);
        }
        else
        {
            audioSource.PlayOneShot(itemSound[1]);
        }
    }

    public void OpenDoorSoundPlay()
    {
        audioSource.PlayOneShot(openDoorClip);
    }

    public void BreakingCrateSoundPlay()
    {
        audioSource.PlayOneShot(breakingCrate[Random.Range(0, breakingCrate.Length)]);
    }

    public void AssasinAttackSoundPlay()
    {
        audioSource.PlayOneShot(assasinAttackClip[Random.Range(0, assasinAttackClip.Length)]);
    }

    public void BodyFallsSoundPlay()
    {
        audioSource.PlayOneShot(bodyFalls[Random.Range(0, bodyFalls.Length)]);
    }

    public void TakeDamageSoundPlay()
    {
        audioSource.PlayOneShot(damageClip[Random.Range(0, damageClip.Length)]);
    }

    public void FootStepSoundPlay_Warrior()
    {
        audioSource.PlayOneShot(walkSoundClip[Random.Range(0, walkSoundClip.Length)]);
    }
    public void FootStepSoundPlay()
    {
        audioSource.PlayOneShot(walkSoundClip_[Random.Range(0, walkSoundClip_.Length)]);
    }

    public void AxeSwiongSoundPlay()
    {
        audioSource.PlayOneShot(axeSwingClip[Random.Range(0, axeSwingClip.Length)]);
    }

    public void VoiceMalePlay()
    {
        audioSource.PlayOneShot(voiceUnit[0]);
    }

    public void VoiceFemalePlay()
    {
        audioSource.PlayOneShot(voiceUnit[1]);
    }

    public void BerserkSoundPlay()
    {
        audioSource.PlayOneShot(berserkClip);
    }
}
