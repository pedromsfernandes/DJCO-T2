using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[CreateAssetMenu(menuName = "Manager/GameSettings")]
public class GameSettings : ScriptableObject
{
    [SerializeField]
    private string _gameVersion = "0.0.0";

    public string GameVersion { get { return _gameVersion; } }

    [SerializeField]
    private string _nickName = "Punfish";

    private string PLAYER_PREFS_NICKNAME = "nickName";

    public Dictionary<string, KeyCode> keybinds = new Dictionary<string, KeyCode>{
        {"muteMic", KeyCode.M},
        {"muteSpeaker", KeyCode.N}
    };

    public string FirstNickName
    {
        get
        {

            string jsonString = PlayerPrefs.GetString(PLAYER_PREFS_NICKNAME);

            if (PlayerPrefs.HasKey(PLAYER_PREFS_NICKNAME))
                return jsonString;

            int value = Random.Range(0, 9999);
            return _nickName + value.ToString();
        }
    }

    public string NickName { get { return _nickName; } }

    public void SetNickName(string newNickName)
    {
        this._nickName = newNickName;
        PhotonNetwork.NickName = newNickName;
        PlayerPrefs.SetString(PLAYER_PREFS_NICKNAME, newNickName);
        PlayerPrefs.Save();
    }

}
