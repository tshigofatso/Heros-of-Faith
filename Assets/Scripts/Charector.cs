using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Charector : MonoBehaviour {



    [SerializeField]
    private EdgeCollider2D swordCollider;
    [SerializeField]
    private Transform knifePosition;
    //this is movement speed
    [SerializeField]
    protected float MovementSpeed = 10.0f;
    protected bool facingRight;
    [SerializeField]
    protected GameObject knifePrefab;
    [SerializeField]
    protected int health;
    [SerializeField]
    private List<string> damageSources;

    public abstract bool IsDead { get; }
    public abstract IEnumerator TakeDamage();

    public abstract void Death();

    public bool Attack { get; set; }
    public Animator MyAnimator { get; private set; }

    public bool TakingDamage { get; set; }

    public EdgeCollider2D SwordCollider
    {
        get
        {
            return swordCollider;
        }

    }


    // Use this for initialization
    public virtual void Start () {

        //assign the direction on the start to the game instead of on fixedUpdRW
        facingRight = true;
        MyAnimator = GetComponent<Animator>();


    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ChangeDirection()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(transform.localScale.x * -1,1,1);
    }

    public virtual void ThrowKnife(int value) {
        if (facingRight)
        {
            GameObject tmp = (GameObject)Instantiate(knifePrefab, knifePosition.position, Quaternion.identity);
            tmp.GetComponent<Knife>().Initialize(Vector2.right);
        }
        else
        {
            GameObject tmp = (GameObject)Instantiate(knifePrefab, knifePosition.position, Quaternion.Euler(0, 0, 180));
            tmp.GetComponent<Knife>().Initialize(Vector2.left);
        }
    }

    

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (damageSources.Contains(other.tag))
        {
            StartCoroutine(TakeDamage());
        }
    }

    public void MeleeAttack()
    {
        SwordCollider.enabled = true;
    }
}
