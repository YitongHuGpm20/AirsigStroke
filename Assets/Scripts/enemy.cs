using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemy : MonoBehaviour
{
    
    public int eHealth = 30;
    public AudioClip dieDuck;
    private AudioSource sourceDuck;
    public Transform player;
    public Image healthbar;
    // Start is called before the first frame update
    void Start()
    {
        sourceDuck = GetComponent<AudioSource>();
        if(GameObject.FindGameObjectWithTag("lefthand") != null)
        {
            player = GameObject.FindGameObjectWithTag("lefthand").GetComponent<Transform>();
        }
        
        healthbar.fillAmount = eHealth / 30.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(eHealth == 0)
        {
            this.gameObject.GetComponent<MeshRenderer>().enabled = false;
            this.gameObject.GetComponent<Collider>().enabled = false;
            Destroy(this.gameObject, 1.5f);
        }

        transform.LookAt(player);
        healthbar.fillAmount = eHealth / 30.0f;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "duck")
        {
            sourceDuck.PlayOneShot(dieDuck);
            Destroy(other.gameObject);
            eHealth -= 10;
        }
    }
}
