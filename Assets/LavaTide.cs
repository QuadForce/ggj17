using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaTide : MonoBehaviour {

    private bool up = true; 

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (up)
        {
            gameObject.transform.position += new Vector3(0, 0.01f, 0);
            if (transform.position.y > -0.5)
            {
                up = false; 
            }
        } else
        {
            gameObject.transform.position -= new Vector3(0, 0.01f, 0);
            if (transform.position.y < -10.5)
            {
                up = true; 
            }
        }
        Debug.Log(transform.position.y);
    }
}
