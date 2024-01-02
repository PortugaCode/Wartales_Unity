using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSystem : MonoBehaviour
{
    public static EffectSystem Instance { get; private set; }

    public ParticleSystem hitEffect;
    public ParticleSystem bombEffect;
    public ParticleSystem fireball;


    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }


    public void BombEffect(Vector3 position)
    {
        bombEffect.transform.position = position;
        bombEffect.Play();
    }
}
