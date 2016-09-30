using UnityEngine;
using System.Collections;
using System;

public class MeleeState : IEnemyState
{

    private float attackTimer;
    private float attackCollDown = 3.0f;
    private bool CanAttack = true;

    private Enemy enemy;

    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
        
    }

    public void Execute()
    {
        Attack();
        if (enemy.InThrowRange && !enemy.InMeleeRange)
        {
            enemy.changeState(new Ranged());
        }
        else if (enemy.Target == null )
        {
            enemy.changeState(new IdleState());
        }
    }

    public void Exit()
    {
       
    }

    public void OnTriggerEnter(Collider2D collider)
    {
       
    }

    private void Attack()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackCollDown)
        {
            CanAttack = true;
            attackTimer = 0;
        }
        if (CanAttack)
        {
            CanAttack = false;
            enemy.MyAnimator.SetTrigger("attack");
        }
    }

}
