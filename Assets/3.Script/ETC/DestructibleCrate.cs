using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleCrate : MonoBehaviour
{

    [SerializeField] private Transform destroyCratePrefab;

    [SerializeField] private Transform[] item;

    private Action onInteractComplete;
    private float timer;
    private bool isActive;

    private GridPosition gridPosition;

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetCrateAtGridPosition(gridPosition, this);
    }

    private void Update()
    {
        if (!isActive) return;
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            isActive = false;
            
            onInteractComplete();
            
        }
    }


    public void Interact(Action onInteractComplete)
    {
        this.onInteractComplete = onInteractComplete;
        isActive = true;
        timer = 0.79f;

        Damage();
    }

    public void Damage()
    {
        int r = UnityEngine.Random.Range(0, 10);
        gameObject.layer = 0;
        Pathfinding.Instance.SetISWalkableGridPosition_Crate();
        Transform a = Instantiate(destroyCratePrefab, transform.position, transform.rotation);


        AudioManager.Instance.BreakingCrateSoundPlay();

        ApplyExplosionToChildren(a, 150f, transform.position, 10f);

        if (r < 7)
        {
            Transform c = Instantiate(item[UnityEngine.Random.Range(0, item.Length)], transform.position, transform.rotation);
        }

        Destroy(a.gameObject, 6f);
        gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
        
        Destroy(gameObject, 0.8f);
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
