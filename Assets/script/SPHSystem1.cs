using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPHSystem1 : MonoBehaviour
{
    public List<GameObject> gameObjectsList;
    public int totalNumberOfParticles;
    public float width;
    public float height;
    public float depth;
    private float volume;
    public double particleMass = 0.001;
    public float particleRadius = 0.1f;
    public float smoothingRadius;
    private float smoothingRadiusSquare;
    public double dynamicViscosity;
    public float isotropicExponent;
    public float baseDensity;
    private double normalizationDensity;
    private float normalizationPressureForce;
    private float normalizationViscousForce;
    private Vector3 gravityForce;

    public bool startWorking = false;
    void Start()
    {
        this.gameObjectsList = new List<GameObject>();
        this.normalizationDensity = (315 * this.particleMass) / (64 * Mathf.PI * Mathf.Pow(this.smoothingRadius, 9));
        this.normalizationPressureForce = (float)(-1 * (45 * this.particleMass) / (Mathf.PI * Mathf.Pow(this.smoothingRadius, 6)));
        this.normalizationViscousForce = (float)(45 * this.dynamicViscosity * this.particleMass) / (Mathf.PI * Mathf.Pow(this.smoothingRadius, 6));
        smoothingRadiusSquare = Mathf.Pow(smoothingRadius, 2.0f);
    }

    void Update()
    {
        if (startWorking)
        {
            addObjects();
            updateDistacesListAndDensitiesAndPressure();
            calculateForce();
            movesParticles();
            removeObjects();
        }
    }

    private void addObjects()
    {
        gameObjectsList = new List<GameObject>();
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.CompareTag("Particle"))
            {
                //if (!PosInRange(obj.transform.position))
                //{
                //    Destroy(obj);
                //    totalNumberOfParticles--;
                //    continue;
                //}
                gameObjectsList.Add(obj);
            }
            
            

        }
    }
    private bool PosInRange(Vector3 position)
    {
        if (position.y < height && position.y > 0)
        {
            if (position.z < depth && position.z > 0)
            {
                if (position.x < (gameObject.transform.position.x - width / 2) + width && position.x > (gameObject.transform.position.x - width / 2))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void updateDistacesListAndDensitiesAndPressure()
    {
        Particle particle1 = null;
        Particle particle2 = null;
        GameObject obj1, obj2;


        for (int i = 0; i < gameObjectsList.Count; i++)
        {
            obj1 = gameObjectsList[i];
            if (obj1)
            {
                if (obj1.CompareTag("Particle") == true)
                {
                    particle1 = obj1.GetComponent<Particle>();
                }
            }
            else
            {
                continue;
            }
            for (int j = i + 1; j < gameObjectsList.Count; j++)
            {
                obj2 = gameObjectsList[j];
                if (obj2)
                {
                    if (obj2.CompareTag("Particle") == true)
                    {
                        particle2 = obj2.GetComponent<Particle>();
                        if (particle1 == particle2)
                        {
                            continue;
                        }
                        float distance = Mathf.Sqrt(
                            Mathf.Pow(particle2.position.x - particle1.position.x, 2.0f)
                            + Mathf.Pow(particle2.position.y - particle1.position.y, 2.0f)
                            + Mathf.Pow(particle2.position.z - particle1.position.z, 2.0f)
                        );

                        if (distance <= smoothingRadius)
                        {
                            particle1.addParticleDistance(particle2, distance);
                            particle2.addParticleDistance(particle1, distance);
                            float currentDensity = (float)(this.normalizationDensity * Mathf.Pow(smoothingRadiusSquare - Mathf.Pow(distance, 2.0f), 3));
                            particle1.density += currentDensity;
                            particle2.density += currentDensity;
                        }
                    }
                }
            }
            particle1.pressure = isotropicExponent * (particle1.density - baseDensity);
        }
    }
    public void calculateForce()
    {

        if (gameObjectsList.Count <= 1)
        {
            return;
        }
        foreach (GameObject obj in gameObjectsList)
        {
            Particle particle = obj.GetComponent<Particle>();
            foreach (ParticleDistancePair pr in particle.particleDistancePairs)
            {
                float distance = pr.distance;
                Particle neighbor = pr.particle;

                // float kernel = (315.0f / (64.0f * Mathf.PI) * Mathf.Pow(smoothingRadius, 2.0f) * Mathf.Pow(distance, 2.0f) - Mathf.Pow(distance, 4.0f)) / Mathf.Pow(smoothingRadius, 3.0f);

                // particle.force +=  (float) (-1 * particle.mass) * (particle.pressure + neighbor.pressure) / 2.0f * kernel * (neighbor.position - particle.position) / Mathf.Pow(distance, 2.0f);
                // particle.force += (float)(particle.mass * dynamicViscosity) * kernel * (neighbor.velocity - particle.velocity) / (particle.density + neighbor.density);

                Vector3 pressureForce = normalizationPressureForce * (
                -(neighbor.position - particle.position) / distance
                * (neighbor.pressure + particle.pressure)
                / (2.0f * neighbor.density) * Mathf.Pow((smoothingRadius - distance), 2)
                );
                Vector3 viscosityForce = normalizationViscousForce * (
                (neighbor.velocity - particle.velocity) / neighbor.density * (smoothingRadius - distance)
                );
                particle.force += pressureForce;
                particle.force += viscosityForce;
            }
            gravityForce = new Vector3(0, (float)(-9.8 * particle.density), 0);
            particle.force += gravityForce;
            // string s = "(" + particle.position.x + ", " + particle.position.y + ", "+ particle.position.z + ")" 
            //     + "\n" + "pressure = " + particle.pressure
            //     + "\n" + "density = " + particle.density
            //     + "\n" + "(" + particle.velocity.x + ", " + particle.velocity.y + ", "+ particle.velocity.z + ")"
            //     + "\n" + "(" + particle.force.x + ", " + particle.force.y + ", "+ particle.force.z + ")";
            // Debug.Log (s);
            particle.updateVelocity(particle.force);
        }
    }
    public void movesParticles()
    {
        foreach (GameObject obj in gameObjectsList)
        {
            Particle particle = obj.GetComponent<Particle>();
            particle.updatePosition();
        }

    }
    public void removeObjects()
    {
        foreach (GameObject obj in gameObjectsList)
        {
            Particle particle = obj.GetComponent<Particle>();
            particle.density = 0.0004f;
            particle.pressure = 0;
            particle.force = new Vector3(0, 0, 0);
            particle.particleDistancePairs = new List<ParticleDistancePair>();
        }
        gameObjectsList = new List<GameObject>();
    }
}
