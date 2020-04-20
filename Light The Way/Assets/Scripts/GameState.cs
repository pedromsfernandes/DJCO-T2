using UnityEngine;
using System.Collections;
 
 public class GameState : MonoBehaviour
 {
    private static GameState instance = null;
    public static GameState Instance
    {
        get { return instance; }
    }

    public bool aiming = false;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        instance = this;
    }
 }