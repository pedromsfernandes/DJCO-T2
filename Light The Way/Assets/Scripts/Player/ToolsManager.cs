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

    void Start()
    {
        lightBeam = transform.Find("Laser").gameObject.GetComponent<PlayerBeam>();

        tool1 = GameObject.Find("Tool1");
        tool2 = GameObject.Find("Tool2");
        tool3 = GameObject.Find("Tool3");
        
        GameState.Instance.hasTool1 = false;
        GameState.Instance.hasTool2 = false;
        GameState.Instance.hasTool3 = false;

        GameState.Instance.canCreateLightBridges = true;
        GameState.Instance.canRotateSun = true;
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
    }

    void dropCurrentTool()
    {
        int toolId = GameState.Instance.currentTool;
        Debug.Log("Going to drop current tool: " + toolId);
        if (toolId != 0)
        {
            if (toolId == 1)
            {
                Debug.Log("hasTool1 = false");
                GameState.Instance.hasTool1 = false;
            }
            else if (toolId == 2)
            {
                Debug.Log("hasTool2 = false");
                GameState.Instance.hasTool2 = false;
            }

            else if (toolId == 3)
            {

                Debug.Log("hasTool3 = false");
                GameState.Instance.hasTool3 = false;
            }

            GameState.Instance.currentTool = 0;
            createDroppedTool(toolId);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision");
        if (other.gameObject.CompareTag("Tool"))
        {
            string toolName = other.gameObject.name;
            Debug.Log("Collision with Tool " + toolName);

            if (toolName == "Tool1(Clone)" || toolName == "Tool1")
            {
                Debug.Log("Activating Tool 1 Power");
                GameState.Instance.hasTool1 = true;
                GameState.Instance.currentTool = 1;
            }
            else if (toolName == "Tool2(Clone)" || toolName == "Tool2")
            {
                Debug.Log("Activating Tool 2 Power");
                GameState.Instance.hasTool2 = true;
                GameState.Instance.currentTool = 2;
            }
            else if (toolName == "Tool3(Clone)" || toolName == "Tool3")
            {
                Debug.Log("Activating Tool 3 Power");
                GameState.Instance.hasTool3 = true;
                GameState.Instance.currentTool = 3;
            }

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

    public void createDroppedTool(int toolId)
    {
        Debug.Log("Going to create Droped Tool Photon - " + toolId);
        Vector3 toolposition = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + 1);
        this.GetComponent<PhotonView>().RPC("createDroppedToolSelf", RpcTarget.All, toolId, toolposition);
    }

    [PunRPC]
    void createDroppedToolSelf(int toolId, Vector3 position)
    {
        Debug.Log("Going to create Droped Tool - " + toolId);
        if (toolId == 1)
        {
            tool1 = Resources.Load<GameObject>("Tools/Tool1");
            tool1.transform.position = position;
            Instantiate(tool1);
            Debug.Log("Tool 1 set active on - " + position);
        }
        else if (toolId == 2)
        {
            tool2 = Resources.Load<GameObject>("Tools/Tool2");
            tool2.transform.position = position;
            Instantiate(tool2);
            Debug.Log("Tool 2 set active on - " + position);
        }
        else if (toolId == 3)
        {
            tool3 = Resources.Load<GameObject>("Tools/Tool3");
            tool3.transform.position = position;
            Instantiate(tool3);
            Debug.Log("Tool 3 set active on - " + position);
        }
    }

    public void deletePickedUpTool(string toolName)
    {
        Debug.Log("Photon destroy tool " + toolName);
        this.GetComponent<PhotonView>().RPC("deletePickedUpToolSelf", RpcTarget.All, toolName);
    }

    [PunRPC]
    void deletePickedUpToolSelf(string toolName)
    {
        Debug.Log("Destroying Tool " + toolName);
        GameObject tool = GameObject.Find(toolName);
        Destroy(tool);
    }
}