using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ScreenShake : MonoBehaviour
{
    public static ScreenShake Instance { get; private set; }

    private CinemachineImpulseSource cinemachineImpulseSource;

    private void Awake()
    {
        #region [ΩÃ±€≈Ê]
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        #endregion

        TryGetComponent(out cinemachineImpulseSource);
    }

    public void Shake(float intensity = 1f)
    {
        cinemachineImpulseSource.GenerateImpulse(intensity);
    }
}
