using UnityEngine;

public class Rope : MonoBehaviour {

    public GameObject hook;
    public GameObject grabberPrefab;
    public GameObject linkPrefab;
    public int linkCount = 7;
    void Start() {
        GenerateRope();
    }

    private void GenerateRope() {
        Rigidbody2D previousRB = hook.GetComponent<Rigidbody2D>();
        for (int i = 0; i < linkCount; i++){
            GameObject link = Instantiate(linkPrefab, transform);
            HingeJoint2D joint = link.GetComponent<HingeJoint2D>();
            joint.connectedBody = previousRB;

            previousRB = link.GetComponent<Rigidbody2D>();
            if (i == linkCount-1){
                GameObject grabber = Instantiate(grabberPrefab, transform);
                grabber.GetComponent<HingeJoint2D>().connectedBody = previousRB;
            }
            
        }
        
    }
}