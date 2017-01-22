using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathFloor : MonoBehaviour {

    public GameObject particles;
    private float spawnChoice; 

    void Start()
    {

    }

    void OnTriggerEnter(Collider other)
    {
   
        if (other.gameObject.CompareTag("Death_floor"))
        {
            Vector3 particleLocation = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 1.5f, gameObject.transform.position.z);
            GameObject pEffect = GameObject.Instantiate(particles, particleLocation, new Quaternion(0,0,0,0));
            gameObject.transform.position = new Vector3(11.86f, 60f, 22.0f);
            Destroy(pEffect, 1.8f);
        }
    }




}
