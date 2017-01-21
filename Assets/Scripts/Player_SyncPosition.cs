using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_SyncPosition : NetworkBehaviour {

	[SyncVar]
	private Vector3 syncPos;
    [SyncVar]
    private Quaternion syncRot;

    [SerializeField] Transform myTransform;
	[SerializeField] float lerpRate = 15;

	void FixedUpdate() {
		TransmitPosition ();
		LerpPosition ();
	}

	void LerpPosition() {
		if (!isLocalPlayer) {
			myTransform.position = Vector3.Lerp(myTransform.position, syncPos, Time.deltaTime * lerpRate);
            myTransform.rotation = Quaternion.Lerp(myTransform.rotation, syncRot, Time.deltaTime * lerpRate);
		}
	}

	[Command]
	void CmdProvidePositionToServer (Vector3 pos) {
		syncPos = pos;
	}

    [Command]
    void CmdProvideRotationToServer(Quaternion rot)
    {
        syncRot = rot;
    }

    [ClientCallback]
	void TransmitPosition () {
		if (isLocalPlayer) {
			CmdProvidePositionToServer(myTransform.position);
            CmdProvideRotationToServer(myTransform.rotation);
		}
	}
}
