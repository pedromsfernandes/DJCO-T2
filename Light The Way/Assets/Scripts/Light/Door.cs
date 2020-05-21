using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Light.Endpoint[] endpoints;

    void Update()
    {
        for (int i = 0; i < endpoints.Length; i++)
        {
            if (!endpoints[i].open)
                return;
        }

        GetComponent<PhotonView>().RPC("OpenDoorSelf", RpcTarget.All);
    }

    [PunRPC]
    public void OpenDoorSelf()
    {
        foreach (var ep in endpoints)
        {
            ep.Deactivate();
        }

        this.gameObject.SetActive(false);
    }
}
