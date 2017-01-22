using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour {
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
            rotationVector.z += 2;
            
            transform.rotation = Quaternion.Euler(rotationVector);
            if (rotationVector.z > 110)
            {
                right = false;
            }
        }

        if (!right)
        {
            rotationVector.z -= 2;
            transform.rotation = Quaternion.Euler(rotationVector);
            if (rotationVector.z < 65)
            {
                right = true;
            }
        }

    }
}
    
