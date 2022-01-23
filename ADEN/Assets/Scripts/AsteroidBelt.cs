using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assertions.Must;
using Random = UnityEngine.Random;

[ExecuteInEditMode]
public class AsteroidBelt : MonoBehaviour {
    public int numOfAsteroids;

    public List<GameObject> asteroids = new List<GameObject>();
    
    public GameObject centerPrefab;
    public Transform center;
    public float rotationRadius;
    public bool debugMode;
    public float angle;
    public float orbitOffset;
    
    
    public bool spawn = false;
    public bool delete;
    public int deleted = 0;
    public float minScale = 0.2f;
    public float maxScale = 1.5f;
    

    public float averageScale = 1f;
    public float stdDeviation = 0.3f;
    
    private void Awake() {
        debugMode = false;
    }

    public int errorCounter;

    public bool autoSpawn = false;
    
    private void Start() {
        if (autoSpawn){
            DeleteAsteroids();
            SpawnAsteroids();    
        }
        
    }
    

    private void DeleteAsteroids() {
        
        List<Transform> children = new List<Transform>();
        foreach (Transform child in transform){
            children.Add(child);
        }
            
        foreach (Transform child in children){
            DestroyImmediate(child.gameObject);
        }
    }
    
    private void SpawnAsteroids() {
        deleted = 0;
        int spawned = 0;
        errorCounter = 0;
        while(errorCounter < 10000 && spawned <= numOfAsteroids){
            //Choose a random asteroid from list
            GameObject rndAsteroid = asteroids[Random.Range((int)0, (int)asteroids.Count)];
                
            Vector2 pos = RandomCirclePosition();
            GameObject newAsteroid = Instantiate(rndAsteroid, pos, Quaternion.identity);
            float scale = RandomFromDistribution.RandomNormalDistribution(averageScale, stdDeviation);
            if (scale < minScale) scale = minScale;
            if (scale > maxScale) scale = maxScale;
            newAsteroid.transform.localScale = new Vector3(scale, scale, 1);
            Collider2D[] contacts = new Collider2D[1];
            if ( Physics2D.OverlapBoxAll(pos, newAsteroid.transform.localScale * 5f, 0).Length > 1){
                DestroyImmediate(newAsteroid);
                deleted++;
            } else{
                newAsteroid.transform.parent = transform;
                newAsteroid.transform.rotation = quaternion.Euler(0, 0, Random.Range(0f, 360f));
                spawned++;
            }

            errorCounter++;
        }
    }
    
    //DEBUG IN EDITOR
    private void Update() {
        if (spawn){
            spawn = false;
            SpawnAsteroids();
        }

        
        if (delete){
            delete = false;
            DeleteAsteroids();
        }
        
        if (debugMode){
            if (center != null){
                RenderOrbit();                
            }
            //move in circles
            float posX = center.position.x + Mathf.Cos(angle) * rotationRadius;
            float posY = center.position.y + Mathf.Sin(angle) * rotationRadius;
            transform.position = new Vector2(posX, posY);
            if (angle > Mathf.PI * 2) angle = 0f; //if angle reached 2PI radians, reset it
        }
    }


    private Vector2 RandomCirclePosition() {
        //calculate random offset form orbit
        //float offset = Random.Range(-orbitOffset, orbitOffset);
        float offset = RandomFromDistribution.RandomNormalDistribution(0, orbitOffset);
        float orbit = rotationRadius + offset;
        float randomAngle = Random.Range(0f, 360f);

        float posX = center.position.x + Mathf.Cos(randomAngle * Mathf.Deg2Rad) * orbit;
        float posY = center.position.y + Mathf.Sin(randomAngle * Mathf.Deg2Rad) * orbit;
        
        return new Vector2(posX, posY);
    }



    private void RenderOrbit() {
        //if the planet doesn't already have a center GameObject with a LineRenderer, create one
        bool hasCircle = false;
        GameObject centerObject = null;
        foreach (Transform child in center){
            if (child.CompareTag("orbitCircle") && child.name == ("orbit" + this.name)){
                hasCircle = true;
                centerObject = child.gameObject;
            }
        }

        //gives "center" a child with a lineRenderer and sets its name to 'orbit' + this planets name
        if (!hasCircle){
            centerObject = Instantiate(centerPrefab, center.position, Quaternion.identity);
            centerObject.name = "orbit" + this.name;
            centerObject.transform.parent = center.transform;
            centerObject.transform.localPosition = new Vector3(0, 0, 20);
        }

        LineRenderer orbitRenderer = centerObject.GetComponent<LineRenderer>();
        int lineSegments = 1000;

        orbitRenderer.positionCount = lineSegments + 1;
        orbitRenderer.useWorldSpace = false;

        float deltaTheta = (float) (2.0 * Mathf.PI) / lineSegments;
        float theta = 0f;
        //draw the circle from given number of line segments
        for (int i = 0; i < lineSegments + 1; i++){
            float x = rotationRadius * Mathf.Cos(theta);
            float y = rotationRadius * Mathf.Sin(theta);
            Vector3 pos = new Vector3(x, y, 1);
            orbitRenderer.SetPosition(i, pos);
            theta += deltaTheta;
        }
    }
    
}