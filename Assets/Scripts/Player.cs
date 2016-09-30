using UnityEngine;
using System.Collections;
using System;


public delegate void DeadEventHandler();

public class Player : Charector {


    private static Player instance;

    public static Player Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<Player>();
            }
            return instance;
        }

        
    }

    public event DeadEventHandler Dead;

    //This script will animate the player pre

    private float direction;
    private bool move;
    private float btnhorizontal;
    //private
    private bool immortal = false;
    [SerializeField]
    private float immotalTime;
    private SpriteRenderer spriterender;
    [SerializeField]
    private Transform[] groundpoints;
    [SerializeField]
    private float groundRadious;
    [SerializeField]
    private LayerMask whatisground;
 
    [SerializeField]
    public float jumpForce;
    [SerializeField]
    public bool aircontrol;
    private Vector3 startPos;

    //public

    public Rigidbody2D MyRigidBody { get; set; }
    
    public bool Slide { get; set; }
    public bool Jump { get; set; }
    public bool OnGround { get; set; }

    public override bool IsDead
    {
        get
        {
            if (health <= 0)
            {
                OnDead();
            }
            return health <= 0;
        }
    }




    // Use this for initialization
    public override void Start () {

        base.Start();
        MyRigidBody = GetComponent<Rigidbody2D>();
        spriterender = GetComponent<SpriteRenderer>();
        startPos = transform.position;
    }

    void Update()
    {
        if (!TakingDamage && !IsDead)
        {
            //jumping off the cliff
            if (transform.position.y <= -14f)
            {
                Death();
            }
        }
        HandleInput();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!TakingDamage && !IsDead)
        {
            //getting the input values from unity on each and ever update
            float horizontal = Input.GetAxis("Horizontal");


            OnGround = IsGrounded();
            if (move)
            {
                ///handle movement with scrren button
                this.btnhorizontal = Mathf.Lerp(btnhorizontal, direction,Time.deltaTime * 2);
                HandleMovement(btnhorizontal);
                Flip(direction);
            }
            else
            {
                //calling the movement of the player
                HandleMovement(horizontal);

                Flip(horizontal);
            }
            

            HandleLayers();
        }

    }

    //moving the payer arounf
    private void HandleMovement(float horizontal) {
        if (MyRigidBody.velocity.y < 0)
        {
            MyAnimator.SetBool("land",true);
        }

        if (!Attack && !Slide && (OnGround || aircontrol))
        {
            MyRigidBody.velocity = new Vector2(horizontal * MovementSpeed, MyRigidBody.velocity.y);
        }
        if (Jump && MyRigidBody.velocity.y == 0)
        {
            MyRigidBody.AddForce(new Vector2(0,jumpForce));
        }

        MyAnimator.SetFloat("speed",Mathf.Abs(horizontal));

    }

    //changing the direction of the player
    private void Flip(float horizontal)
    {
        //to change the facing direction change the x value to -1
        if ((horizontal > 0 && !facingRight) || (horizontal < 0 && facingRight))
        {
            ChangeDirection();

        }
    }

    

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MyAnimator.SetTrigger("jump");
        }
        //attack jkey
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            MyAnimator.SetTrigger("attack");
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            MyAnimator.SetTrigger("slide");
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            MyAnimator.SetTrigger("throw");
            
        }
    }

 

    private bool IsGrounded()
    {
        if(MyRigidBody.velocity.y <= 0)
        {
            foreach (Transform point in groundpoints) {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position,groundRadious,whatisground);

                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject)
                    {
                        MyAnimator.SetBool("land",false);
                        return true;
                    }
                }
            }

        }
        return false;

        
    }


    /// <summary>
    /// hardcoded values here please try and set a constant
    /// </summary>
    private void HandleLayers()
    {
        if (!OnGround)
        {
            MyAnimator.SetLayerWeight(1, 1);
        }
        else
        {
            MyAnimator.SetLayerWeight(1, 0);
        }
    }

    public override void ThrowKnife(int value)
    {
        if (!OnGround && value == 1 || OnGround && value == 0)
        {
            base.ThrowKnife(value);
        }
        
    }

    public override IEnumerator TakeDamage()
    {
        if (!immortal)
        {
            health -= 10;
            if (!IsDead)
            {
                MyAnimator.SetTrigger("damage");
                //inidate if the player is immotal
                immortal = true;
                StartCoroutine(IndicateImmotal());
                yield return new WaitForSeconds(immotalTime);

                immortal = false;
            }
            else
            {
                MyAnimator.SetLayerWeight(1, 0);
                MyAnimator.SetTrigger("die");
            }
        }

    }

    private IEnumerator IndicateImmotal()
    {
        while (immortal)
        {
            spriterender.enabled = false;
            yield return new WaitForSeconds(.1f);
            spriterender.enabled = true;
            yield return new WaitForSeconds(.1f);
        }
    }

    public void OnDead() {
        if (Dead != null)
        {
            Dead();
        }
    }

    public override void Death()
    {
        MyRigidBody.velocity = Vector2.zero;
        MyAnimator.SetTrigger("idle");
        health = 30;
        transform.position = startPos;
    }

    public void BtnJump()
    {
        MyAnimator.SetTrigger("jump");
        Jump = true;

    }
    public void BtnAttack()
    {
        MyAnimator.SetTrigger("attack");
    }
    public void BtnSlide()
    {
        MyAnimator.SetTrigger("slide");
    }
    public void BtnThrow()
    {
        MyAnimator.SetTrigger("throw");
    }
    public void BtnMove(float direction)
    {
        this.direction = direction;
        this.move = true;
    }

    public void BtnStopMove()
    {
        this.direction = 0;
        this.move = false;
        this.btnhorizontal = 0;
    }

}


