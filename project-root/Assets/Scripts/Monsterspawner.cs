using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monsterspawner : MonoBehaviour
{


    public List<Transform> spawnPoints = new List<Transform>();
    private bool done;


    private Vector3 points;
    public bool zombieSpawn;
    public bool wolfSpawn;
    public  GameObject wolf;
    public GameObject zombie;
    public int amountToSpawn;
    public float timeToSpawn;
    public float time; //GameObject.Find("monsterspawner").GetComponent<Monsterspawner>().time = 0;   on enemy destroy
    public int enemyCount;  //GameObject.Find("monsterspawner").GetComponent<Monsterspawner>().enemyCount--;

    public void Start()
    {
         points = spawnPoints[Random.Range(0, spawnPoints.Count)].position;
        
    }

	public void Update ()
     {
        if (enemyCount == amountToSpawn)
        {
            done = true;
        }
        
        if (wolfSpawn == true && enemyCount < amountToSpawn && done == false )
        {
            
            
                Instantiate(wolf, points, Quaternion.identity);
                InvokeRepeating("Spawn", 0.1f, 0.1f);

                enemyCount++;
            
        }
        if (zombieSpawn == true && enemyCount < amountToSpawn && done == false )
         {

            Instantiate(zombie, transform.position, Quaternion.identity);
            InvokeRepeating("Spawn", 0.1f, 0.1f);
          
            enemyCount++;
        }
        
        
        
    }
    public void Spawn()
    {
       
        time += Time.deltaTime;
        if ( wolfSpawn == true && enemyCount < amountToSpawn && time >= timeToSpawn)
        {
            
            Instantiate(wolf, points, Quaternion.identity);
            enemyCount++;
            time = 0;
            
        }
        if ( zombieSpawn == true && enemyCount < amountToSpawn && time >= timeToSpawn)
         {
            Instantiate(zombie, transform.position, Quaternion.identity);
            enemyCount++;
            time = 0;
         }

    }
    public void MakeWolf()
    {
        Instantiate(wolf, points, Quaternion.identity);
    }
   
}
