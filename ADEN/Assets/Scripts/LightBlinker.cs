using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightBlinker : MonoBehaviour {
    public Light2D mylight;

    void Start() {
        myLight = GetComponent<Light2D>();
        StartCoroutine(flashNow());
    }
    
    
    
    public float totalSeconds;     // The total of seconds the flash wil last
    public float maxIntensity;     // The maximum intensity the flash will reach
    public float minIntensity;
    public Light2D myLight;        // Your light
 
    public IEnumerator flashNow ()
    {
        float waitTime = totalSeconds / 2;                        
        // Get half of the seconds (One half to get brighter and one to get darker)
        while (myLight.intensity < maxIntensity) {
            myLight.intensity += Time.deltaTime / waitTime;        // Increase intensity
            yield return null;
        }
        while (myLight.intensity > minIntensity) {
            myLight.intensity -= Time.deltaTime / waitTime;        //Decrease intensity
            yield return null;
        }

        yield return StartCoroutine(flashNow());
    }
    

    void Update() {

        //StartCoroutine(flashNow());
        
        //light.intensity = Mathf.PingPong(Time.time, 1);
        /*
        float value = light.volumeOpacity;
        if (value >= min){
            value = Mathf.Lerp(min, max, Time.deltaTime)
        } else{
            
        }*/
    }
}