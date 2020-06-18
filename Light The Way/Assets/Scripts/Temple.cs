using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public enum TempleName
{

    Tutorial,
    Red,
    Green,
    Blue,
    Final
}

public class Temple : MonoBehaviour
{
    public Light.Endpoint[] endpoints;
    public bool completed = false;

    public TempleName templeName;

    public string message;

    public float delay = 3f;

    public Text text;

    public LineRenderer laser;

    // Update is called once per frame
    void Update()
    {
        if (!completed)
        {
            for (int i = 0; i < endpoints.Length; i++)
            {
                if (endpoints[i].active)
                    return;
            }

            completed = true;
            int index = Array.FindIndex(PhotonNetwork.PlayerList, x => x == PhotonNetwork.LocalPlayer);

            StartCoroutine(ShowMessage());

            switch (templeName)
            {
                case TempleName.Tutorial:
                    if (index == 0)
                        GameState.Instance.canDestroyObjects = true;
                    break;
                case TempleName.Red:
                    if (index == 2)
                        GameState.Instance.canCreateLightBridges = true;
                    laser.SetPosition(1, new Vector3(-289.8f, 144.5f, -80.1f));
                    break;
                case TempleName.Blue:
                    if (index == 1)
                        GameState.Instance.canRotateSun = true;
                    laser.SetPosition(1, new Vector3(-289.8f, 144.5f, -80.1f));
                    break;
                 case TempleName.Green:
                    laser.SetPosition(1, new Vector3(-289.8f, 144.5f, -80.1f));
                    break;
                default:
                    break;
            }
        }
    }

    IEnumerator ShowMessage()
    {
        text.text = message;
        text.gameObject.SetActive(true);
        yield return new WaitForSeconds(delay);
        text.gameObject.SetActive(false);
    }
}
