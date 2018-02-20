using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshSpawner : MonoBehaviour
{
    private Vector3 pointOnNavMesh;
    public GameObject monsterToSpawn;
    public float countDown;  // add GameObject.Find("Spawner").GetComponent<NavMeshSpawner>().countDown = 0; on enemy script when destroyed
    public bool doneSpawning;
    public List<GameObject> monsters = new List<GameObject>();
    public int spawned; // add GameObject.Find("Spawner").GetComponent<NavMeshSpawner>().spawned--; on enemy script when destroyed
    public int amountToSpawn;
    public int reSpawnTime;
    public float range = 10f;

    public void Update()
    {
        countDown += Time.deltaTime;

        if (doneSpawning == false)
        {
            Invoke("Spawn", 0.1f);
        }
        else
        {
            
            Invoke("Spawn", reSpawnTime);
        }
    }


    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        
        for (int i = 0; i < 10; i++)
        {
            
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1 , NavMesh.AllAreas))
            {
               
                result = hit.position;
                
                return true;
               
            }
        }
        result = Vector3.zero;
        return false;
    }


    public void Spawn()
    {
        
        
        foreach (GameObject go in monsters)
            if (RandomPoint(transform.position, range, out pointOnNavMesh ) && amountToSpawn > spawned)
        {
                spawned++;
                Instantiate(go, pointOnNavMesh, Quaternion.identity);
                
                if (spawned == amountToSpawn)
            {
                doneSpawning = true;
                    
                }
            else
                {
                    doneSpawning = false;
                }
           
           
        }
        
    }
   
    public void MakeWolf()
    {
        Instantiate(monsterToSpawn, pointOnNavMesh, Quaternion.identity);
    }

}
