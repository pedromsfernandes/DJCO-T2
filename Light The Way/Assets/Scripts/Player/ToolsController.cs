using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ToolsController : MonoBehaviour
{
    GameObject tool1;
    GameObject tool2;
    GameObject tool3;

    void Start()
    {
        //TEMP - Setting up 1 instance of each tool when the player is created
        tool1 = Resources.Load<GameObject>("Tools/Tool1");
        tool2 = Resources.Load<GameObject>("Tools/Tool2");
        tool3 = Resources.Load<GameObject>("Tools/Tool3");

        tool1.transform.position = new Vector3(this.transform.position.x - 2, this.transform.position.y, this.transform.position.z + 2);
        tool2.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + 2);
        tool3.transform.position = new Vector3(this.transform.position.x + 2, this.transform.position.y, this.transform.position.z + 2);

        Instantiate(tool1);
        Instantiate(tool2);
        Instantiate(tool3);

        GameState.Instance.hasTool1 = false;
        GameState.Instance.hasTool2 = false;
        GameState.Instance.hasTool3 = false;

        GameState.Instance.canCreateLightBridges = true;
        GameState.Instance.canRotateSun = true;
    }

    void Update()
    {
        chooseTool();
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
        if(toolId == 1)
        {
            transform.Find("Laser").gameObject.GetComponent<LightBeam>().UpdateColor(true, false, false);
        }
        else if (toolId == 2)
        {
            transform.Find("Laser").gameObject.GetComponent<LightBeam>().UpdateColor(false, true, false);
        }
        else if (toolId == 3)
        {
            transform.Find("Laser").gameObject.GetComponent<LightBeam>().UpdateColor(false, false, true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision");
        if (other.gameObject.CompareTag("Tool"))
        {
            string toolName = other.gameObject.name;
            Debug.Log("Collision with Tool " + toolName);

            if(toolName == "Tool1(Clone)")
            {
                Debug.Log("Activating Tool 1 Power");
                GameState.Instance.hasTool1 = true;
                //takeOutTool(1);
            }
            else if (toolName == "Tool2(Clone)")
            {
                Debug.Log("Activating Tool 2 Power");
                GameState.Instance.hasTool2 = true;
                //takeOutTool(2);
            }
            else if (toolName == "Tool3(Clone)")
            {
                Debug.Log("Activating Tool 3 Power");
                GameState.Instance.hasTool3 = true;
                //takeOutTool(3);
            }
            


            deletePickedUpTool(toolName);
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
        tool.SetActive(false);
    }
}
