using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;

[NetworkSettings (channel = 0, sendInterval = 0.1f)]
public class Player_SyncPosition : NetworkBehaviour {

	[SyncVar (hook = "SyncPositionValues")]
	private Vector3 syncPos;

	[SerializeField] Transform myTransform;
	private float lerpRate;
	private float normalLerpRate = 16;
	private float fasterLerpRate = 27;

	private Vector3 lastPos;
	private float threshold = 0.5f;

	private NetworkClient nClient;
	private int latency;
	private Text latencyText;

	private List<Vector3> syncPosList = new List<Vector3>();
	[SerializeField] private bool useHistoricalLerping = false;
	private float closeEnough = 0.11f;

	void Start ()
	{
		nClient = GameObject.Find("NetworkManager").GetComponent<NetworkManager>().client;
		latencyText = GameObject.Find("Latency Text").GetComponent<Text>();
		lerpRate = normalLerpRate;
	}

	void Update ()
	{
		LerpPosition();
		ShowLatency();
	}

	void FixedUpdate () 
	{
		TransmitPosition();

	}

	void LerpPosition ()
	{
		if(!isLocalPlayer)
		{
			if(useHistoricalLerping)
			{
				HistoricalLerping();
			}
			else
			{
				OrdinaryLerping();
			}

			//Debug.Log(Time.deltaTime.ToString());
		}
	}

	[Command]
	void CmdProvidePositionToServer (Vector3 pos)
	{
		syncPos = pos;
		//Debug.Log("Command called");
	}

	[ClientCallback]
	void TransmitPosition ()
	{
		if(isLocalPlayer && Vector3.Distance(myTransform.position, lastPos) > threshold)
		{
			CmdProvidePositionToServer(myTransform.position);
			lastPos = myTransform.position;
		}
	}

	[Client]
	void SyncPositionValues (Vector3 latestPos)
	{
		syncPos = latestPos;
		syncPosList.Add(syncPos);
	}

	void ShowLatency ()
	{
		if(isLocalPlayer)
		{
			latency = nClient.GetRTT();
			latencyText.text = latency.ToString();
		}
	}

	void OrdinaryLerping ()
	{
		myTransform.position = Vector3.Lerp(myTransform.position, syncPos, Time.deltaTime * lerpRate);
	}

	void HistoricalLerping ()
	{
		if(syncPosList.Count > 0)
		{
			myTransform.position = Vector3.Lerp(myTransform.position, syncPosList[0], Time.deltaTime * lerpRate);

			if(Vector3.Distance(myTransform.position, syncPosList[0]) < closeEnough)
			{
				syncPosList.RemoveAt(0);
			}

			if(syncPosList.Count > 10)
			{
				lerpRate = fasterLerpRate;
			}
			else
			{
				lerpRate = normalLerpRate;
			}

			//Debug.Log(syncPosList.Count.ToString());
		}
	}
}
