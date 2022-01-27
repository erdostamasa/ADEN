using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

public class CapsuleController : MonoBehaviour {
    public GameObject connectPoint;

    public GameObject mainThruster = null;
    public GameObject frrO;
    public GameObject fruO;
    public GameObject fllO;
    public GameObject fluO;
    public GameObject brrO;
    public GameObject brdO;
    public GameObject bllO;
    public GameObject bldO;

    private ThrusterScript mts;
    private ThrusterScript frr;
    private ThrusterScript fru;
    private ThrusterScript fll;
    private ThrusterScript flu;
    private ThrusterScript brr;
    private ThrusterScript brd;
    private ThrusterScript bll;
    private ThrusterScript bld;

    public float displayConnectPointDistance = 2f;

    public float thrusterPower = 1f;
    public float mainSpeed = 2f;

    public Grabber grabber;

    public List<GameObject> possibleConnectPoints = new List<GameObject>();
    private Transform closestConnectTransform;
    public GameObject connected;
    public Connectable connectedScript;

    private void Awake() {
        mts = mainThruster.GetComponent<ThrusterScript>();
        frr = frrO.GetComponent<ThrusterScript>();
        fru = fruO.GetComponent<ThrusterScript>();
        fll = fllO.GetComponent<ThrusterScript>();
        flu = fluO.GetComponent<ThrusterScript>();
        brr = brrO.GetComponent<ThrusterScript>();
        brd = brdO.GetComponent<ThrusterScript>();
        bll = bllO.GetComponent<ThrusterScript>();
        bld = bldO.GetComponent<ThrusterScript>();
    }

    public bool isConnected = false;
    public bool canConnect = false;
    private IDictionary<string, bool> inputs = new Dictionary<string, bool>();

    private Slider mainSlider;
    private Slider posSlider;
    private bool usingSliders = false;

    private void Start() {
        if (GameMaster.Instance.mainPowerSlider != null && GameMaster.Instance.sidePowerSlider != null) {
            mainSlider = GameMaster.Instance.mainPowerSlider;
            posSlider = GameMaster.Instance.sidePowerSlider;
            usingSliders = true;
        }

        UpdateConnectPoints();
    }

    public void UpdateConnectPoints() {
        possibleConnectPoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("connectPoint"));
    }

    private void FixedUpdate() {
        if (possibleConnectPoints.Count > 0) {
            closestConnectTransform = GetClosest(possibleConnectPoints);


            //Require rotation to be matched with connecting part
            float thisAngle = transform.rotation.eulerAngles.z;
            float thatAngle = closestConnectTransform.rotation.eulerAngles.z;
            float deltaAngle = Mathf.Abs(Mathf.DeltaAngle(thisAngle, thatAngle));

            //Control visual display of connect points
            if (!connected && canConnect) {
                if (Vector2.Distance(connectPoint.transform.position, closestConnectTransform.position) >
                    displayConnectPointDistance) {
                    if (connectPoint.GetComponent<Light2D>().enabled) {
                        connectPoint.GetComponent<Light2D>().enabled = false;
                    }

                    if (closestConnectTransform.GetComponent<Light2D>().enabled) {
                        closestConnectTransform.GetComponent<Light2D>().enabled = false;
                    }
                }
                else {
                    if (!connectPoint.GetComponent<Light2D>().enabled) {
                        connectPoint.GetComponent<Light2D>().enabled = true;
                    }

                    if (!closestConnectTransform.GetComponent<Light2D>().enabled) {
                        closestConnectTransform.GetComponent<Light2D>().enabled = true;
                    }
                }
            }

            //connect if conditions are met
            if (canConnect && Vector2.Distance(connectPoint.transform.position, closestConnectTransform.position) <
                0.6f && deltaAngle < 60f) {
                if (!isConnected) {
                    Connect();
                    connectPoint.GetComponent<Light2D>().enabled = false;
                    closestConnectTransform.GetComponent<Light2D>().enabled = false;
                }
            }
        }


        if (isConnected) {
            if (connected.transform.localPosition != connectedScript.offset) {
                connected.transform.localPosition = connectedScript.offset;
            }

            if (connected.transform.localRotation != quaternion.identity) {
                connected.transform.localRotation = quaternion.identity;
            }

            inputs["W"] = Input.GetKey(KeyCode.W);
            inputs["A"] = Input.GetKey(KeyCode.A);
            inputs["S"] = Input.GetKey(KeyCode.S);
            inputs["D"] = Input.GetKey(KeyCode.D);
            inputs["RIGHT"] = Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.E);
            inputs["LEFT"] = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.Q);
            inputs["UP"] = Input.GetKey(KeyCode.UpArrow);
            inputs["DOWN"] = Input.GetKey(KeyCode.DownArrow);

            connectedScript.Control(inputs);
        }
        else {
            //main engine
            if (Input.GetKey(KeyCode.W)) {
                mts.Fire(mainSpeed);
            }

            //strafe left
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
                frr.Fire(thrusterPower / 1.75f);
                brr.Fire(thrusterPower);
            }

            //strafe right
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
                fll.Fire(thrusterPower / 1.75f);
                bll.Fire(thrusterPower);
            }

            //forward thruster
            if (Input.GetKey(KeyCode.UpArrow)) {
                bld.Fire(thrusterPower);
                brd.Fire(thrusterPower);
            }

            //backwards truster
            if (Input.GetKey(KeyCode.DownArrow) ||Input.GetKey(KeyCode.S)) {
                flu.Fire(thrusterPower);
                fru.Fire(thrusterPower);
            }

            //turn left
            if (Input.GetKey(KeyCode.Q)) {
                frr.Fire(thrusterPower);
                bll.Fire(thrusterPower);
            }

            //turn right
            if (Input.GetKey(KeyCode.E)) {
                fll.Fire(thrusterPower);
                brr.Fire(thrusterPower);
            }
        }
    }

    private Transform GetClosest(List<GameObject> objects) {
        Transform closest = null;
        float closestDistSqr = Mathf.Infinity;
        Vector2 currentPosition = transform.position;
        foreach (GameObject obj in objects) {
            Vector2 directionToTarget = (Vector2)obj.transform.position - currentPosition;
            float sqrDist = directionToTarget.sqrMagnitude;
            if (sqrDist < closestDistSqr) {
                closest = obj.transform;
                closestDistSqr = sqrDist;
            }
        }

        return closest;
    }

    void Update() {
        if (usingSliders) {
            mainSpeed = mainSlider.value;
            thrusterPower = posSlider.value;
        }

        UpdateConnectPoints();

        if (Input.GetKeyDown(KeyCode.T)) {
            Disconnect();
        }
    }


    public void Connect() {
        //grabber.RetractGrabber();

        connected = closestConnectTransform.parent.gameObject;

        transform.rotation = connected.transform.rotation;


        Rigidbody2D thisRb = GetComponent<Rigidbody2D>();
        Rigidbody2D thatRb = connected.GetComponent<Rigidbody2D>();

        Vector2 weightedVelocity = (thisRb.mass * thisRb.velocity + thatRb.mass * thatRb.velocity) /
                                   (thisRb.mass + thatRb.mass);
        float weightedAngularVelocity = (thisRb.mass * thisRb.angularVelocity + thatRb.mass * thatRb.angularVelocity) /
                                        (thisRb.mass + thatRb.mass);


        isConnected = true;
        canConnect = false;

        connected.GetComponent<GravityApplier>().enabled = false;
        connectedScript = connected.GetComponent<Connectable>();
        Transform connectedTransform = connected.transform;
        Destroy(closestConnectTransform.parent.gameObject.GetComponent<Rigidbody2D>());
        connectedTransform.parent = transform;
        connectedTransform.localEulerAngles = Vector3.zero;
        connectedTransform.localPosition = connectedScript.offset;

        thisRb.velocity = weightedVelocity;
        thisRb.angularVelocity = weightedAngularVelocity;
    }

    //waits given seconds before making a connection possible again
    private IEnumerator ConnectTimeout(float seconds) {
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(),
            closestConnectTransform.parent.GetComponent<Collider2D>(), true);
        yield return new WaitForSeconds(seconds);
        canConnect = true;
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(),
            closestConnectTransform.parent.GetComponent<Collider2D>(), false);
    }

    public void DestroyConnected() {
        Destroy(connected);
        connected = null;
        isConnected = false;
        connectedScript = null;
        canConnect = true;
        //StartCoroutine(ConnectTimeout(3f));
    }

    public void Disconnect() {
        if (!connected) return;

        StartCoroutine(ConnectTimeout(3f));
        canConnect = false;
        isConnected = false;

        Rigidbody2D thisRb = GetComponent<Rigidbody2D>();

        connected.transform.parent = null;


        Rigidbody2D rb = connected.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.drag = 0;
        rb.angularDrag = 0;

        connected.GetComponent<GravityApplier>().enabled = true;
        rb.velocity = thisRb.velocity;
        rb.angularVelocity = thisRb.angularVelocity;
        connected.GetComponent<GravityApplier>().rb = rb;


        connected = null;
        connectedScript = null;
        //rb.AddForce(transform.up * -1, ForceMode2D.Impulse);
        GetComponent<Rigidbody2D>().AddForce(transform.up * 3f, ForceMode2D.Impulse);
    }
}