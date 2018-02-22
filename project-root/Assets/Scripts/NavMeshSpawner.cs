using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshSpawner : MonoBehaviour
{



    public GameObject monsterToSpawn;
    public List<GameObject> monsters = new List<GameObject>();
    [Range(0, 10)] public int amountToSpawn;
    public int respawnTime;
    [Range(0, 20)] public float range = 10f;
    private Vector3 pointOnNavMesh;
    private bool doneSpawning = false;
    private List<GameObject> spawnedMonsters = new List<GameObject>();
    private int spawned;


    public void Update()
    {
        if (spawnedMonsters.Count == 0 && doneSpawning == true)
        {
            spawned = 0;
            StartCoroutine("Respawn");
        }

        for (var i = spawnedMonsters.Count - 1; i > -1; i--)
        {
            if (spawnedMonsters[i] == null)
                spawnedMonsters.RemoveAt(i);
        }

        if (doneSpawning == false)
        {
            StartCoroutine("Spawn");
        }


    }


    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {

        for (int i = 0; i < 10; i++)
        {

            var randomPoint = center + Random.insideUnitSphere * range;
            var hit = new NavMeshHit();
            if (NavMesh.SamplePosition(randomPoint, out hit, 1, NavMesh.AllAreas))
            {

                result = hit.position;

                return true;

            }
        }
        result = Vector3.zero;
        return false;
    }


    IEnumerator Spawn()
    {
        foreach (var enemy in monsters)
        {
            if (RandomPoint(transform.position, range, out pointOnNavMesh) && amountToSpawn > spawned)
            {
                spawnedMonsters.Add(Instantiate(enemy, pointOnNavMesh, Quaternion.identity));
                spawned++;
                if (spawned == amountToSpawn)
                {
                    doneSpawning = true;
                }
            }
        }

        yield return null;
    }
    IEnumerator Respawn()
    {

        yield return new WaitForSeconds(respawnTime);

        doneSpawning = false;


    }

    public void MakeWolf()
    {
        Instantiate(monsterToSpawn, pointOnNavMesh, Quaternion.identity);
    }

}
