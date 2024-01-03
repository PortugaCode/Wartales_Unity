using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public static event EventHandler OnAnyFireBallExploded;

    private Vector3 targetPosition;
    private Action onFireBallBehaviourComplete;

    [SerializeField] private AnimationCurve arcYAnimationCurve;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float damageRadius = 4f;
    [SerializeField] private int damage = 50;
    private float reachedTargetDistance = 0.1f;
    private float totalDistance;
    private Vector3 positionXZ;


    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

    private void Start()
    {
        EffectSystem.Instance.fireball.transform.position = transform.position;
        EffectSystem.Instance.fireball.Play();
    }

    private void FixedUpdate()
    {
        EffectSystem.Instance.fireball.transform.position = transform.position;
    }


    private void Update()
    {
        Vector3 moveDir = (targetPosition - positionXZ).normalized;
        positionXZ += moveDir * moveSpeed * Time.deltaTime;

        float distance = Vector3.Distance(positionXZ, targetPosition);
        float distanceNormalized = 1 - distance / totalDistance;

        float maxHeight = totalDistance / 4f;
        float positionY = arcYAnimationCurve.Evaluate(distanceNormalized) * maxHeight;
        transform.position = new Vector3(positionXZ.x, positionY, positionXZ.z);

        if (Vector3.Distance(positionXZ, targetPosition) <= reachedTargetDistance)
        {

            Collider[] colliderArray = Physics.OverlapSphere(targetPosition, damageRadius);
            foreach(Collider col in colliderArray)
            {
                if(col.TryGetComponent<Unit>(out Unit target))
                {
                    target.Damage(damage);
                }
                if(col.TryGetComponent<DestructibleCrate>(out DestructibleCrate crate))
                {
                    crate.Damage();
                }
            }
            OnAnyFireBallExploded?.Invoke(this, EventArgs.Empty);
            EffectSystem.Instance.BombEffect(transform.position);
            EffectSystem.Instance.fireball.Stop();
            Destroy(gameObject);
            onFireBallBehaviourComplete();
        }
    }


    public void SetUp(GridPosition targetGridPosition, Action onFireBallBehaviourComplete)
    {
        this.onFireBallBehaviourComplete = onFireBallBehaviourComplete;
        targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);

        positionXZ = transform.position;
        positionXZ.y = 0;
        totalDistance = Vector3.Distance(positionXZ, targetPosition);
    }
}
