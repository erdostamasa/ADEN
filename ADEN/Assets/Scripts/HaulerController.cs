using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaulerController : Connectable {
    public GameObject leftUp;
    public GameObject leftDown;
    public GameObject rightUp;
    public GameObject rightDown;

    private ThrusterScript lu;
    private ThrusterScript ld;
    private ThrusterScript ru;
    private ThrusterScript rd;

    private void Start() {
        lu = leftUp.GetComponent<ThrusterScript>();
        ld = leftDown.GetComponent<ThrusterScript>();
        ru = rightUp.GetComponent<ThrusterScript>();
        rd = rightDown.GetComponent<ThrusterScript>();
        mass = GetComponent<Rigidbody2D>().mass;
    }

    public override void Control(IDictionary<string, bool> inputs) {
        if (inputs["W"]){
            ld.Fire(mainPower);
            rd.Fire(mainPower);
        }

        if (inputs["S"]){
            lu.Fire(mainPower);
            ru.Fire(mainPower);
        }

        if (inputs["A"]){
            lu.Fire(mainPower);
            rd.Fire(mainPower);
        }

        if (inputs["D"]){
            ld.Fire(mainPower);
            ru.Fire(mainPower);
        }
    }
}