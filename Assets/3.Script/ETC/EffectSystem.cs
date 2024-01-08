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
    public ParticleSystem healEffect;
    public ParticleSystem smokeEffect;
    public ParticleSystem ItemEffect;


    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void HealingPlay(Vector3 position)
    {
        healEffect.transform.position = position;
        healEffect.Play();
    }

    public void ItemEffectPlay(Vector3 position)
    {
        ItemEffect.transform.position = position;
        ItemEffect.Play();
    }
    public void SmokePlay(Vector3 position)
    {
        smokeEffect.transform.position = position;
        smokeEffect.Play();
    }


    public void BombEffect(Vector3 position)
    {
        bombEffect.transform.position = position;
        bombEffect.Play();
    }
}
