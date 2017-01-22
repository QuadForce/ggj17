using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScene : MonoBehaviour {

    // Use this for initialization
    void Start () {
        
    }

    // Update is called once per frame


    bool right = true;
    
    void Update()
    {
        var rotationVector = transform.rotation.eulerAngles;
       
        if (right)
        {
            rotationVector.z += 2;
            transform.rotation = Quaternion.Euler(rotationVector);
            if(rotationVector.z > 110)
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

        if(Input.anyKey == true)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenuScene");
        }

        if (Time.time > 5)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenuScene");
        }

    }
}
