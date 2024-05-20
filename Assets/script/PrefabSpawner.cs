using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PrefabSpawner : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public float spawnDelay = 1.0f;
    public float distroy =5.0f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnPrefab());
    }

    private IEnumerator SpawnPrefab()
    {
        while (true)
        {
            //Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
             GameObject spawnedObject = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
            Destroy(spawnedObject,distroy); // Destroy the prefab after 10 seconds
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}

