using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapLineRenderer : MonoBehaviour {
    // Start is called before the first frame update

    private LineRenderer line;
    private LineRenderer parentLine;
    public Material material;
    
    void Start() {
        parentLine = transform.parent.GetComponent<LineRenderer>();
        line = gameObject.AddComponent<LineRenderer>();
        line.positionCount = parentLine.positionCount;
        
        Vector3[] positions = new Vector3[2000];
        parentLine.GetPositions(positions);
        
        line.SetPositions(positions);
        line.widthMultiplier = 20;
        line.sharedMaterial = material;
    }

    // Update is called once per frame
    void Update() { }
}