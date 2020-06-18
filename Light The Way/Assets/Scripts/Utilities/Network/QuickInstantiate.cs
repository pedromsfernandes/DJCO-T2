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
    private Vector3[] startPositions;


    [SerializeField]
    private Vector3[] bluePositions;


    [SerializeField]
    private Vector3[] redPositions;


    [SerializeField]
    private Vector3[] greenPositions;

    [SerializeField]
    private Vector3[] finalPositions;

    [SerializeField]
    private GameObject[] prefabs;

    private void Awake()
    {
        int index = Array.FindIndex(PhotonNetwork.PlayerList, x => x == PhotonNetwork.LocalPlayer);
        int level = MasterManager.Checkpoint;
        Vector3 position = startPositions[index];
        
        switch (level)
        {
            case 1:
                position = redPositions[index];
                break;
            case 2:
                position = bluePositions[index];
                break;
            case 3:
                position = greenPositions[index];
                break;
            case 4:
                position = finalPositions[index];
                break;
            default:
                break;
        }

        GameObject prefab = prefabs[index];

        MasterManager.NetworkInstantiate(prefab, position, Quaternion.identity);

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
