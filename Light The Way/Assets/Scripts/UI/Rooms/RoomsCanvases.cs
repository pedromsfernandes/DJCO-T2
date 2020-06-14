using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomsCanvases : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private CreateOrJoinRoomCanvas _createOrJoinRoomCanvas;

    public CreateOrJoinRoomCanvas CreateOrJoinRoomCanvas { get { return _createOrJoinRoomCanvas; } }

    [SerializeField]
    private CurrentRoomCanvas _currentRoomCanvas;

    public CurrentRoomCanvas CurrentRoomCanvas { get { return _currentRoomCanvas; } }

    private void Awake()
    {
        FirstInitialize();
    }

    private void FirstInitialize()
    {   
        CurrentRoomCanvas.FirstInitialize(this);
        CreateOrJoinRoomCanvas.FirstInitialize(this);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
       this.CreateOrJoinRoomCanvas._roomListingsMenu.OnRoomListUpdate(roomList);
    }
}
