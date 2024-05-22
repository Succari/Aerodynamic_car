using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    public double mass = 0.00000000001;
    public float radius = 0.1f;
    // public float C_d = 0.09f;
    public float density;
    public float pressure;
    public Vector3 velocity;
    public Vector3 position;
    public Vector3 force;
    public List<ParticleDistancePair> particleDistancePairs;
    /*
    public float isotropic_exponent;
    public float smoothing_length;
    public float dynamic_velcotiy;
    public float damping_coeffecient;

    */

    public void setInfo(double mass, float radius, Vector3 position, Vector3 velocity)
    {
        this.mass = mass;
        this.radius = radius;
        this.position = position;
        this.position = new Vector3(
            Mathf.FloorToInt(this.position.x * 100.0f) / 100.0f,
            Mathf.FloorToInt(this.position.y * 100.0f) / 100.0f,
            Mathf.FloorToInt(this.position.z * 100.0f) / 100.0f
        );
        this.gameObject.transform.position = this.position;
        this.velocity = velocity;
        particleDistancePairs = new List<ParticleDistancePair>();
        
    }
    public void addParticleDistance(Particle pr, float distance)
    {
        particleDistancePairs.Add(new ParticleDistancePair(pr, distance));
    }
    public void setVelocity(Vector3 velocity)
    {
        this.velocity = velocity;
    }
    public void updateVelocity(Vector3 force, float dt = 0.02f)
    {
        this.velocity.x += (float) ((force.x / this.density) * dt);
        this.velocity.y += (float) ((force.y / this.density) * dt);
        this.velocity.z += (float) ((force.z / this.density) * dt);
        this.velocity.x = Mathf.FloorToInt(this.velocity.x * 100.0f) / 100.0f;
        this.velocity.y = Mathf.FloorToInt(this.velocity.y * 100.0f) / 100.0f;
        this.velocity.z = Mathf.FloorToInt(this.velocity.z * 100.0f) / 100.0f;
    }
    public void updatePosition(float dt=0.02f)
    {
        this.position.x += (this.velocity.x * dt);
        this.position.y += (this.velocity.y * dt);
        this.position.z += (this.velocity.z * dt);

        this.position = new Vector3(
            Mathf.FloorToInt(this.position.x * 100.0f) / 100.0f,
            Mathf.FloorToInt(this.position.y * 100.0f) / 100.0f,
            Mathf.FloorToInt(this.position.z * 100.0f) / 100.0f
        );
        gameObject.transform.position = position;

    }
}
