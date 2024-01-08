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
        Book
    }

    [SerializeField] private State state;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Unit target = other.GetComponent<Unit>();
            GridPosition targetGridPosition = LevelGrid.Instance.GetGridPosition(gameObject.transform.position);
            if (target.isRogue && state == State.Dagger)
            {
                other.gameObject.GetComponent<ClassAction>().enabled = true;
                target.SetWeapon();
                UnitActionSystem.Instance.UndateSelectVisual();
                EffectSystem.Instance.ItemEffectPlay(LevelGrid.Instance.GetWorldPosition(targetGridPosition));
                Destroy(gameObject);
            }
            else if (target.isWarrior && state == State.Axe)
            {
                other.gameObject.GetComponent<ClassAction>().enabled = true;
                target.SetWeapon();
                UnitActionSystem.Instance.UndateSelectVisual();
                EffectSystem.Instance.ItemEffectPlay(LevelGrid.Instance.GetWorldPosition(targetGridPosition));
                Destroy(gameObject);
            }
            else if (target.isAchor && state == State.Bow)
            {
                other.gameObject.GetComponent<ClassAction>().enabled = true;
                target.SetWeapon();
                UnitActionSystem.Instance.UndateSelectVisual();
                EffectSystem.Instance.ItemEffectPlay(LevelGrid.Instance.GetWorldPosition(targetGridPosition));
                Destroy(gameObject);
            }
            else if (target.isWizard && state == State.Book)
            {
                other.gameObject.GetComponent<ClassAction>().enabled = true;
                target.SetWeapon();
                UnitActionSystem.Instance.UndateSelectVisual();
                EffectSystem.Instance.ItemEffectPlay(LevelGrid.Instance.GetWorldPosition(targetGridPosition));
                Destroy(gameObject);
            }

        }
    }
}
