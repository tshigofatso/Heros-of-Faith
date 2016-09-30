﻿using UnityEngine;
using System.Collections;
using System;

public class IdleState : IEnemyState
{

    private Enemy enemy;
    private float idleTimer;
    private float idleDuration;

    public void Enter(Enemy enemy)
    {
        idleDuration = UnityEngine.Random.Range(1,10);
        this.enemy = enemy;
    }

    public void Execute()
    {
       
        Idle();
        if (enemy.Target != null)
        {
            enemy.changeState(new PatrolSate());
        }
    }

    public void Exit()
    {
       
    }

    public void OnTriggerEnter(Collider2D other)
    {
        if (other.tag == "Knife")
        {
            enemy.Target = Player.Instance.gameObject;
        }
    }

    private void Idle()
    {
        enemy.MyAnimator.SetFloat("speed",0);
        idleTimer += Time.deltaTime;
        if (idleTimer >= idleDuration )
        {
            enemy.changeState(new PatrolSate());
        }
    }
}
