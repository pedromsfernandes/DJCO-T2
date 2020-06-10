using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class CreateRoomMenu : MonoBehaviourPunCallbacks
{
    public MainMenuController menu;

    [SerializeField]
    private Text _roomName;

    private RoomsCanvases _roomsCanvases;

    private bool _private = false;

    public void FirstInitialize(RoomsCanvases canvases)
    {
        _roomsCanvases = canvases;
    }

    public void OnClick_CreateRoom()
    {
        if (!PhotonNetwork.IsConnected)
            return;

        Debug.Log(_private);

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 3;
        roomOptions.BroadcastPropsChangeToAll = true;
        roomOptions.IsVisible = !_private;

        PhotonNetwork.JoinOrCreateRoom(_roomName.text, roomOptions, TypedLobby.Default);
    }

    public void OnValueChanged_TogglePrivate(bool isPrivate)
    {
        this._private = isPrivate;
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Created room successfully");
        menu.CreateRoomSuccessful();
    }

    public override void OnJoinedRoom()
    {
        menu.CreateRoomSuccessful();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Room creation failed: " + message);
    }
}
