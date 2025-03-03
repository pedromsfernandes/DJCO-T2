﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class OwnershipTransfer : MonoBehaviourPun, IPunOwnershipCallbacks
{

    private void Awake()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void Destroy()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    private void OnMouseDown()
    {
        base.photonView.RequestOwnership();
    }

    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        if (targetView != base.photonView)
            return;

        base.photonView.TransferOwnership(requestingPlayer);
    }

    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        if (targetView != base.photonView)
            return;
    }
}
