using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Light;

public class ToolsManager : MonoBehaviour
{
    public PlayerBeam lightBeam;

    // Start is called before the first frame update 
    void Start()
    {
        lightBeam = transform.Find("Laser").gameObject.GetComponent<PlayerBeam>();
        GameState.Instance.hasTool1 = true; 
        GameState.Instance.hasTool2 = true; 
        GameState.Instance.hasTool3 = true;
    }

    // Update is called once per frame 
    void Update()
    {
        chooseTool();
    }

    private void chooseTool()
    {
        /*if (GameState.Instance.hasTool1 && Input.GetKeyDown(KeyCode.R))
        {
            GameState.Instance.currentTool = 1;
            lightBeam.UpdateColor(LightColor.Of(Light.LightType.Red));
        }
        else if (GameState.Instance.hasTool2 && Input.GetKeyDown(KeyCode.G))
        {
            GameState.Instance.currentTool = 2;
            lightBeam.UpdateColor(LightColor.Of(Light.LightType.Green));
        }
        else if (GameState.Instance.hasTool3 && Input.GetKeyDown(KeyCode.B))
        {
            GameState.Instance.currentTool = 3;
            lightBeam.UpdateColor(LightColor.Of(Light.LightType.Blue));
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.Find("Laser").gameObject.GetComponent<PlayerBeam>().UpdateColor(LightColor.Of(Light.LightType.Red));
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            transform.Find("Laser").gameObject.GetComponent<PlayerBeam>().UpdateColor(LightColor.Of(Light.LightType.Green));
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            transform.Find("Laser").gameObject.GetComponent<PlayerBeam>().UpdateColor(LightColor.Of(Light.LightType.Blue));
        }*/
    }
}