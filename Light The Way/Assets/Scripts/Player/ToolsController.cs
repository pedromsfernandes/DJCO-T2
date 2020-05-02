using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolsController : MonoBehaviour
{
    public LightBeam lightBeam;

    // Start is called before the first frame update
    void Start()
    {
        lightBeam = transform.Find("Laser").gameObject.GetComponent<LightBeam>();
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
}
