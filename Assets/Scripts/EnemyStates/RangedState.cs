using UnityEngine;
using System.Collections;
using System;

public class Ranged : IEnemyState
{
    private float throwTimer;
    private float throwCoolDown = 3.0f;
    private bool CanTrow = true;

    private Enemy enemy;
    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Execute()
    {
        throwKnife();
        
        if (enemy.InMeleeRange)
        {
            enemy.changeState(new MeleeState());
        }else if (enemy.Target != null)
        {
            
            enemy.Move();
        }
        else
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

    private void throwKnife()
    {
        throwTimer += Time.deltaTime;

        if (throwTimer >= throwCoolDown)
        {
            CanTrow = true;
            throwTimer = 0;
        }
        if (CanTrow)
        {
            CanTrow = false;
            enemy.MyAnimator.SetTrigger("throw");
        }
    }
}
