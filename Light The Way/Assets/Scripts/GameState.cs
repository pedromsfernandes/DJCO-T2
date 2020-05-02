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

    //Tools Picked Up
    public bool hasTool1 = false;
    public bool hasTool2 = false;
    public bool hasTool3 = false;

    //Ability Unlocks
    public bool canRotateSun = true;
    public bool canCreateLightBridges = true;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }
}