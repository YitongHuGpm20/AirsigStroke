using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBullet : MonoBehaviour
{
    public Transform target;
    public float speed = 30f;
    public DeveloperDefined developer;
    public GameObject gameManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        target = GameObject.Find("Controller (left)").transform;
        gameManager = GameObject.Find("GameManager");
        developer = gameManager.GetComponent<DeveloperDefined>();
        float step = speed * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(collision.gameObject.tag == "lefthand")
    //    {
    //        developer.pHealth -= 10;
    //        Destroy(this.gameObject);
    //    }

    //    if(collision.gameObject.tag == "shield")
    //        Destroy(this.gameObject);
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "lefthand")
        {
            developer.pHealth -= 10;
            Destroy(this.gameObject);
        }

        if (other.gameObject.tag == "shield")
            Destroy(this.gameObject);
    }
}
