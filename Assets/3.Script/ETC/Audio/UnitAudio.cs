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
        AudioManager.Instance.VoiceMalePlay();
    }

    public void VoiceFeMale()
    {
        AudioManager.Instance.VoiceFemalePlay();
    }

    public void BodyFallsSound()
    {
        AudioManager.Instance.BodyFallsSoundPlay();
    }

    public void DaggerAttackSound()
    {
        AudioManager.Instance.AssasinAttackSoundPlay();
    }
}
