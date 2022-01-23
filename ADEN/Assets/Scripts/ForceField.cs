using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceField : MonoBehaviour {
    public float attractionForce;

    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.layer == 14){
            Vector2 dir = transform.position - other.transform.position;

            float forceMagnitude = Mathf.Lerp(0.1f, GetComponent<CircleCollider2D>().radius,
                Vector2.Distance(transform.position, other.transform.position));

            other.GetComponent<Rigidbody2D>()
                .AddForce(dir.normalized * other.GetComponent<Rigidbody2D>().mass * forceMagnitude * attractionForce);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == 14){
            other.GetComponent<Rigidbody2D>().drag = 1f;
            other.GetComponent<Rigidbody2D>().angularDrag = 0.9f;
            Physics2D.IgnoreCollision(transform.parent.parent.GetComponent<Collider2D>(), other.GetComponent<Collider2D>(), true);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.layer == 14){
            other.GetComponent<Rigidbody2D>().drag = 0f;
            other.GetComponent<Rigidbody2D>().angularDrag = 0f;
            Physics2D.IgnoreCollision(transform.parent.parent.GetComponent<Collider2D>(), other.GetComponent<Collider2D>(), false);
        }
    }
}