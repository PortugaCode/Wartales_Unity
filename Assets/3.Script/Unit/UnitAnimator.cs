using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [Header("Animator")]
    public Animator animator;

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
            swordAction.OnBackAttack += SwordAction_OnBackAttack;
        }
        if (TryGetComponent<InteractAction>(out InteractAction interactAction))
        {
            interactAction.OnInteract += interactAction_OnInteract;
        }

        if (TryGetComponent<ClassAction>(out ClassAction classAction))
        {
            classAction.OnBerserk += ClassAction_OnBerserk;
            classAction.OnHealing += ClassAction_OnHealing;
            classAction.OnAssassination += ClassAction_OnAssassination;
        }
    }

    private void ClassAction_OnHealing(object sender, EventArgs e)
    {
        animator.SetTrigger("Healing");
    }

    private void ClassAction_OnAssassination(object sender, EventArgs e)
    {
        animator.SetTrigger("Assassination");
    }

    private void ClassAction_OnBerserk(object sender, EventArgs e)
    {
        animator.SetTrigger("Berserk");
    }

    private void interactAction_OnInteract(object sender, EventArgs e)
    {
        animator.SetTrigger("Interact");
    }

    private void SwordAction_OnBackAttack(object sender, EventArgs e)
    {
        animator.SetTrigger("BackAttack");
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
