using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class RoomListing : MonoBehaviour
{
    [SerializeField]
    private Text _text;


    [FMODUnity.EventRef]
    public string selectedPickOptionSound = "event:/Misc/Menu/Menu Pick";
    [FMODUnity.EventRef]
    public string selectedHooverSound = "event:/Misc/Menu/Menu Hoover";

    public RoomInfo RoomInfo { get; private set; }

    public void SetRoomInfo(RoomInfo roomInfo)
    {
        RoomInfo = roomInfo;
        _text.text = roomInfo.Name;
    }

    public void OnClick_Button()
    {
        PhotonNetwork.JoinRoom(RoomInfo.Name);
        Debug.Log("PLAY SELECT");
        FMODUnity.RuntimeManager.PlayOneShot(selectedPickOptionSound);
    }

    public void PlayHooverSound()
    {
        Debug.Log("PLAY HOOVER");
        FMODUnity.RuntimeManager.PlayOneShot(selectedHooverSound);
    }
}
