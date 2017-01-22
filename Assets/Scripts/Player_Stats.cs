using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player_Stats : NetworkBehaviour {

    [SyncVar] public Color c;
	// Use this for initialization
	void Start () {
        if (!isServer)
        {
            return;
        }
		
	}

    void changeColor()
    {

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
