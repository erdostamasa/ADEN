using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BuildingPlacer : MonoBehaviour {
    public GameObject listeningPost;

    public float buildingCost = 1f;
    //public GameObject waterCollector;

    public float verticalOffset;

    private GameObject selectedBuilding;
    private Transform planet;
    private GameObject indicator;
    private bool canPlace;
    void Start() {
        canPlace = false;
    }

    private RaycastHit2D hit;

    private void Update() {
        if (QuestManager.instance.currentQuest.goal.goalType != GoalType.BuildScanningPost) return;
        
        //TODO: implement function for multiple planets?
        //set planet to currently orbited
        if (GetComponent<GravityApplier>().planets.Count != 0){
            planet = GetComponent<GravityApplier>().planets[0].gameObject.transform;
        } else{
            selectedBuilding = null;
            Destroy(indicator);
        }

        if (planet == null) return;
        CustomTag ct = planet.GetComponent<CustomTag>();
        if (ct == null) return;
        if (!ct.HasTag("buildScanner")) return;
        
        if (Input.GetKeyDown(KeyCode.Alpha0)){
            Destroy(indicator);
            selectedBuilding = null;
            indicator = null;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)){
            Destroy(indicator);
            selectedBuilding = listeningPost;
            indicator = Instantiate(selectedBuilding, hit.point, quaternion.identity);
            
            indicator.transform.rotation = Quaternion.FromToRotation(Vector3.left, hit.normal);
            indicator.transform.Rotate(Vector3.forward, 90f);

            
            
            indicator.GetComponent<Collider2D>().isTrigger = true;
            indicator.transform.parent = planet;
            Physics2D.IgnoreCollision(indicator.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
/*
        if (Input.GetKeyDown(KeyCode.Alpha2)){
            Destroy(indicator);
            selectedBuilding = waterCollector;
            indicator = Instantiate(selectedBuilding, hit.point, quaternion.identity);
            indicator.GetComponent<Collider2D>().isTrigger = true;
            indicator.transform.parent = planet;
            Physics2D.IgnoreCollision(indicator.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }*/
        
        //place building if the indicator is on a planet
        if (selectedBuilding != null && Input.GetMouseButtonDown(0)){
            if (hit.collider.gameObject.layer == 11 && canPlace){
                GameObject selectedObject = Instantiate(selectedBuilding, hit.point, quaternion.identity);
                selectedObject.transform.parent = planet;
                selectedObject.transform.position = indicator.transform.position;
                selectedObject.transform.rotation = indicator.transform.rotation;
                planet.GetComponent<Buildable>().metalResources -= buildingCost;
                planet.GetComponent<Buildable>().buildingCount++;
                Debug.DrawLine(hit.point, (hit.point + hit.normal * 5), Color.red, 10f);
            }
        }
    }

    void FixedUpdate() {
        if (planet == null) return;

        if (selectedBuilding != null){
            int mask = 1 << LayerMask.NameToLayer("planet");
            Vector3 mpv3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos = new Vector2(mpv3.x, mpv3.y);
            hit = Physics2D.Raycast(mousePos, new Vector2(planet.position.x, planet.position.y) - mousePos, 100, mask);
            Debug.DrawLine(mousePos, mousePos + new Vector2(planet.position.x, planet.position.y) - mousePos);

            //display placement indicator and update its position
            indicator.transform.rotation = Quaternion.FromToRotation(Vector3.left, hit.normal);
            indicator.transform.Rotate(Vector3.forward, 90f);
            indicator.transform.position = (Vector3) (hit.point + (hit.normal * (indicator.transform.lossyScale.y)) + (hit.normal * (verticalOffset)) ) + new Vector3(0, 0, -5);

            Collider2D indicatorCollider = indicator.GetComponent<Collider2D>();
            ContactFilter2D filter = new ContactFilter2D();
            Collider2D[] results = new Collider2D[10];
            //Only able to place item if its not colliding with anything
            Buildable buildable = planet.GetComponent<Buildable>();
            if (indicatorCollider.OverlapCollider(filter, results) == 0 && buildable.metalResources >= buildingCost && buildable.buildingCount < buildable.neededBuildingCount){
                indicator.GetComponent<SpriteRenderer>().color = Color.green;
                canPlace = true;
            } else{
                indicator.GetComponent<SpriteRenderer>().color = Color.red;
                canPlace = false;
            }
        }
    }
}