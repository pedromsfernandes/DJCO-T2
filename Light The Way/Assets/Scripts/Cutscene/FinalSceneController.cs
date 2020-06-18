using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class FinalSceneController : MonoBehaviour
{
    public Camera camera;
    public SceneCameraController cc;
    public Image splash;

    public GameObject[] waypoints;

    Vector3 lastPosition = new Vector3(0, 0, 0);
    bool stop = false;

    [FMODUnity.EventRef]
    public string cutsceneMusic = "event:/Misc/Original Theme Song/Light The Way Theme";

    void Start()
    {
        StartCoroutine("PlayScene");
    }

    void Update()
    {
        if(camera.transform.localPosition == lastPosition)
            stop = true;
        
        lastPosition = camera.transform.localPosition;
    }

    IEnumerator PlayScene()
    {
        FMODUnity.RuntimeManager.PlayOneShot(cutsceneMusic);
        cc.target = waypoints[0];
        cc.follow = waypoints[1];
        StartCoroutine("HideSplash");

        while(!stop) yield return null;

        cc.target = waypoints[3];
        cc.follow = waypoints[2];

        stop = false;
        while(!stop) yield return null;

        cc.target = waypoints[0];
        cc.follow = waypoints[4];
        
        stop = false;
        while(!stop) yield return null;

        cc.target = waypoints[6];
        cc.follow = waypoints[5];
        
        stop = false;
        while(!stop) yield return null;

        cc.target = waypoints[0];
        cc.follow = waypoints[7];
        
        stop = false;
        while(!stop) yield return null;

        cc.target = waypoints[9];
        cc.follow = waypoints[8];
        
        stop = false;
        while(!stop) yield return null;

        cc.target = waypoints[11];
        cc.follow = waypoints[10];
        
        stop = false;
        while(!stop) yield return null;

        cc.follow = waypoints[12];

        yield return new WaitForSeconds(3f);
        yield return StartCoroutine("ShowSplash");

        Application.Quit();
    }

    IEnumerator HideSplash()
    {
        for (int i = 0; i < 20; i++)
        {
            splash.color = new Color(splash.color.r, splash.color.g, splash.color.b, splash.color.a - 0.05f);
            yield return new WaitForSeconds(0.04f);
        }
    }

    IEnumerator ShowSplash()
    {
        for (int i = 0; i < 20; i++)
        {
            splash.color = new Color(splash.color.r, splash.color.g, splash.color.b, splash.color.a + 0.05f);
            yield return new WaitForSeconds(0.04f);
        }
    }
}
