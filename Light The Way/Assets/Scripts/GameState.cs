using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour
{
    public static GameState Instance { get; private set; }
    //Light Beam
    public bool aiming = false;
    public bool castingRay = false;
    public Vector3 lastBeamHit;

    //Movement
    public bool running = false;
    public bool walkingSlow = false;

    //Current Tool
    public int currentTool = 0;

    //Tools Picked Up
    public bool hasTool1;
    public bool hasTool2;
    public bool hasTool3;

    //Ability Unlocks
    public bool canRotateSun;
    public bool canCreateLightBridges;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }
}