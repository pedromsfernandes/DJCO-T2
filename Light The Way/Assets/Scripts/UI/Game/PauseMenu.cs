using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;

public class PauseMenu : MonoBehaviour
{
    public void MainMenu()
    {
        PhotonNetwork.LeaveRoom(true);
        SceneManager.LoadScene("Rooms");
    }

    public void Continue()
    {
        GameState.Instance.pause = false;
        Cursor.lockState = GameState.Instance.pause ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = GameState.Instance.pause;
        this.gameObject.SetActive(false);
    }

    public void ExitGame()
    {
        PhotonNetwork.LeaveRoom(true);
        Application.Quit();
    }
}
