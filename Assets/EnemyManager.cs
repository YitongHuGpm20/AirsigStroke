using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //List of enemies editable in inspector cant access in another script
    [SerializeField] private List<GameObject> enemies;
    public GameObject prefab;
    public int numOfStartingEnemies;
    public float spawnDelay;
    [Tooltip("Max number of enemies at one time")] public int maxGroupSize;
    public int maxNumberOfEnemies;

    private int numOfSpawned;
    private SphereCollider spawnArea;
    const float enemyBodyRadius = .5f; //Magic number 
    // The max and min height of a enemy position
    public float groundHeight = 1.06f;
    public float maxHeight = 1.06f;
    private void Awake()
    {
        spawnArea = GetComponent<SphereCollider>();
        enemies = new List<GameObject>();
        numOfSpawned = 0;
        for (int i = 0; i < numOfStartingEnemies; i++)
        {
            spawn();
        }
        StartCoroutine(autoSpawn());
    }

    IEnumerator autoSpawn()
    {
        while (numOfSpawned<maxNumberOfEnemies)
        {
            yield return new WaitForSeconds(spawnDelay);

            //remove all destroyed enemies
            enemies.RemoveAll(enemy => enemy == null);
            if (enemies.Count < maxGroupSize)
            {
                spawn();
            }
        }

    }

    bool isValidPosition(Vector3 pos)
    {
        return enemies.TrueForAll(enemy => Vector3.Distance(enemy.transform.position, pos) > (enemyBodyRadius * 2));
    }

    void spawn()
    {
        //Pick a random spot inside our sphere collider
        Vector3 pos = transform.position + Random.insideUnitSphere * spawnArea.radius;
        pos.y = Mathf.Clamp(pos.y, groundHeight, maxHeight);

        //Lets avoid overlaps or attempt to atleast 5 times
        for(int attempts = 0; attempts < 5; attempts++)
        {
            if (isValidPosition(pos))
            {
                break;
            }
            pos = transform.position + Random.insideUnitSphere * spawnArea.radius;
            pos.y = Mathf.Clamp(pos.y, groundHeight, maxHeight);
        }

        //the Quarternion.identity is the rotation 0,0,0
        GameObject enemy = Instantiate(prefab, pos, Quaternion.identity);
        enemy.transform.rotation = Quaternion.EulerAngles(0, 0, 0);
        enemies.Add(enemy);
        numOfSpawned++;
    }
}
