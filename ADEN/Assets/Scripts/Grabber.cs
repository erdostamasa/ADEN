using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Grabber : MonoBehaviour {
    public GameObject contact;
    public bool hasGrabbed;

    public Collider2D overlapped;
    private FixedJoint2D joint;
    public float overlapRadius = 1f;

    public CapsuleController capsule;
    
    public float ejectionForce = 0f;

    public GameObject grabbedObject;

    private Animator animator;

    private void Start() {
        oldPos = contact.transform.position;
        animator = GetComponent<Animator>();
    }

    private bool grabberExtended = false;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.G)/* && !capsule.connected*/){

            if (hasGrabbed){
                Release();
            }
            
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("grabberClosed")){
                
                animator.SetTrigger("extendGrabber");
                animator.ResetTrigger("retractGrabber");
            } else if(animator.GetCurrentAnimatorStateInfo(0).IsName("GrabberOpen")){
                
                animator.SetTrigger("retractGrabber");
                animator.ResetTrigger("extendGrabber");
            }
        }

        if (Input.GetKeyDown(KeyCode.Space)){
            if (joint != null){
                Release();
            } else if (overlapped != null){
                Grab();
            }
        }
    }

    public void RetractGrabber() {
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("GrabberOpen")){
                
            animator.SetTrigger("retractGrabber");
            animator.ResetTrigger("extendGrabber");
        }
    }
    
    private void Release() {
        joint.connectedBody = null;
        Destroy(joint);
        hasGrabbed = false;
        grabbedObject.GetComponent<CustomTag>().RemoveTag("grabbed");
        grabbedObject.GetComponent<Rigidbody2D>().AddForce(transform.up * ejectionForce, ForceMode2D.Impulse);
        grabbedObject = null;
    }

    private void Grab() {
        if (overlapped != null){
            CustomTag ct = overlapped.GetComponent<CustomTag>();
            if (ct != null && ct.HasTag("grabbable")){
                hasGrabbed = true;
                //overlapped.gameObject.transform.position = contact.transform.position;
                joint = overlapped.gameObject.AddComponent<FixedJoint2D>();
                joint.connectedBody = transform.root.GetComponent<Rigidbody2D>();
                
                grabbedObject = overlapped.gameObject;
                ct.AddTag("grabbed");
            }
        }
    }

    private Vector2 oldPos;
    private Vector2 newPos;
    public Vector2 contactVelocity;

    private void FixedUpdate() {
        newPos = contact.transform.position;
        contactVelocity = (newPos - oldPos) / Time.fixedDeltaTime;
        oldPos = newPos;

        //overlapped = Physics2D.OverlapPoint(contact.transform.position);
        overlapped = Physics2D.OverlapCircle(contact.transform.position, overlapRadius);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(contact.transform.position, overlapRadius);
    }
}