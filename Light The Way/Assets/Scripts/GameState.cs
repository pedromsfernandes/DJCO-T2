using UnityEngine;

public class GameState : MonoBehaviour
{
    public static GameState Instance { get; private set; }
    
    // Sun
    public Vector3 sunDirection;
    
    // Light Beam
    public bool aiming = false;
    public bool castingRay = false;
    public Vector3 lastBeamHit;

    // Movement
    public float currentSurface = 0;
    public bool moving = false;
    public bool running = false;
    public bool walkingSlow = false;

    // Current Tool
    public int currentTool = 0;

    // Tools Picked Up
    public bool hasTool1 = false;
    public bool hasTool2 = false;
    public bool hasTool3 = false;

    // Ability Unlocks
    public bool canRotateSun = false;
    public bool canCreateLightBridges = false;
    public bool canDestroyObjects = false;

    public bool pause = false;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }
}