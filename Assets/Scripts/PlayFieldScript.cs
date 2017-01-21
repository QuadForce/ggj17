using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayFieldScript : MonoBehaviour {

    private float shrinkSpeed = 0.002f;
    private int minScale = 1; 
	
	void Update () {
        if(transform.localScale.x >= minScale) {
            transform.localScale -= new Vector3(shrinkSpeed, 0, shrinkSpeed);
        }
    }
}
