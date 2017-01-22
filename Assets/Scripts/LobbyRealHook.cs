using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prototype.NetworkLobby;
using UnityEngine.Networking;
using UnityStandardAssets.Characters.FirstPerson;

public class LobbyRealHook : LobbyHook{

	public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer) {
        Debug.Log(lobbyPlayer);
        Debug.Log(gamePlayer);
        LobbyPlayer lp = lobbyPlayer.GetComponent<LobbyPlayer>();
        FirstPersonController fpc = gamePlayer.GetComponent<FirstPersonController>();

        if (lp.playerColor == Color.blue)
        {
            //blue person
            fpc.playerID = 1;
            Debug.Log("set blue person sound and color");
        }
        else if (lp.playerColor == Color.green)
        {
            //green person
            fpc.playerID = 2;
            Debug.Log("set green person sound and color");
        }
        else if (lp.playerColor == Color.yellow)
        {
            //yellow person
            fpc.playerID = 3;
            Debug.Log("set yellow person sound and color");
        }
        else if (lp.playerColor == Color.red)
        {
            //red person
            fpc.playerID = 4;
            Debug.Log("set red person sound and color");
        }
    }
}
