using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LightBridge : MonoBehaviour
{
    const float maxBridgeLength = 10;
    const float maxDistanceBetweenPlayerAndLightBridgePoint = 100;

    bool LightBridgeFirstPointFixed = false;
    Vector3 firstPoint;

    GameObject activeLightBridge;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameState.Instance.canCreateLightBridges && GameState.Instance.castingRay)
        {
            bool isFixatingPoint = Input.GetKeyDown("e");

            if (isFixatingPoint && isPointValid())
            {
                if (LightBridgeFirstPointFixed)
                {
                    if (isBridgePossible(firstPoint, GameState.Instance.lastBeamHit))
                    {
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
        Debug.Log("Fixating 1nd point");
        LightBridgeFirstPointFixed = true;
        firstPoint = GameState.Instance.lastBeamHit;
        Debug.Log(firstPoint);
    }

    private bool isBridgePossible(Vector3 p1, Vector3 p2)
    {
        float distance = Vector3.Distance(p1, p2);
        if(distance <= maxBridgeLength)
        {
            Debug.Log("Distance is less than maxBridgeLength");
            return true;
        }
        Debug.Log("Distance is NOT less than maxBridgeLength");
        return false;
    }

    private void returnToIdle()
    {
        LightBridgeFirstPointFixed = false;
        Debug.Log("Returning to Idle");
    }

    private bool isPointValid()
    {
        Vector3 playerPosition = this.transform.position;
        float distance = Vector3.Distance(playerPosition, GameState.Instance.lastBeamHit);
        if (distance <= maxDistanceBetweenPlayerAndLightBridgePoint)
        {
            Debug.Log("Distance is less than maxDistanceBetweenPlayerAndLightBridgePoint");
            return true;
        }
        Debug.Log("Distance is NOT less than maxDistanceBetweenPlayerAndLightBridgePoint");
        return false;
    }

    public void createLightBridge(Vector3 p1, Vector3 p2)
    {
        this.GetComponent<PhotonView>().RPC("createLightBridgeSelf", RpcTarget.All, p1, p2);
    }

    [PunRPC]
    void createLightBridgeSelf(Vector3 p1, Vector3 p2)
    {

        Debug.Log("Creating Light Bridge between: " + p1 + " and " + p2);
        float distance = Vector3.Distance(p1, p2);
        GameObject lightBridge = Resources.Load<GameObject>("Light Bridge");
        
        lightBridge.transform.position = (p1 + p2) / 2;
        lightBridge.transform.localScale = new Vector3(distance, lightBridge.transform.localScale.y, lightBridge.transform.localScale.z);

        
        activeLightBridge = Instantiate(lightBridge);
    }
}
