using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathFloor : MonoBehaviour {

    public Camera playerCamera;

    void Start()
    {

    }

    void OnTriggerEnter(Collider other)
    {
   
        if (other.gameObject.CompareTag("Death_floor"))
        {
            gameObject.transform.position = new Vector3(0, 63, 2);
            gameObject.transform.rotation = new Quaternion(90, 0, 0, 0);
            Debug.Log("Touching death floor"); 
        }
    }




}
