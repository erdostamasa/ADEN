using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RocketController : MonoBehaviour {

    public float capsuleMass;

    public Vector2 startVeloctiy;

    public GameObject capsule = null;
    public Transform leftThrust = null;
    public Transform rightTrust = null;

    public GameObject currentPlanet = null;

    public GameObject fuelBar = null;
    private Slider s1;
    private Slider s2;

    public float totalFuel;
    public float startingFuel;

    

    public float steerConsumption = 0.05f;
    public float steerSpeed = 3f;

    public float totalMass;
    public List<GameObject> stages = new List<GameObject>();
    public GameObject activeStage;

    public float currentGravity;


    public bool lerpGravity = true;

    public void DropStage() {
        if(stages.Count > 0) {
            GameObject lastStage = stages[stages.Count - 1];
            lastStage.transform.parent = null;
           // GetComponent<Rigidbody2D>().mass -= lastStage.GetComponent<StageScript>().mass;
           // lastStage.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            lastStage.GetComponent<Rigidbody2D>().gravityScale = 0;
            lastStage.GetComponent<Rigidbody2D>().drag = 0;
            stages.Remove(lastStage);
            if(stages.Count > 0) {
                activeStage = stages[stages.Count - 1];
                activeStage.GetComponent<StageScript>().isActive = true;
            } else {
                DeployCapsule();
                capsule.GetComponent<CapsuleController>().enabled = true;
            }
        }        
    }

    private void DeployCapsule() {

    }

    public void FuelPickup() {
        if (activeStage == this.gameObject) {
            activeStage.GetComponent<StageScript>().fuel += 10;
        }
    }


    private void Start() {
        GetComponent<Rigidbody2D>().velocity = startVeloctiy;

        if (s1 != null && s2 != null) {
            s1 = fuelBar.transform.GetChild(0).GetComponent<Slider>();
            s2 = fuelBar.transform.GetChild(1).GetComponent<Slider>();
        }



        if (stages.Count > 0) {
            totalMass = capsuleMass;
            foreach (GameObject stage in stages) {
                //totalMass += stage.GetComponent<StageScript>().mass;
            }
            GetComponent<Rigidbody2D>().mass = totalMass;

            startingFuel = 0;
            foreach (GameObject stage in stages) {
                startingFuel += stage.GetComponent<StageScript>().fuel;
            }
            totalFuel = startingFuel;

            activeStage = stages[stages.Count - 1];
            activeStage.GetComponent<StageScript>().isActive = true;
        } else {
            DeployCapsule();
        }

    }


    private void Update() {

        float tmp = 0;
        foreach (GameObject stage in stages) {
            tmp += stage.GetComponent<StageScript>().fuel;
        }
        totalFuel = tmp;

        //currentGravity = Mathf.Lerp(1f, -0.2f, CubicBezier(0.92f, 0.57f, transform.position.y / 166));

        //if (lerpGravity) GetComponent<Rigidbody2D>().gravityScale = currentGravity;

        if (s1 != null && s2 != null) {
            s1.value = Mathf.Lerp(0, 1, totalFuel / startingFuel);
            s2.value = Mathf.Lerp(0, 1, totalFuel / startingFuel);
        }
        
    }

    private void FixedUpdate() {

        //transform.position += Vector3.up * 10 * Time.fixedDeltaTime;
        /*
        if(totalFuel > 0) {
            if (Input.GetKey(KeyCode.A)) {
                GetComponent<Rigidbody2D>().AddForceAtPosition(rightTrust.right * -1  * steerSpeed, rightTrust.position, ForceMode2D.Force);
                totalFuel -= steerConsumption;
            }
            if (Input.GetKey(KeyCode.D)) {
                GetComponent<Rigidbody2D>().AddForceAtPosition(leftThrust.right * steerSpeed, leftThrust.position, ForceMode2D.Force);
                totalFuel -= steerConsumption;
            }
        }*/
        
    }
    /*
    private void OnTriggerEnter2D(Collider2D collision) {
        FuelPickup();
        Destroy(collision.gameObject);
    }*/

    private float CubicBezier(float y1, float y2, float t) {
        float v1 = 0;
        float v2 = y1;
        float v3 = y2;
        float v4 = 1;
       
        float result = Mathf.Pow((1-t),3)*v1 + Mathf.Pow((1-t),2)*3*t*v2 + 3*Mathf.Pow(t,2)*(1-t)*v3 + Mathf.Pow(t,3)*v4;

        return result;
    }

}
