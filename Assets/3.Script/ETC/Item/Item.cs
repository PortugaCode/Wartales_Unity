using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private enum State
    {
        Axe,
        Bow,
        Dagger,
        Book,
        Potion
    }

    [SerializeField] private State state;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Unit target = other.GetComponent<Unit>();
            GridPosition targetGridPosition = LevelGrid.Instance.GetGridPosition(gameObject.transform.position);
            if (target.isRogue && state == State.Dagger && !target.IsEnemy())
            {
                other.gameObject.GetComponent<ClassAction>().enabled = true;
                target.SetWeapon();
                target.GetAction<SwordAction>().SetDamage(10);
                UnitActionSystem.Instance.UndateSelectVisual();
                EffectSystem.Instance.ItemEffectPlay(target.gameObject);
                Destroy(gameObject);
            }
            else if (target.isWarrior && state == State.Axe && !target.IsEnemy())
            {
                other.gameObject.GetComponent<ClassAction>().enabled = true;
                target.SetWeapon();
                target.GetAction<SwordAction>().SetDamage(15);
                UnitActionSystem.Instance.UndateSelectVisual();
                EffectSystem.Instance.ItemEffectPlay(target.gameObject);
                Destroy(gameObject);
            }
            else if (target.isAchor && state == State.Bow && !target.IsEnemy())
            {
                other.gameObject.GetComponent<ClassAction>().enabled = true;
                target.SetWeapon();
                target.GetAction<ShootAction>().SetDamage(10);
                UnitActionSystem.Instance.UndateSelectVisual();
                EffectSystem.Instance.ItemEffectPlay(target.gameObject);
                Destroy(gameObject);
            }
            else if (target.isWizard && state == State.Book && !target.IsEnemy())
            {
                other.gameObject.GetComponent<ClassAction>().enabled = true;
                target.SetWeapon();
                target.GetAction<FireBallAction>().SetDamage2(20);
                UnitActionSystem.Instance.UndateSelectVisual();
                EffectSystem.Instance.ItemEffectPlay(target.gameObject);
                Destroy(gameObject);
            }
            else if(state == State.Potion)
            {
                target.SetHealth(50);
                UnitActionSystem.Instance.UndateSelectVisual();
                EffectSystem.Instance.ItemEffectPlay(target.gameObject);
                Destroy(gameObject);
            }
        }
    }
}
