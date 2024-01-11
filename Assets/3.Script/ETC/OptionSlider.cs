using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionSlider : MonoBehaviour
{
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        bgmSlider.value = AudioManager.Instance.GetBGM().volume;
        sfxSlider.value = AudioManager.Instance.GetSFX().volume;
    }
    private void Update()
    {
        AudioManager.Instance.GetBGM().volume = bgmSlider.value;
        AudioManager.Instance.GetSFX().volume = sfxSlider.value;
    }
}
