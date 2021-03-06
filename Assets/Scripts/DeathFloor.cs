﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Characters.FirstPerson;

public class DeathFloor : NetworkBehaviour {

    public GameObject particles;
    public GameObject deathVision;
    public int[] Deathtracker;
    private int playerID; 

    void Start()
    {
        Deathtracker = new int[4] { 0, 0, 0, 0 };
    }

    void OnTriggerEnter(Collider other)
    {
   
        if (other.gameObject.CompareTag("Death_floor"))
        {
            // Keep track of player death 
            FirstPersonController fpc = GetComponent<FirstPersonController>();
            playerID = fpc.playerID - 1; 
            Deathtracker[playerID] += 1; 
            if (Deathtracker[playerID] >= 2)
            {
                fpc.isDisabled = true;
               // gameObject.GetComponent<FirstPersonController>().enabled = false;
            }

            // Death effects 
            CmdSpawnParticles();
            gameObject.transform.position = new Vector3(11.86f, 60f, 22.0f);
            GameObject pEffect = GameObject.Instantiate(deathVision, new Vector3(11.86f, 55f, 22.0f), gameObject.transform.rotation);
            Destroy(pEffect, 3f);
			fpc.PlayCustomAudio("death");
        }
    }

    [Command]
    void CmdSpawnParticles()
    {
        Vector3 particleLocation = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 1.5f, gameObject.transform.position.z);
        var pEffect = (GameObject) Instantiate(particles, particleLocation, new Quaternion(0, 0, 0, 0));
        NetworkServer.Spawn(pEffect);
        Destroy(pEffect, 1.8f);
    }




}
