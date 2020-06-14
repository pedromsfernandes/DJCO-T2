using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LightBridge : MonoBehaviour
{
    public float maxBridgeLength = 10;
    public float maxDistanceBetweenPlayerAndLightBridgePoint = 100;

    bool LightBridgeFirstPointFixed = false;
    Vector3 firstPoint;

    GameObject activeLightBridge;

    //sound
    [FMODUnity.EventRef]
    public string selectedFixatePointSound;
    FMOD.Studio.EventInstance fixatePointSoundEvent;

    [FMODUnity.EventRef]
    public string impossibleCreateSound;
    FMOD.Studio.EventInstance impossibleCreateSoundEvent;

    [FMODUnity.EventRef]
    public string selectedCreateSound;
    FMOD.Studio.EventInstance createSoundEvent;

    void Start()
    {
        //sound
        fixatePointSoundEvent = FMODUnity.RuntimeManager.CreateInstance(selectedFixatePointSound);
        impossibleCreateSoundEvent = FMODUnity.RuntimeManager.CreateInstance(impossibleCreateSound);
        createSoundEvent = FMODUnity.RuntimeManager.CreateInstance(selectedCreateSound);
    }

    void Update()
    {
        if (GameState.Instance.canCreateLightBridges && GameState.Instance.hasTool3 && GameState.Instance.currentTool == 3)
        {
            bool isFixatingPoint = Input.GetKeyDown("e");

            if (isFixatingPoint && isPointValid())
            {
                if (LightBridgeFirstPointFixed)
                {
                    if (isBridgePossible(firstPoint, GameState.Instance.lastBeamHit))
                    {
                        deleteLightBridge();
                        createLightBridge(firstPoint, GameState.Instance.lastBeamHit);
                    }

                    returnToIdle();
                                       
                }
                else
                {
                    fixateFirstPoint();
                }
            }
        }
    }


    private void fixateFirstPoint()
    {
        LightBridgeFirstPointFixed = true;
        firstPoint = GameState.Instance.lastBeamHit;

        fixatePointSoundEvent.start();
    }

    private bool isBridgePossible(Vector3 p1, Vector3 p2)
    {
        float distance = Vector3.Distance(p1, p2);
        if(distance <= maxBridgeLength)
        {
            return true;
        }

        FMODUnity.RuntimeManager.AttachInstanceToGameObject(impossibleCreateSoundEvent, GameState.Instance.playerTransform, GameState.Instance.playerRigidbody);
        impossibleCreateSoundEvent.start();

        return false;
    }

    private void returnToIdle()
    {
        LightBridgeFirstPointFixed = false;
    }

    private bool isPointValid()
    {
        Vector3 playerPosition = this.transform.position;
        float distance = Vector3.Distance(playerPosition, GameState.Instance.lastBeamHit);
        if (distance <= maxDistanceBetweenPlayerAndLightBridgePoint)
        {
            return true;
        }
        return false;
    }

    public void createLightBridge(Vector3 p1, Vector3 p2)
    {
        this.GetComponent<PhotonView>().RPC("createLightBridgeSelf", RpcTarget.All, p1, p2);
    }

    [PunRPC]
    void createLightBridgeSelf(Vector3 p1, Vector3 p2)
    {
        float distance = Vector3.Distance(p1, p2);
        Vector3 direction = p2 - p1;
        Vector2 dirYX = new Vector2(direction.y, direction.x);
        Vector2 dirYZ = new Vector2(direction.y, direction.z);
        float angleOfSlope;

        if (dirYX.magnitude > dirYZ.magnitude)
        {
            angleOfSlope = Mathf.Atan2(direction.y, direction.x);
        }
        else
        {
            angleOfSlope = Mathf.Atan2(direction.y, direction.z);
        }

        float xRotationAngle = -Mathf.Sin(angleOfSlope) * Mathf.Rad2Deg;
        float yRotationAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        GameObject lightBridge = Resources.Load<GameObject>("Light Bridge");
        
        lightBridge.transform.position = (p1 + p2) / 2;
        lightBridge.transform.localScale = new Vector3(lightBridge.transform.localScale.x, lightBridge.transform.localScale.y, distance * 1.15f);
        lightBridge.transform.rotation = Quaternion.Euler(xRotationAngle, yRotationAngle, 0);

        activeLightBridge = Instantiate(lightBridge);

        Rigidbody originalRigidbody = new Rigidbody();
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(createSoundEvent, lightBridge.transform, originalRigidbody);

        createSoundEvent.start();
    }

    public void deleteLightBridge()
    {
        this.GetComponent<PhotonView>().RPC("deleteLightBridgeSelf", RpcTarget.All);
    }

    [PunRPC]
    void deleteLightBridgeSelf()
    {
        if (activeLightBridge != null)
        {
            activeLightBridge.SetActive(false);
            Destroy(activeLightBridge);
        }
    }
}
