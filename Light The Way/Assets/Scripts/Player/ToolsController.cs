using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ToolsController : MonoBehaviour
{
    public LightBeam lightBeam;

    GameObject tool1;
    GameObject tool2;
    GameObject tool3;

    // Start is called before the first frame update
    void Start()
    {
        lightBeam = transform.Find("Laser").gameObject.GetComponent<LightBeam>();

        tool1 = Resources.Load<GameObject>("Tools/Tool1");
        tool2 = Resources.Load<GameObject>("Tools/Tool2");
        tool3 = Resources.Load<GameObject>("Tools/Tool3");

        tool1.transform.position = new Vector3(this.transform.position.x - 2, this.transform.position.y, this.transform.position.z + 2);
        tool2.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + 2);
        tool3.transform.position = new Vector3(this.transform.position.x + 2, this.transform.position.y, this.transform.position.z + 2);

        Instantiate(tool1);
        Instantiate(tool2);
        Instantiate(tool3);


        /*GameState.Instance.hasTool1 = true;
        GameState.Instance.hasTool2 = true;
        GameState.Instance.hasTool3 = true;*/
    }

    // Update is called once per frame
    void Update()
    {
        chooseTool();
    }

    private void chooseTool()
    {
        if (GameState.Instance.hasTool1 && Input.GetKeyDown(KeyCode.R))
        {
            GameState.Instance.currentTool = 1;
            lightBeam.UpdateColor(true, false, false);
        }
        else if (GameState.Instance.hasTool2 && Input.GetKeyDown(KeyCode.G))
        {
            GameState.Instance.currentTool = 2;
            lightBeam.UpdateColor(false, true, false);
        }
        else if (GameState.Instance.hasTool3 && Input.GetKeyDown(KeyCode.B))
        {
            GameState.Instance.currentTool = 3;
            lightBeam.UpdateColor(false, false, true);
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
            }
            else if (toolName == "Tool2(Clone)")
            {
                Debug.Log("Activating Tool 2 Power");
            }
            else if (toolName == "Tool3(Clone)")
            {
                Debug.Log("Activating Tool 3 Power");
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
