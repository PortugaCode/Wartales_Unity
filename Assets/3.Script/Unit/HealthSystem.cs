using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDead;

    [SerializeField] private int health;
    private bool isDie = false;
    public bool IsDie => isDie;

    [Header("MaxHP")]
    [SerializeField] private int Maxhealth = 100;

    public bool isAssasin;
    private void Awake()
    {
        health = Maxhealth;
    }

    public int Gethealth()
    {
        return health;
    }

    public void Sethealth(int a)
    {
        health += a;

        if(health >= Maxhealth)
        {
            health = Maxhealth;
        }


        if (health <= 0)
        {
            TryGetComponent<SwordAction>(out SwordAction swordAction);
            if(swordAction != null)
            {
                if(swordAction.IsBerserk)
                {
                    health = 1;
                }
                else
                {
                    health = 0;
                }
            }
            else
            {
                health = 0;
            }
        }


        if (health == 0 && !isDie)
        {
            isDie = true;
            Die();
        }

    }

    public void Damage(int amount)
    {
        health -= amount;
        if(health <= 0)
        {
            health = 0;
        }

        if(health == 0 && !isDie)
        {
            isDie = true;
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
