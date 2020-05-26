

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class UIOverlay : MonoBehaviour
{
    public GameObject pauseUI;

    private void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            GameState.Instance.pause = !GameState.Instance.pause;
            pauseUI.SetActive(GameState.Instance.pause);
            Cursor.lockState = GameState.Instance.pause ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = GameState.Instance.pause;
        }
    }
}
