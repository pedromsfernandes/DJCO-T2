using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class SetNickNameMenu : MonoBehaviour
{

    [SerializeField]
    private Text _nickName;

    private RoomsCanvases _roomsCanvases;

    public void FirstInitialize(RoomsCanvases canvases)
    {
        _roomsCanvases = canvases;
    }

    public void OnClick_SetNickName()
    {
        MasterManager.GameSettings.SetNickName(_nickName.text);
    }
}
