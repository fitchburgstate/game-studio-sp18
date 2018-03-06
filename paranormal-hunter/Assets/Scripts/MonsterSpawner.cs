using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class MonsterSpawner : MonoBehaviour
{
    
    [Tooltip("A list of monster prefabs")]
    public List<GameObject> monsters = new List<GameObject>();
    [Tooltip("How many monsters you want to spawn")]
    [Range(0, 10)] public int amountToSpawn;
    [Tooltip("Area range you want the random spawning in")]
    [Range(0, 20)] public float range = 10f;
    [Tooltip("Time it takes for them to respawn after they are killed")]
    public int respawnTime;
    [Tooltip("Monster you want the 'Spawn Monster' button to spawn")]
    public GameObject monsterToSpawn;

    private Vector3 pointOnNavMesh;
    private bool doneSpawning = false;
    private List<GameObject> spawnedMonsters = new List<GameObject>();
    private int spawned;
    private GameObject room;

    public void Update()
    {
        if (spawnedMonsters.Count == 0 && doneSpawning == true)
        {
            spawned = 0;
            StartCoroutine("Respawn");
            StartCoroutine("TurnLightsOn");
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
                if (amountToSpawn == 1)
                {
                    spawnedMonsters.Add(Instantiate(enemy,transform.position, Quaternion.identity));
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
        Instantiate(monsterToSpawn, transform.position, Quaternion.identity);
    }
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, range);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.5f);
    }
    IEnumerable TurnLightsOn()
    {
        room.SetActive(true);
        yield return true;
    }
}
