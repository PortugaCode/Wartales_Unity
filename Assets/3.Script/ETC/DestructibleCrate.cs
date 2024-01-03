using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleCrate : MonoBehaviour
{

    [SerializeField] private Transform destroyCratePrefab;

    public void Damage()
    {
        Transform a = Instantiate(destroyCratePrefab, transform.position, transform.rotation);

        ApplyExplosionToChildren(a, 150f, transform.position, 10f);

        Destroy(a.gameObject, 6f);
        Destroy(gameObject);
    }

    private void ApplyExplosionToChildren(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange)
    {
        foreach(Transform child in root)
        {
            if(child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
            {
                childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
            }

            ApplyExplosionToChildren(child, explosionForce, explosionPosition, explosionRange);
        }
    }
}
