using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Voice.Unity;
using Photon.Voice.PUN;
using Photon.Pun;

public class QuickInstantiate : MonoBehaviour
{
    [SerializeField]
    private GameObject _prefab;

    public Recorder recorder;

    private float volumeBeforeMute;

    [SerializeField]
    private Vector3[] positions;

    private void Awake()
    {
        int index = Array.FindIndex(PhotonNetwork.PlayerList, x => x == PhotonNetwork.LocalPlayer);

        Vector3 position = positions[index];

        MasterManager.NetworkInstantiate(_prefab, position, Quaternion.identity);

    }

    private void UpdateKeyBinds()
    {
        Dictionary<string, KeyCode> keybinds = MasterManager.GameSettings.keybinds;

        if (Input.GetKeyDown(keybinds["muteMic"]))
        {
            recorder.TransmitEnabled = !recorder.TransmitEnabled;
        }
        else if (Input.GetKeyDown(keybinds["muteSpeaker"]))
        {
            if (AudioListener.volume != 0f)
            {
                volumeBeforeMute = AudioListener.volume;
                AudioListener.volume = 0f;
            }
            else
            {
                AudioListener.volume = volumeBeforeMute;
                volumeBeforeMute = 0f;
            }
        }
    }

    private void Update()
    {
        UpdateKeyBinds();
    }
}
