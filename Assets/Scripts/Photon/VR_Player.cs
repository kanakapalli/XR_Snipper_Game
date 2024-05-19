using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class VR_Player : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        DisableOtherCameras();
    }
    private void DisableOtherCameras()
    {
        OVRCameraRig[] cameraRigs = FindObjectsOfType<OVRCameraRig>();
        foreach (OVRCameraRig cameraRig in cameraRigs)
        {
            if (!cameraRig.transform.parent.GetComponent<PhotonView>().IsMine)
            {
                cameraRig.gameObject.SetActive(false);
            }
            else
            {
                cameraRig.gameObject.SetActive(true);
            }
        }
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        DisableOtherCameras();
    }
}
