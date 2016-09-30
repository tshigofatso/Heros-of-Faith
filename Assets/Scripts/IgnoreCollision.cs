using UnityEngine;
using System.Collections;

public class IgnoreCollision : MonoBehaviour {

    [SerializeField]
    private Collider2D other;

	// Use this for initialization
	void Awake() {
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(),other,true);
	}
	
	
}
