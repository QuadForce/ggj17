using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour {
    public float speed = 4f;
    bool right = true;
    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        

        var rotationVector = transform.rotation.eulerAngles;

        if (right)
        {
            rotationVector.z += speed;
            
            transform.rotation = Quaternion.Euler(rotationVector);
            if (rotationVector.z > 125)
            {
                right = false;
            }
        }

        if (!right)
        {
            rotationVector.z -= speed;
            transform.rotation = Quaternion.Euler(rotationVector);
            if (rotationVector.z < 50)
            {
                right = true;
            }
        }

    }
}
    
