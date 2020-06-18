using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class MovieController : MonoBehaviour
{
    public Camera camera;
    public Image splash;

    public GameObject set1;
    public GameObject set2;
    public GameObject set3;
    public GameObject set4;
    public GameObject staff;

    public Image logo;
    public Image logo_hl;
    public Image laser;

    float speed = 0;

    void Start()
    {
        StartCoroutine("PlayScene");
    }

    IEnumerator PlayScene()
    {
        speed = 10f;
        Coroutine cm = StartCoroutine("CameraZoom");
        yield return StartCoroutine("HideSplash");
        yield return new WaitForSeconds(3.5f);
        splash.color = new Color(splash.color.r, splash.color.g, splash.color.b, 1f);
        StopCoroutine(cm);
        yield return new WaitForSeconds(1.5f);

        speed = 2f;
        camera.transform.localPosition = new Vector3(-8f, 94f, 2076f);
        camera.transform.localEulerAngles = new Vector3(5f, -201f, 0f);
        cm = StartCoroutine("CameraZoom");
        yield return StartCoroutine("HideSplash");
        yield return new WaitForSeconds(3.5f);
        splash.color = new Color(splash.color.r, splash.color.g, splash.color.b, 1f);
        StopCoroutine(cm);
        yield return new WaitForSeconds(1.5f);

        speed = 0.01f;
        camera.transform.localPosition = new Vector3(13.9f, 0f, 1489.8f);
        camera.transform.localEulerAngles = new Vector3(79.82f, -138.9f, 0f);
        set1.SetActive(true);
        cm = StartCoroutine("CameraZoom");
        yield return StartCoroutine("HideSplash");
        yield return new WaitForSeconds(3.5f);
        splash.color = new Color(splash.color.r, splash.color.g, splash.color.b, 1f);
        StopCoroutine(cm);
        yield return new WaitForSeconds(1.5f);

        camera.transform.localPosition = new Vector3(10.21f, -4.345f, 1476.87f);
        camera.transform.localEulerAngles = new Vector3(8.35f, -329.97f, 0f);
        set1.SetActive(false);
        set3.SetActive(true);
        yield return StartCoroutine("HideSplash");
        yield return new WaitForSeconds(1.5f);
        yield return StartCoroutine("ShowSplash");
        yield return StartCoroutine("MenuAnim");
    }

    IEnumerator CameraZoom()
    {
        while (true)
        {
            camera.transform.localPosition += camera.transform.forward.normalized * speed;
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator HideSplash()
    {
        for (int i = 0; i < 20; i++)
        {
            splash.color = new Color(splash.color.r, splash.color.g, splash.color.b, splash.color.a - 0.05f);
            yield return new WaitForSeconds(0.05f);
        }
    }

    IEnumerator ShowSplash()
    {
        for (int i = 0; i < 20; i++)
        {
            splash.color = new Color(splash.color.r, splash.color.g, splash.color.b, splash.color.a + 0.02f);
            yield return new WaitForSeconds(0.04f);
        }
    }

    IEnumerator MenuAnim()
    {
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < 20; i++)
        {
            logo.color = new Color(logo.color.r, logo.color.g, logo.color.b, logo.color.a + 0.05f);
            yield return new WaitForSeconds(0.05f);
        }

        for (int i = 0; i < 20; i++)
        {
            laser.rectTransform.offsetMax = new Vector2(laser.rectTransform.offsetMax.x + 120.5f, laser.rectTransform.offsetMax.y);
            yield return new WaitForSeconds(0.025f);
        }

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < 20; i++)
        {
            logo_hl.color = new Color(logo_hl.color.r, logo_hl.color.g, logo_hl.color.b, logo_hl.color.a + 0.05f);
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(2f);

        for (int i = 0; i < 20; i++)
        {
            logo_hl.color = new Color(logo_hl.color.r, logo_hl.color.g, logo_hl.color.b, logo_hl.color.a - 0.05f);
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(1.5f);
    }
}
