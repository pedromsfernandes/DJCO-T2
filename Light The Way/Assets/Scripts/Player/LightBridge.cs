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
    }

    private bool isBridgePossible(Vector3 p1, Vector3 p2)
    {
        float distance = Vector3.Distance(p1, p2);
        if(distance <= maxBridgeLength)
        {
            return true;
        }
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
        lightBridge.transform.localScale = new Vector3(lightBridge.transform.localScale.x, lightBridge.transform.localScale.y, distance);
        lightBridge.transform.rotation = Quaternion.Euler(xRotationAngle, yRotationAngle, 0);

        activeLightBridge = Instantiate(lightBridge);
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
