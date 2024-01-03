using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] private Animator animator;

    private void Awake()
    {
        if (TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            moveAction.OnStartMoving += MoveAction_OnStartMoving;
            moveAction.OnStopMoving += MoveAction_OnStopMoving;
        }
        if (TryGetComponent<ShootAction>(out ShootAction shootAction))
        {
            shootAction.OnShooting += ShootAction_OnShooting;
            shootAction.OnAiming += ShootAction_OnAiming;
        }
        if(TryGetComponent<Unit>(out Unit unit))
        {
            unit.OnDamage += Unit_OnDamage;
            unit.OnDie += Unit_OnDie;
        }
        if(TryGetComponent<FireBallAction>(out FireBallAction fireBallAction))
        {
            fireBallAction.OnShootingFireBall += FireBallAction_OnShootingFireBall;
        }
        if (TryGetComponent<SwordAction>(out SwordAction swordAction))
        {
            swordAction.OnAttack += SwordAction_OnAttack;
        }
    }

    private void SwordAction_OnAttack(object sender, EventArgs e)
    {
        animator.SetTrigger("Attack");
    }

    private void FireBallAction_OnShootingFireBall(object sender, EventArgs e)
    {
        animator.SetTrigger("FireBall");
    }

    private void Unit_OnDie(object sender, EventArgs e)
    {
        ScreenShake.Instance.Shake();
        animator.SetTrigger("Die");
    }

    private void Unit_OnDamage(object sender, EventArgs e)
    {
        animator.SetTrigger("Damage");
    }

    private void ShootAction_OnShooting(object sender, EventArgs e)
    {
        animator.SetTrigger("Shooting");
    }

    private void ShootAction_OnAiming(object sender, EventArgs e)
    {
        animator.SetTrigger("Aiming");
    }

    private void MoveAction_OnStartMoving(object sender, EventArgs e)
    {
        animator.SetBool("isWalking", true);
    }

    private void MoveAction_OnStopMoving(object sender, EventArgs e)
    {
        animator.SetBool("isWalking", false);
    }
}
