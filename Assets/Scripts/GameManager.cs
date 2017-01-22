using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour {

    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += onSceneLoaded;
    }

    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= onSceneLoaded;
    }

    // Use this for initialization
    void onSceneLoaded (Scene scene, LoadSceneMode mode) {
        Debug.Log("Level Loaded");
        Debug.Log(scene.name);
        Debug.Log(mode);
        StartCoroutine(sceneLoadCoroutine());
    }


    IEnumerator sceneLoadCoroutine()
    {
        //yield return new WaitForSeconds(5);
        LobbyPlayer[] listOfPlayers = GameObject.Find("LobbyManager").GetComponentsInChildren<LobbyPlayer>();
        LobbyPlayer player = listOfPlayers[0].GetComponent<LobbyPlayer>();
        FirstPersonController fpc = player.GetComponentInParent<NetworkIdentity>().GetComponent<FirstPersonController>();
        Debug.Log(player.GetComponentInParent<NetworkIdentity>().gameObject);
        Debug.Log(player.GetComponentInParent<NetworkIdentity>().observers);
        if (player.playerColor == Color.blue)
        {
            //blue person
            fpc.playerID = 1;
            Debug.Log("set blue person sound and color");
        }
        else if (player.playerColor == Color.green)
        {
            //green person
            fpc.playerID = 2;
        }
        else if (player.playerColor == Color.yellow)
        {
            //yellow person
            fpc.playerID = 3;
        }
        else if (player.playerColor == Color.red)
        {
            //red person
            fpc.playerID = 4;
        }
        yield return null;
    }
}
