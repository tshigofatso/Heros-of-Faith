using UnityEngine;
using System.Collections;
using System;

public class Enemy : Charector {

    private IEnemyState currentState;
    public GameObject Target { get; set; }
    [SerializeField]
    private float meleeRange = 3.0f;
    [SerializeField]
    private float throwRange = 3.0f;

    private Vector3 startPos;
    [SerializeField]
    private Transform LeftEdge;
    [SerializeField]
    private Transform rightEdge;

    public bool InThrowRange {
        get
        {
            if (Target != null)
            {
                return Vector2.Distance(transform.position, Target.transform.position) <= throwRange;
            }
            return false;
        }
    }


    public bool InMeleeRange
    {
        get
        {
            if (Target != null)
            {
                return Vector2.Distance(transform.position, Target.transform.position) <= meleeRange;
            }
            return false;
        }
    }




    // Use this for initialization
    public override void Start ()
    {
        base.Start();
        Player.Instance.Dead += new DeadEventHandler(RemoveTarget);
        changeState(new IdleState());
        startPos = transform.position;

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!IsDead)
        {
            if (!TakingDamage)
            {
                currentState.Execute();
            }
            

            LootAtTarget();
        }
        
	}

    public void changeState(IEnemyState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;

        currentState.Enter(this);
    }

    public void Move()
    {
        if (!Attack)
        {
            
            if ((GetDirection().x > 0 && transform.position.x < rightEdge.position.x)  || (GetDirection().x < 0 && transform.position.x > LeftEdge.position.x))
            {
                MyAnimator.SetFloat("speed", 1);

                transform.Translate(GetDirection() * (MovementSpeed * Time.deltaTime));
            }
            else if (currentState is PatrolSate)
            {
                ChangeDirection();
            }

            
        }
        

    }

    public Vector2 GetDirection()
    {
        return facingRight ? Vector2.right : Vector2.left;
    }

    public override void OnTriggerEnter2D(Collider2D other) {
        base.OnTriggerEnter2D(other);
        currentState.OnTriggerEnter(other);
    }

    private void LootAtTarget() {
        if (Target != null)
        {
            float xDir = Target.transform.position.x - transform.position.x;
            if (xDir < 0 && facingRight || xDir > 0 && !facingRight)
            {
                ChangeDirection();
            }
        }
    }
    public override bool IsDead
    {

        
        get
        {
            return health <= 0;
        }
    }
    public override IEnumerator TakeDamage()
    {
        
        health -= 10;
        if (!IsDead)
        {
            MyAnimator.SetTrigger("damage");
        }
        else
        {
            MyAnimator.SetTrigger("die");
            yield return null;
        }
    }

    public void RemoveTarget()
    {
        Target = null;
        changeState(new PatrolSate());   
     }

    public override void Death()
    {
        MyAnimator.ResetTrigger("die");
        MyAnimator.SetTrigger("idle");
        health = 30;
        transform.position = startPos;
        
            
            //Destroy(gameObject); dont not respawn the enemy
    }
}
