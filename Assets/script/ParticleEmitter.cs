using UnityEngine;
//using System;
using System.Collections.Generic;
using System.Linq;

public class ParticleEmitter : MonoBehaviour
{

    public GameObject particlePrefab;
    public float voxelSize = 1.0f;
    public int particleCount = 5000;
    public Vector3 emitterPosition = new (0,0,0);
    public Vector3 emitterPosition1 = new (0,0,0);
    public Vector3 emitterPosition2 = new (0,0,0);
    public Vector3 emitterPosition3 = new (0,0,0);
        public Vector3 emitterSize = new (10,10,1);
    public HashSet<CustomParticle> particles = new(); // put set outside
    public Dictionary<Vector3Int, List<CustomParticle>> voxelGrid = new();

    //todo: make a wall with multiple emitters
    //todo: size is not changing
    //todo: change particles color

    void Start()
    {
        InitializeParticles(); 
    }

    void Update(){
        SpawnParticle(emitterPosition);
        SpawnParticle(emitterPosition1);
        SpawnParticle(emitterPosition2);
        SpawnParticle(emitterPosition3);
        
        UpdateParticles(); //edit hash
        CheckCollisions(); //check hash for collision
    }

    void InitializeParticles()
    {
        foreach (CustomParticle particle in particles)
        {
            AddParticleToHash(particle);
        }
    }

    void UpdateParticles()
    {
        voxelGrid.Clear();
        foreach (CustomParticle particle in particles)
        {
            if (!particle.IsDead()) AddParticleToHash(particle);
        }
    }

    void AddParticleToHash(CustomParticle particle)
    {
        Vector3Int voxel = GetVoxelCoordinate(particle.transform.position);
        if (!voxelGrid.ContainsKey(voxel))
        {
            voxelGrid[voxel] = new List<CustomParticle>();
        }
        voxelGrid[voxel].Add(particle);
    }

    Vector3Int GetVoxelCoordinate(Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x / voxelSize);
        int y = Mathf.FloorToInt(position.y / voxelSize);
        int z = Mathf.FloorToInt(position.z / voxelSize);
        return new Vector3Int(x, y, z);
    }

    void CheckCollisions()
    {
        foreach (CustomParticle particle in particles)
        {
            List<CustomParticle> inVoxel = voxelGrid[GetVoxelCoordinate(particle.transform.position)]; // in the same voxel
            List<CustomParticle> nearby = GetNearbyParticles(GetVoxelCoordinate(particle.transform.position));
            // union them
            foreach (CustomParticle other in inVoxel.Union(nearby))
            {
                if (other != particle && IsColliding(particle, other))
                {
                    Debug.Log($"{particle.name} is colliding with {other.name}");
                }
            }
        }
    }

    List<CustomParticle> GetNearbyParticles(Vector3Int voxel)
    {
        List<CustomParticle> nearbyParticles = new List<CustomParticle>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    Vector3Int neighborVoxel = voxel + new Vector3Int(x, y, z);
                    if (voxelGrid.ContainsKey(neighborVoxel))
                    {
                        nearbyParticles.AddRange(voxelGrid[neighborVoxel]);
                    }
                }
            }
        }

        return nearbyParticles;
    }

    bool IsColliding(CustomParticle p1, CustomParticle p2)
    {
        float distance = Vector3.Distance(p1.transform.position, p2.transform.position);
        float collisionDistance = 0.1f;
        return distance < collisionDistance;
    }

    void SpawnParticle(Vector3 emitterPos)
    {
        Vector3 position = emitterPos + new Vector3(
            Random.Range(-emitterSize.x /5, emitterSize.x/5),
            Random.Range(-emitterSize.y /5, emitterSize.y/5),
            Random.Range(-emitterSize.z /5, emitterSize.z/5)
        );

        GameObject particle = Instantiate(particlePrefab, position, Quaternion.identity);
        //CustomParticle customParticle = particle.GetComponent<CustomParticle>();
        //customParticle.velocity = initialVelocity;

        particles.Add(particle.GetComponent<CustomParticle>());

        //delete dead particles(accumulate then delete, to not disturb the iteration)
        List<CustomParticle> particlesToRemove = new();

        foreach(CustomParticle p in particles){
            if(p.IsDead()){
                particlesToRemove.Add(p);
            }
        }

        foreach(CustomParticle p in particlesToRemove){
            particles.Remove(p);
        }
    }

    
}
