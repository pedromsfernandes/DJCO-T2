using UnityEditor;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public static GameState Instance { get; private set; }
    
    //Light Beam
    public GameObject beamModel;
    
    public bool aiming = false;
    public Vector3 lastBeamHit;

    //Movement
    public bool running = false;
    public bool walkingSlow = false;

    //Ability Unlocks
    public bool canRotateSun = true;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }
}