using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public Camera camera;
    public Image splash;
    public Text upperLeftText;

    public GameObject set1;

    void Start()
    {
        StartCoroutine("PlayScene");
    }

    IEnumerator PlayScene()
    {
        Coroutine cm = StartCoroutine("CameraZoom");
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine("HideSplash");
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(AddText(upperLeftText, "Mid-August, 1993"));
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(AddText(upperLeftText, "\nSomewhere in the South Pacific Ocean"));
        yield return new WaitForSeconds(3f);

        upperLeftText.text = "";
        yield return StartCoroutine(AddText(upperLeftText, "Aukland Captaincy Traffic Control recieved a distrss signal from an unidentified mid-class ship. "));
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(AddText(upperLeftText, "Heavy interference was caused by a cathegory 4 storm 120 kilometers east of the new zelander coast. "));
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
        set1.SetActive(false);
    }

    IEnumerator CameraZoom()
    {
        while(true)
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
