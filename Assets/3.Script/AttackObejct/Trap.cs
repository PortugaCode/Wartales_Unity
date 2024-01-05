using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{

    private GridPosition gridPosition;


    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetTrapAtGridPosition(gridPosition, this);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.transform.GetComponent<Unit>().SetHealth(-50);

            EffectSystem.Instance.hitEffect.transform.position = transform.position + Vector3.up * 0.5f;
            EffectSystem.Instance.hitEffect.Play();
            Destroy(gameObject);

        }
        else if (collision.CompareTag("Player"))
        {
            collision.transform.GetComponent<Unit>().SetHealth(-50);

            EffectSystem.Instance.hitEffect.transform.position = transform.position + Vector3.up * 0.5f;
            EffectSystem.Instance.hitEffect.Play();
            Destroy(gameObject);

        }

    }
}
