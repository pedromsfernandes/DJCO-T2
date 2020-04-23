using UnityEngine;
using System.Collections;
 
 public class GameState : MonoBehaviour
 {
    private static GameState instance = null;
    public static GameState Instance
    {
        get { return instance; }
    }

    //Light Beam
    public bool aiming = false;

    //Movement
    public bool running = false;
    public bool walkingSlow = false;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        instance = this;
    }
 }