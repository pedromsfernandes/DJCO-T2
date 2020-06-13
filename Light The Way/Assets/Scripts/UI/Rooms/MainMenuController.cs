using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    static bool first = true;
    static bool shading = false;

    float counter = 0;

    public GameObject canvas;
    public GameObject[] altCanvas;
    public GameObject[] btns;
    public GameObject[] subBtns;

    public Image logo;
    public Image logo_hl;
    public Image laser;

    Vector2 screenSize;

    //Sound
    [FMODUnity.EventRef]
    public string selectedPickOptionSound = "event:/Misc/Menu/Menu Pick";
    [FMODUnity.EventRef]
    public string selectedBackSound = "event:/Misc/Menu/Menu Back";
    [FMODUnity.EventRef]
    public string selectedHooverSound = "event:/Misc/Menu/Menu Hoover";

    void Start()
    {
        //Generate world space point information for position and scale calculations
        screenSize.x = canvas.GetComponent<RectTransform>().sizeDelta.x;
        screenSize.y = canvas.GetComponent<RectTransform>().sizeDelta.y;

        float delta = 4.5f * screenSize.y / 9f;

        for (int i = 0; i < btns.Length; i++)
        {
            btns[i].GetComponent<RectTransform>().sizeDelta = new Vector2(screenSize.y / 2.5f, screenSize.y / 9f);
            btns[i].GetComponent<RectTransform>().localPosition = new Vector3(0, (-screenSize.y / 2f) - (screenSize.y / 9f) * (i + 1), 0);
            btns[i].transform.Find("Text").GetComponent<Text>().fontSize = (int)(btns[i].GetComponent<RectTransform>().sizeDelta.y / 2f);
        }

        for (int i = 0; i < subBtns.Length; i++)
        {
            subBtns[i].GetComponent<RectTransform>().sizeDelta = new Vector2(screenSize.y / 2.5f, screenSize.y / 9f);
            subBtns[i].GetComponent<RectTransform>().localPosition = new Vector3(0, delta + (-screenSize.y / 2f) - (screenSize.y / 9f) * 4, 0);
            subBtns[i].transform.Find("Text").GetComponent<Text>().fontSize = (int)(subBtns[i].GetComponent<RectTransform>().sizeDelta.y / 2f);
        }

        if (first)
        {
            StartCoroutine(FirstMenuAnim(delta));
            first = false;
        }
        else
        {
            logo.color = new Color(logo.color.r, logo.color.g, logo.color.b, 1f);
            laser.rectTransform.offsetMax = new Vector2(0, laser.rectTransform.offsetMax.y);

            foreach (GameObject btn in btns)
                btn.transform.localPosition = new Vector3(btn.transform.localPosition.x, btn.transform.localPosition.y + delta, btn.transform.localPosition.z);
        }
    }

    void Update()
    {
        if (shading)
        {
            counter += Time.deltaTime;
            if (counter >= 5f)
            {
                StartCoroutine("ShadingAnim");
                counter = -10000f;
            }
        }
    }

    public void JoinRoom()
    {
        canvas.SetActive(false);
        altCanvas[0].SetActive(true);
        FMODUnity.RuntimeManager.PlayOneShot(selectedPickOptionSound);
    }

    public void CreateRoom()
    {
        canvas.SetActive(false);
        altCanvas[1].SetActive(true);
        FMODUnity.RuntimeManager.PlayOneShot(selectedPickOptionSound);
    }

    public void Settings()
    {
        canvas.SetActive(false);
        altCanvas[2].SetActive(true);
        FMODUnity.RuntimeManager.PlayOneShot(selectedPickOptionSound);
    }

    public void CreateRoomSuccessful()
    {
        altCanvas[1].SetActive(false);
        altCanvas[3].SetActive(true);
        FMODUnity.RuntimeManager.PlayOneShot(selectedPickOptionSound);
    }

    public void Back()
    {
        canvas.SetActive(true);
        altCanvas[0].SetActive(false);
        altCanvas[1].SetActive(false);
        altCanvas[2].SetActive(false);
        altCanvas[3].SetActive(false);
        FMODUnity.RuntimeManager.PlayOneShot(selectedBackSound);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void PlayHooverSound()
    {
        Debug.Log("HOOVER");
        FMODUnity.RuntimeManager.PlayOneShot(selectedHooverSound);
    }

    IEnumerator FirstMenuAnim(float delta)
    {
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < 20; i++)
        {
            logo.color = new Color(logo.color.r, logo.color.g, logo.color.b, logo.color.a + 0.05f);
            yield return new WaitForSeconds(0.03f);
        }

        shading = true;

        for (int i = 0; i < 20; i++)
        {
            laser.rectTransform.offsetMax = new Vector2(laser.rectTransform.offsetMax.x + 120.5f, laser.rectTransform.offsetMax.y);
            yield return new WaitForSeconds(0.03f);
        }

        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < btns.Length; i++)
        {
            StartCoroutine(ShowBtn(btns[i], delta));
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator ShowBtn(GameObject btn, float delta)
    {
        float step = delta / 20f;

        for (int i = 0; i < 20; i++)
        {
            btn.transform.localPosition = new Vector3(btn.transform.localPosition.x, btn.transform.localPosition.y + step, btn.transform.localPosition.z);
            yield return new WaitForSeconds(0.02f);
        }
    }

    IEnumerator ShadingAnim()
    {
        for (int i = 0; i < 20; i++)
        {
            logo_hl.color = new Color(logo_hl.color.r, logo_hl.color.g, logo_hl.color.b, logo_hl.color.a + 0.05f);
            yield return new WaitForSeconds(0.08f);
        }

        for (int i = 0; i < 20; i++)
        {
            logo_hl.color = new Color(logo_hl.color.r, logo_hl.color.g, logo_hl.color.b, logo_hl.color.a - 0.05f);
            yield return new WaitForSeconds(0.08f);
        }

        counter = 0;
    }
}
