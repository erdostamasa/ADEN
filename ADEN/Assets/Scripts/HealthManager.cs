using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

public class HealthManager : MonoBehaviour {
    public int health;
    public float damageThreshold = 1f;
    private bool canTakeDamage = true;
    [SerializeField] GameObject explosionParticles;
    [SerializeField] float deathWaitDuration = 3f;


    public void Damage(int damage, int cooldown) {
        canTakeDamage = false;
        health -= damage;

        if (health <= 0) {
            EventManager.instance.PlayerDied();
        }

        AudioManager.Instance.PlayDamage(transform.position, 1f);
        StartCoroutine(DamageTimeout(cooldown));
    }
    

    private IEnumerator DamageTimeout(float seconds) {
        yield return new WaitForSeconds(seconds);
        canTakeDamage = true;
    }

    private void OnCollisionStay2D(Collision2D other) {
        CustomTag ct = other.gameObject.GetComponent<CustomTag>();
        if (ct != null && other.gameObject.GetComponent<CustomTag>().HasTag("lavaPlanet")) {
            if (canTakeDamage) {
                Damage(1, 1);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        //If collision force is greater than threshold, do damage relative to the force (max: 80)
        if (canTakeDamage && other.relativeVelocity.magnitude > damageThreshold) {
            ContactPoint2D[] contacts = new ContactPoint2D[1];
            other.GetContacts(contacts);
            if (contacts[0].otherCollider.gameObject.CompareTag("Player")) {
                if (canTakeDamage) {
                    Damage(Convert.ToInt32(Mathf.Lerp(0f, 100f, other.relativeVelocity.magnitude / 10f)), 2);
                }
            }
            else {
                //if connected to module, receive quarter of damage
                if (canTakeDamage) {
                    Damage((int)(Mathf.Lerp(0f, 100f, other.relativeVelocity.magnitude / 100f) / 4f), 2);
                }
            }
        }
    }
}