using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartGame : MonoBehaviour {

    public GameObject gravitySlider;

    public List<GameObject> planets;

    void Start() {
        planets = new List<GameObject>(GameObject.FindGameObjectsWithTag("planet"));
    }

    void Update() {

        foreach(GameObject planet in planets) {

            //planet.GetComponent<Attractor>().currentGravityScale = planet.GetComponent<Attractor>().gravityScale * gravitySlider.GetComponent<Slider>().value;
        }

        if (Input.GetKey(KeyCode.R)) {
            SceneManager.LoadScene(0);
        }
    }
}
