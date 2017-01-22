using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathFloor : MonoBehaviour {

    public Camera playerCamera; 
    private float spawnChoice; 

    void Start()
    {

    }

    void OnTriggerEnter(Collider other)
    {
   
        if (other.gameObject.CompareTag("Death_floor"))
        {
            gameObject.transform.position = new Vector3(11.86f, 60f, 22.0f);
        }
    }




}
