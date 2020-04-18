using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentRoomCanvas : MonoBehaviour
{
    [SerializeField]
    private LeaveRoomMenu _leaveRoomMenu;

    [SerializeField]
    private PlayerListingsMenu _playerListingsMenu;

    private RoomsCanvases _roomsCanvases;

    public LeaveRoomMenu LeaveRoomMenu { get { return _leaveRoomMenu; } }

    public void FirstInitialize(RoomsCanvases canvases)
    {
        _roomsCanvases = canvases;
        _playerListingsMenu.FirstInitialize(canvases);
        _leaveRoomMenu.FirstInitialize(canvases);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
