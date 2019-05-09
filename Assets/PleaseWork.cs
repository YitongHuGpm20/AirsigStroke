using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AirSig;

public class PleaseWork : MonoBehaviour
{
    public AirSigManager airSigManager;

    private void Awake()
    {
        airSigManager.SetMode(AirSigManager.Mode.DeveloperDefined);
    }


    // Start is called before the first frame update
    void Start()
    {
         Debug.Log("I AM IN DEVELOPER DEFINED MODE:" + AirSigManager.Mode.DeveloperDefined);   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
