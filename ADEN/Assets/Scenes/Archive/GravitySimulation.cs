using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySimulation : MonoBehaviour {

    public RealGravity[] bodies;

    private void Awake() {
        bodies = FindObjectsOfType<RealGravity>();
    }

    private void FixedUpdate() {
        for (int i = 0; i < bodies.Length; i++) {
            //Vector3 acceleration = CalculateAcceleration(bodies[i].Position, bodies[i]);
            //bodies[i].UpdateVelocity(acceleration, Universe.physicsTimeStep);
            bodies[i].UpdateVelocity (bodies, Time.fixedDeltaTime);
        }

        for (int i = 0; i < bodies.Length; i++) {
            bodies[i].UpdatePosition(Time.fixedDeltaTime);
        }
    }

}
