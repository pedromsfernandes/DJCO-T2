using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Light;

public class ToolsManager : MonoBehaviour
{
    public PlayerBeam lightBeam;

    GameObject tool1;
    GameObject tool2;
    GameObject tool3;

    private Transform mainCameraTransform;

    //Sound
    [FMODUnity.EventRef]
    public string selectedSwapToolSound;

    [FMODUnity.EventRef]
    public string selectedPickUpToolSound;

    void Start()
    {
        lightBeam = transform.Find("Laser").gameObject.GetComponent<PlayerBeam>();

        mainCameraTransform = Camera.main.transform;

        tool1 = GameObject.Find("Tool1");
        tool2 = GameObject.Find("Tool2");
        tool3 = GameObject.Find("Tool3");

        int index = Array.FindIndex(PhotonNetwork.PlayerList, x => x == PhotonNetwork.LocalPlayer);

        switch (index)
        {
            case 0:
                GameState.Instance.hasTool1 = true;
                GameState.Instance.currentTool = 1;
                if (MasterManager.Checkpoint >= 1)
                    GameState.Instance.canDestroyObjects = true;
                break;
            case 1:
                GameState.Instance.hasTool2 = true;
                GameState.Instance.currentTool = 2;
                if (MasterManager.Checkpoint >= 3)
                    GameState.Instance.canRotateSun = true;
                break;
            case 2:
                GameState.Instance.hasTool3 = true;
                GameState.Instance.currentTool = 3;
                if (MasterManager.Checkpoint >= 2)
                    GameState.Instance.canCreateLightBridges = true;
                break;
            default:
                break;
        }
    }

    void Update()
    {
        chooseTool();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            dropCurrentTool();
        }
    }

    private void chooseTool()
    {
        if (GameState.Instance.hasTool1 && Input.GetKeyDown(KeyCode.R))
        {
            takeOutTool(1);
        }
        else if (GameState.Instance.hasTool2 && Input.GetKeyDown(KeyCode.G))
        {
            takeOutTool(2);
        }
        else if (GameState.Instance.hasTool3 && Input.GetKeyDown(KeyCode.B))
        {
            takeOutTool(3);
        }
    }

    private void takeOutTool(int toolId)
    {
        GameState.Instance.currentTool = toolId;
        if (toolId == 1)
        {
            lightBeam.UpdateColor(LightColor.Of(Light.LightType.Red));
        }
        else if (toolId == 2)
        {
            lightBeam.UpdateColor(LightColor.Of(Light.LightType.Green));
        }
        else if (toolId == 3)
        {
            lightBeam.UpdateColor(LightColor.Of(Light.LightType.Blue));
        }

        if (GetComponent<PhotonView>().IsMine)
        {
            GetComponent<PhotonView>().RPC("takeOutToolSoundSelf", RpcTarget.All, GameState.Instance.playerTransform.name);
        }
    }

    bool isObjectHere(Vector3 position)
    {
        Collider[] intersecting = Physics.OverlapSphere(position, 1f);

        return intersecting.Length != 0;
    }

    void dropCurrentTool()
    {
        int toolId = GameState.Instance.currentTool;
        //Debug.Log("Going to drop current tool: " + toolId);

        if (toolId != 0)
        {

            Vector3 direction = mainCameraTransform.forward;
            direction.y = 0;
            direction.Normalize();
            direction *= 2;

            Vector3 toolPosition = this.transform.position + direction;

            if (this.isObjectHere(toolPosition))
            {
                return;
            }

            if (toolId == 1)
            {
                //Debug.Log("hasTool1 = false");
                GameState.Instance.hasTool1 = false;
            }
            else if (toolId == 2)
            {
                //Debug.Log("hasTool2 = false");
                GameState.Instance.hasTool2 = false;
            }

            else if (toolId == 3)
            {
                //Debug.Log("hasTool3 = false");
                GameState.Instance.hasTool3 = false;
            }

            GameState.Instance.currentTool = 0;
            createDroppedTool(toolId, toolPosition);

            if (!(GameState.Instance.hasTool1 || GameState.Instance.hasTool2 || GameState.Instance.hasTool3))
                lightBeam.Enable(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GetComponent<PhotonView>().IsMine && other.gameObject.CompareTag("Tool"))
        {
            string toolName = other.gameObject.name;
            Debug.Log("Collision with Tool " + toolName);

            if (toolName == "Tool1(Clone)" || toolName == "Tool1")
            {
                GameState.Instance.hasTool1 = true;
                GameState.Instance.currentTool = 1;
            }
            else if (toolName == "Tool2(Clone)" || toolName == "Tool2")
            {
                GameState.Instance.hasTool2 = true;
                GameState.Instance.currentTool = 2;
            }
            else if (toolName == "Tool3(Clone)" || toolName == "Tool3")
            {
                GameState.Instance.hasTool3 = true;
                GameState.Instance.currentTool = 3;
            }

            FMODUnity.RuntimeManager.PlayOneShot(selectedPickUpToolSound, GameState.Instance.playerTransform.position);

            deletePickedUpTool(toolName);
            Invoke("UpdateColor", Time.deltaTime);
        }
    }

    private void UpdateColor()
    {
        if (GameState.Instance.currentTool == 1)
        {
            lightBeam.UpdateColor(LightColor.Of(Light.LightType.Red));
        }
        else if (GameState.Instance.currentTool == 2)
        {
            lightBeam.UpdateColor(LightColor.Of(Light.LightType.Green));
        }
        else if (GameState.Instance.currentTool == 3)
        {
            lightBeam.UpdateColor(LightColor.Of(Light.LightType.Blue));
        }
    }

    public void createDroppedTool(int toolId, Vector3 position)
    {
        this.GetComponent<PhotonView>().RPC("createDroppedToolSelf", RpcTarget.All, toolId, position);
    }

    [PunRPC]
    void createDroppedToolSelf(int toolId, Vector3 position)
    {
        FMODUnity.RuntimeManager.PlayOneShot(selectedSwapToolSound, position);

        if (toolId == 1)
        {
            tool1 = Resources.Load<GameObject>("Tools/Tool1");
            tool1.transform.position = position;
            Instantiate(tool1);
        }
        else if (toolId == 2)
        {
            tool2 = Resources.Load<GameObject>("Tools/Tool2");
            tool2.transform.position = position;
            Instantiate(tool2);
        }
        else if (toolId == 3)
        {
            tool3 = Resources.Load<GameObject>("Tools/Tool3");
            tool3.transform.position = position;
            Instantiate(tool3);
        }
    }

    public void deletePickedUpTool(string toolName)
    {
        //Debug.Log("Photon destroy tool " + toolName);
        this.GetComponent<PhotonView>().RPC("deletePickedUpToolSelf", RpcTarget.All, toolName);
    }

    [PunRPC]
    void deletePickedUpToolSelf(string toolName)
    {
        GameObject tool = GameObject.Find(toolName);
        Destroy(tool);
    }


    [PunRPC]
    private void takeOutToolSoundSelf(string originalPlayerName)
    {
        if (this.transform.name != originalPlayerName)
            return;
        GameObject originalPlayer = GameObject.Find(originalPlayerName);
        Transform originalTransform = originalPlayer.GetComponent<Transform>();

        FMODUnity.RuntimeManager.PlayOneShot(selectedSwapToolSound, originalTransform.position);
    }
}