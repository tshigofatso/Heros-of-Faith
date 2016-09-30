using UnityEngine;
using System.Collections;

public class SwordCollier : MonoBehaviour {
    [SerializeField]
    private string TargetTag;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == TargetTag)
        {
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
