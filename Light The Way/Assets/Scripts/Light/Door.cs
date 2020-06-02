using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Light.Endpoint[] endpoints;

    //sound
    [FMODUnity.EventRef]
    public string selectedOpenSound = "event:/FX/Door/DoorOpen";
    FMOD.Studio.EventInstance openSoundEvent;

    void Start()
    {
        //sound
        openSoundEvent = FMODUnity.RuntimeManager.CreateInstance(selectedOpenSound);
    }

    void Update()
    {
        for (int i = 0; i < endpoints.Length; i++)
        {
            if (!endpoints[i].open)
                return;
        }

        GetComponent<PhotonView>().RPC("OpenDoorSelf", RpcTarget.All, this.name);
    }

    [PunRPC]
    public void OpenDoorSelf(string originalObjectName)
    {
        foreach (var ep in endpoints)
        {
            ep.Deactivate();
        }

        GameObject originalObject = GameObject.Find(originalObjectName);
        Transform originalTransform = originalObject.GetComponent<Transform>();
        Rigidbody originalRigidbody = originalObject.GetComponentInChildren<Rigidbody>();

        FMODUnity.RuntimeManager.AttachInstanceToGameObject(openSoundEvent, originalTransform, originalRigidbody);
        openSoundEvent.start();

        this.gameObject.SetActive(false);
    }
}
