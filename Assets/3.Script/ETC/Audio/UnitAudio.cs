using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAudio : MonoBehaviour
{
    public void FootStep_Warrior()
    {
        AudioManager.Instance.FootStepSoundPlay_Warrior();
    }

    public void FootStep()
    {
        AudioManager.Instance.FootStepSoundPlay();
    }

    public void BerserkSound()
    {
        AudioManager.Instance.BerserkSoundPlay();
    }

    public void AxeSwingSound()
    {
        AudioManager.Instance.AxeSwiongSoundPlay();
    }

    public void VoiceMale()
    {
        if(gameObject.transform.parent.GetComponent<Unit>().isAchor && !gameObject.transform.parent.GetComponent<Unit>().IsEnemy())
        {
            AudioManager.Instance.VoiceFemalePlay();
        }
        else if(!gameObject.transform.parent.GetComponent<Unit>().IsEnemy())
        {
            AudioManager.Instance.VoiceMalePlay();
        }
    }

    public void BodyFallsSound()
    {
        AudioManager.Instance.BodyFallsSoundPlay();
    }

    public void DaggerAttackSound()
    {
        AudioManager.Instance.AssasinAttackSoundPlay();
    }

    public void BowShootSound()
    {
        AudioManager.Instance.BowShootSoundPlay();
    }

    public void BowDrawSound()
    {
        AudioManager.Instance.BowDrawSoundPlay();
    }

    public void BowHandleSound()
    {
        AudioManager.Instance.BowHandleSoundPlay();
    }

    public void SetTrapSound()
    {
        AudioManager.Instance.TrapSoundPlay();
    }

    public void FireShotSound()
    {
        AudioManager.Instance.FireShotSoundPlay();
    }

    public void HealSound()
    {
        AudioManager.Instance.HealSoundPlay();
    }
}
