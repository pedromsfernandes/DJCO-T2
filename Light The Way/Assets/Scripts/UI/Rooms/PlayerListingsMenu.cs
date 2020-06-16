using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class PlayerListingsMenu : MonoBehaviourPunCallbacks
{
    public GameObject canvas;

    [SerializeField]
    private PlayerListing _playerListing;

    [SerializeField]
    private Transform _content;

    [SerializeField]
    private Text _readyUpText;

    private List<PlayerListing> _listings = new List<PlayerListing>();

    private RoomsCanvases _roomsCanvases;

    private bool _ready = false;

    //Sound
    [FMODUnity.EventRef]
    public string selectedPickOptionSound = "event:/Misc/Menu/Menu Pick";
    [FMODUnity.EventRef]
    public string selectedBackSound = "event:/Misc/Menu/Menu Back";

    public void FirstInitialize(RoomsCanvases canvases)
    {
        _roomsCanvases = canvases;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        SetReadyUp(false);
        GetCurrentRoomPlayers();
        canvas.SetActive(false);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        for (int i = 0; i < _listings.Count; i++)
            Destroy(_listings[i].gameObject);

        _listings.Clear();
    }

    private void SetReadyUp(bool state)
    {
        _ready = state;

        _readyUpText.text = _ready ? "Ready!" : "Not Ready";
    }

    private void GetCurrentRoomPlayers()
    {
        if (!PhotonNetwork.IsConnected || PhotonNetwork.CurrentRoom == null || PhotonNetwork.CurrentRoom.Players == null)
            return;

        foreach (KeyValuePair<int, Player> playerInfo in PhotonNetwork.CurrentRoom.Players)
            AddPlayerListing(playerInfo.Value);

    }

    private void AddPlayerListing(Player player)
    {
        int index = _listings.FindIndex(x => x.Player == player);

        if (index != -1)
        {
            _listings[index].SetPlayerInfo(player);
        }
        else
        {
            PlayerListing listing = Instantiate(_playerListing, _content);

            if (listing != null)
            {
                listing.SetPlayerInfo(player);
                _listings.Add(listing);
            }
        }


    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        _roomsCanvases.CurrentRoomCanvas.LeaveRoomMenu.OnClick_LeaveRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddPlayerListing(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        int index = _listings.FindIndex(x => x.Player == otherPlayer);

        if (index != -1)
        {
            Destroy(_listings[index].gameObject);
            _listings.RemoveAt(index);
        }
    }

    public void OnClick_StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            base.photonView.RPC("LoadCutscene", RpcTarget.All);
        }
    }

    [PunRPC]
    private void LoadCutscene()
    {
        SceneManager.LoadScene(2);   
    }

    public void OnClick_ReadyUp()
    {
        SetReadyUp(!_ready);
        base.photonView.RPC("RPC_ChangeReadyState", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer, _ready);


        if (_ready)
        {
            FMODUnity.RuntimeManager.PlayOneShot(selectedPickOptionSound);
        }
        else
        {
            FMODUnity.RuntimeManager.PlayOneShot(selectedBackSound);
        }
    }

    [PunRPC]
    private void RPC_ChangeReadyState(Player player, bool ready)
    {
        int index = _listings.FindIndex(x => x.Player == player);

        if (index != -1)
            _listings[index].ready = ready;
    }
}
