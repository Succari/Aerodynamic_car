using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quicker : MonoBehaviour
{
    public GameObject SPHWorld;
    // public GameObject worldSystem;
    // public GameObject treeWorld;
    // public GameObject treeWorldShader;
    public GameObject particlePrefab;
    public float cubeWidth, cubeHeight, cubeDepth, farBetweenParticles;
    public Vector3 initailVelocity = new Vector3(0, 0, 0);
    // public bool isHardUpdated = false;
    public bool startAdding = false;
    private void cubeInstantiate()
    {
        // double mass = treeWorld.GetComponent<TreeWorld>().particleMass;
        // float radius = treeWorld.GetComponent<TreeWorld>().particleRadius;

        // double mass = treeWorldShader.GetComponent<TreeWorldShader>().particleMass;
        // float radius = treeWorldShader.GetComponent<TreeWorldShader>().particleRadius;

        double mass = SPHWorld.GetComponent<SPHSystem1>().particleMass;
        float radius = SPHWorld.GetComponent<SPHSystem1>().particleRadius;

        // double mass = worldSystem.GetComponent<WorldSystem>().particleMass;
        // float radius = worldSystem.GetComponent<WorldSystem>().particleRadius;

        float startX = gameObject.transform.position.x - cubeWidth / 2;
        float startY = gameObject.transform.position.y - cubeHeight / 2;
        float startZ = gameObject.transform.position.z - cubeDepth / 2;

        float endX = gameObject.transform.position.x + cubeWidth / 2;
        float endY = gameObject.transform.position.y + cubeHeight / 2;
        float endZ = gameObject.transform.position.z + cubeDepth / 2;

        Vector3 current;
        for (float k = startZ; k <= endZ; k += farBetweenParticles)
        {
            for (float j = startY; j <= endY; j += farBetweenParticles)
            {
                for (float i = startX; i <= endX; i += farBetweenParticles)
                {
                    current = new Vector3(i, j, k);
                    float x = initailVelocity.x + Random.Range(-1.0f, 1.0f);
                    float y = initailVelocity.y + Random.Range(-1.0f, 1.0f);
                    float z = initailVelocity.z + Random.Range(-1.0f, 1.0f);
                    Vector3 vel = new Vector3(x, y, z);
                    GameObject particleInstantiate = Instantiate(particlePrefab, current, gameObject.transform.rotation);
                    particleInstantiate.transform.localScale = new Vector3((float)radius, (float)radius, (float)radius);
                    particleInstantiate.AddComponent<Particle>().setInfo(mass, radius, current, vel);
                    // treeWorldShader.GetComponent<TreeWorldShader>().totalNumberOfParticles++;
                    // treeWorld.GetComponent<TreeWorld>().totalNumberOfParticles++;
                    // worldSystem.GetComponent<WorldSystem>().totalNumberOfParticles++;
                    SPHWorld.GetComponent<SPHSystem1>().totalNumberOfParticles++;
                }
            }
        }
    }
    void Start()
    {
        cubeInstantiate();
    }
    void Update()
    {
        if (startAdding)
        {
            cubeInstantiate();
            startAdding = false;
        }
        // worldSystem.GetComponent<WorldSystem>().isHardUpdated = isHardUpdated;
    }
}
