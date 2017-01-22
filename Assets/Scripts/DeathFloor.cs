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
            spawnChoice = Random.Range(1, 5);
            if (spawnChoice == 1)
            {
                gameObject.transform.position = new Vector3(61.2f, 10f, 67.9f);
            }
            if (spawnChoice == 2)
            {
                gameObject.transform.position = new Vector3(-33.4f, 10f, 65.0f);
            }
            if (spawnChoice == 3)
            {
                gameObject.transform.position = new Vector3(-21.8f, 10f, -21.8f);
            }
            if (spawnChoice == 4)
            {
                gameObject.transform.position = new Vector3(56.1f, 10f, -23.6f);
            };
            gameObject.transform.rotation = new Quaternion(90, 0, 0, 0);
        }
    }




}
