using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class SceneController : MonoBehaviour
{
    public Camera camera;
    public Image splash;
    public Text upperLeftText;
    public Text rightText;
    public Text leftText;

    public GameObject set1;
    public GameObject set2;
    public GameObject set3;
    public GameObject set4;
    public GameObject staff;

    [FMODUnity.EventRef]
    public string cutsceneMusic = "event:/Misc/Original Theme Song/Light The Way Theme";

    void Start()
    {
        int level = MasterManager.Checkpoint;

        if (level == 0)
            StartCoroutine("PlayScene");
        else if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.LoadLevel(1);
        }
    }

    IEnumerator PlayScene()
    {
        FMODUnity.RuntimeManager.PlayOneShot(cutsceneMusic);

        Coroutine cm = StartCoroutine("CameraZoom");
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine("HideSplash");
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(AddText(upperLeftText, "Mid-August, 1993"));
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(AddText(upperLeftText, "\nSomewhere in the South Pacific Ocean"));
        yield return new WaitForSeconds(3f);

        upperLeftText.text = "";
        yield return StartCoroutine(AddText(upperLeftText, "Auckland Captaincy Traffic Control received a distress signal from an unidentified mid-class ship. "));
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(AddText(upperLeftText, "Heavy interference was caused by a cathegory 4 storm 120 kilometers east of the new zealander coast. "));
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(AddText(upperLeftText, "No further communication was established."));
        yield return new WaitForSeconds(4f);

        yield return StartCoroutine(HideText(upperLeftText));
        yield return new WaitForSeconds(2f);

        yield return StartCoroutine("ShowSplash");
        upperLeftText.color = new Color(upperLeftText.color.r, upperLeftText.color.g, upperLeftText.color.b, 1f);
        StopCoroutine(cm);
        camera.transform.localPosition = new Vector3(13.9f, 0f, 1489.8f);
        camera.transform.localEulerAngles = new Vector3(-20.18f, -138.91f, 0f);
        upperLeftText.text = "";
        set1.SetActive(true);

        yield return new WaitForSeconds(2f);
        yield return StartCoroutine("HideSplash");
        yield return new WaitForSeconds(1.5f);
        yield return StartCoroutine("CameraPanLow");
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine("ShowSplash");

        yield return new WaitForSeconds(2f);
        set1.SetActive(false);
        set2.SetActive(true);
        camera.transform.localPosition = new Vector3(16.4f, -8.1f, 1492f);
        camera.transform.localEulerAngles = new Vector3(8.35f, -150.83f, 0f);
        yield return StartCoroutine("HideSplash");
        yield return new WaitForSeconds(1.5f);

        yield return StartCoroutine(AddText(rightText, "Uh, my head..."));
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(AddText(rightText, "\nAre you guys ok?"));
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(AddText(leftText, "Yeah, we're fine..."));
        yield return new WaitForSeconds(3f);
        rightText.text = "";
        leftText.text = "";
        yield return StartCoroutine(AddText(rightText, "Wait, what are those?"));
        yield return new WaitForSeconds(1.5f);
        rightText.text = "";
        leftText.text = "";
        yield return StartCoroutine("ShowSplash");

        yield return new WaitForSeconds(0.5f);

        set2.SetActive(false);
        set3.SetActive(true);
        staff.SetActive(true);
        camera.transform.localPosition = new Vector3(10.21f, -4f, 1476.87f);
        camera.transform.localEulerAngles = new Vector3(8.35f, -329.97f, 0f);
        yield return StartCoroutine("HideSplash");

        yield return StartCoroutine("CameraPanDown");
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(AddText(rightText, "And, more importantly..."));
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(AddText(rightText, "\nWhat is that?"));
        yield return new WaitForSeconds(1f);
        rightText.text = "";
        yield return StartCoroutine("ShowSplash");

        yield return new WaitForSeconds(0.5f);

        set3.SetActive(false);
        staff.SetActive(false);
        set4.SetActive(true);
        camera.transform.localPosition = new Vector3(20.93f, -7.8f, 1483f);
        camera.transform.localEulerAngles = new Vector3(3.4f, -241f, 0f);
        yield return StartCoroutine("HideSplash");
        yield return new WaitForSeconds(3f);
        yield return StartCoroutine(AddText(rightText, "Where are we?"));
        yield return new WaitForSeconds(3f);
        yield return StartCoroutine("ShowSplash");
        yield return new WaitForSeconds(2f);

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.LoadLevel(1);
        }
    }

    IEnumerator CameraZoom()
    {
        while (true)
        {
            camera.transform.localPosition += camera.transform.forward.normalized;
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator CameraPanLow()
    {
        for (int i = 0; i < 200; i++)
        {
            camera.transform.localEulerAngles = new Vector3(camera.transform.localEulerAngles.x + 0.5f, camera.transform.localEulerAngles.y, camera.transform.localEulerAngles.z);
            yield return new WaitForSeconds(0.03f);
        }
    }

    IEnumerator CameraPanDown()
    {
        for (int i = 0; i < 200; i++)
        {
            camera.transform.localPosition = new Vector3(camera.transform.localPosition.x, camera.transform.localPosition.y - 0.015f, camera.transform.localPosition.z);
            yield return new WaitForSeconds(0.03f);
        }
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

    IEnumerator AddText(Text t, string s)
    {
        for (int i = 0; i < s.Length; i++)
        {
            t.text += s[i];
            yield return new WaitForSeconds(0.04f);
        }
    }

    IEnumerator HideText(Text text)
    {
        for (int i = 0; i < 20; i++)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - 0.05f);
            yield return new WaitForSeconds(0.03f);
        }
    }
}
