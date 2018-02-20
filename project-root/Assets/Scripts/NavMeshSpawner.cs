using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshSpawner : MonoBehaviour
{
    public GameObject monsterSpawn;
    Vector3 point;
    public float time;
    public bool done;
    public List<GameObject> monsters = new List<GameObject>();
    //public int enemyIndex;
    public int spawned;
    public int amountToSpawn;
    public int reSpawnTime;

    public float range = 10f;

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


     void Spawn()
    {
        
        Vector3 point;
        foreach (GameObject go in monsters)
            if (RandomPoint(transform.position, range, out point ) && amountToSpawn > spawned)
        {
                spawned++;
                Instantiate(go, point, Quaternion.identity);
                
                if (spawned == amountToSpawn)
            {
                done = true;
                    
                }
            else
                {
                    done = false;
                }
           
           
        }
        
    }
    public void Update()
    {
        time += Time.deltaTime;

        if (done == false)
        {
            Invoke("Spawn", 0.1f);
        }
       if (done == true && time >= reSpawnTime)
        {

            Invoke("Spawn", 0.1f);
        }
    }
    public void MakeWolf()
    {
        Instantiate(monsterSpawn, point, Quaternion.identity);
    }

}
