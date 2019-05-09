using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    public static float beat = 1.5f;
    public float timer;
    public int count;
    public GameObject bullet;
    public Transform[] bulletSpawn;
    public AudioClip spawnBullet;
    private AudioSource sourceBullet;
    // Start is called before the first frame update
    void Start()
    {
        sourceBullet = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > beat)
        {
            /*GameObject cube = Instantiate(cubes[Random.Range(0, 2)], points[0]);
            cube.transform.localPosition = Vector3.zero;
            timer -= beat;*/
            if ((count % 8) < 4)
            {
                Instantiate(bullet, bulletSpawn[Random.Range(0, 3)]);
                bullet.transform.localPosition = Vector3.zero;
                sourceBullet.PlayOneShot(spawnBullet);
            }


            timer -= beat;
            count++;
        }
        else
            timer += Time.deltaTime;
    }
}
