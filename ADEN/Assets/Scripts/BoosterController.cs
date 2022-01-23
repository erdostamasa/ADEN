using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterController : Connectable {
    public ThrusterScript ru;
    public ThrusterScript rr;
    public ThrusterScript rd;
    public ThrusterScript lu;
    public ThrusterScript ll;
    public ThrusterScript ld;
    public ThrusterScript ml;
    public ThrusterScript md;
    public ThrusterScript mr;
    
    public override void Control(IDictionary<string, bool> inputs) {
        if (inputs["W"]){
            ml.Fire(mainPower);
            md.Fire(mainPower);
            mr.Fire(mainPower);
        }

        if (inputs["A"]){
            lu.Fire(sidePower);
            rd.Fire(sidePower);
        }

        if (inputs["D"]){
            ld.Fire(sidePower);
            ru.Fire(sidePower);
        }

        if (inputs["S"]){
            lu.Fire(sidePower);
            ru.Fire(sidePower);
        }

        if (inputs["LEFT"]){
            rr.Fire(sidePower);
        }

        if (inputs["RIGHT"]){
            ll.Fire(sidePower);
        }

        if (inputs["UP"]){
            ld.Fire(sidePower);
            rd.Fire(sidePower);
        }

        if (inputs["DOWN"]){
            lu.Fire(sidePower);
            ru.Fire(sidePower);
        }
    }
}