using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDead;

    private int health;
    [Header("MaxHP")]
    [SerializeField] private int Maxhealth = 100;

    private void Awake()
    {
        health = Maxhealth;
    }

    public int Gethealth()
    {
        return health;
    }

    public void Damage(int amount)
    {
        health -= amount;
        if(health <= 0)
        {
            health = 0;
        }

        if(health == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDead?.Invoke(this, EventArgs.Empty);
    }

    public float GetHealthNormalized()
    {
        return (float) health / Maxhealth;
    }
}
