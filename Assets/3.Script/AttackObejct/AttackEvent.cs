using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEvent : MonoBehaviour
{
    [SerializeField] private Unit masterUnit;
    public int damage;

    private void Start()
    {
        damage = masterUnit.GetAction<SwordAction>().intdamage;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(!masterUnit.IsEnemy())
        {
            if (collision.CompareTag("Enemy"))
            {
                collision.transform.GetComponent<Unit>().Damage(damage);
                EffectSystem.Instance.hitEffect.transform.position = transform.position;
                EffectSystem.Instance.hitEffect.Play();
            }
        }
        else
        {
            if (collision.CompareTag("Player"))
            {
                collision.transform.GetComponent<Unit>().Damage(damage);
                EffectSystem.Instance.hitEffect.transform.position = transform.position;
                EffectSystem.Instance.hitEffect.Play();
            }
        }

    }

}
