using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    public Unit target;
    public int damage;

    private void Start()
    {
        transform.transform.LookAt(target.GetWorldPosition() + new Vector3(0f, 1.2f, 0f));
        rb.AddForce(transform.forward * 1000f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Enemy"))
        {
            collision.transform.GetComponent<Unit>().Damage(damage);
            EffectSystem.Instance.hitEffect.transform.position = transform.position;
            EffectSystem.Instance.hitEffect.Play();
            Destroy(gameObject);
        }
        else if(collision.collider.CompareTag("Player"))
        {
            collision.transform.GetComponent<Unit>().Damage(damage);
            EffectSystem.Instance.hitEffect.transform.position = transform.position;
            EffectSystem.Instance.hitEffect.Play();
            Destroy(gameObject);
        }
    }
}
