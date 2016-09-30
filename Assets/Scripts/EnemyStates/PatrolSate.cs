using UnityEngine;
using System.Collections;
using System;

public class PatrolSate : IEnemyState
{

    private Enemy enemy;
    private float patrolTimer;
    private float patrolDuration;


    public void Enter(Enemy enemy)
    {
        patrolDuration = UnityEngine.Random.Range(1,10);
        this.enemy = enemy;
    }

    public void Execute()
    {
        
        Patrol();

        enemy.Move();
        if (enemy.Target != null && enemy.InThrowRange)
        {
            enemy.changeState(new Ranged());
        }
        
    }

    public void Exit()
    {
        
    }

    public void OnTriggerEnter(Collider2D other)
    {
        
        //aquire the player as a target
        if (other.tag == "Knife")
        {
            enemy.Target = Player.Instance.gameObject;
        }
    }


    private void Patrol()
    {

        patrolTimer += Time.deltaTime;

        if (patrolTimer >= patrolDuration)
        {
            enemy.changeState(new IdleState());
        }
    }
}
