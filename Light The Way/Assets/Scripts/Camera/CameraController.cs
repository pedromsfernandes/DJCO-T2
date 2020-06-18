using System.Collections;
using Light;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CameraController : MonoBehaviourPun
{
    float rotX = 0.0f;
    float rotY = 0.0f;

    Coroutine cameraMovement = null;

    public float speed = 120.0f;
    public float inputSensitivity = 150.0f;
    public float clampAngle = 80.0f;
    public int smooth = 10;
    public float animTime = 0.5f;
    public GameObject follow;
    public GameObject player;
    public PlayerBeam beam;

    public Text text;

    void Start()
    {
        Vector3 rot = transform.localRotation.eulerAngles;
        rotX = rot.x;
        rotY = rot.y;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        StartCoroutine("ShowStartMessages");
    }

    void Update()
    {
        if (GameState.Instance.pause)
            return;

        GetMouseInputs();

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rotX -= mouseY * inputSensitivity * Time.deltaTime;
        rotY += mouseX * inputSensitivity * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

        Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
        transform.rotation = localRotation;
        player.transform.rotation = Quaternion.Euler(0, rotY, 0);
        player.transform.Find("Model").Find("CameraSource").rotation = Quaternion.Euler(rotX, rotY, 0);
    }

    void LateUpdate()
    {
        Transform target = follow.transform;

        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    }

    void GetMouseInputs()
    {
        if (Input.GetMouseButtonDown(1))
        {
            SwitchCameraAim();
            player.GetComponent<MovementController>().SetStaffActive(true);
        } 
        else if (Input.GetMouseButtonUp(1))
        {
            SwitchCameraNormal();
            player.GetComponent<MovementController>().SetStaffActive(false);
        }

        if (GameState.Instance.currentTool != 0 && GameState.Instance.aiming && Input.GetMouseButtonDown(0)) beam.Enable(true);

        else if (GameState.Instance.aiming && Input.GetMouseButtonUp(0)) beam.Enable(false);
    }

    void SwitchCameraAim()
    {
        if (cameraMovement != null)
            StopCoroutine(cameraMovement);
        cameraMovement = StartCoroutine(CameraMovementAnim(transform.Find("Main Camera"), new Vector3(1f, 0, -1.5f), true));
    }

    void SwitchCameraNormal()
    {
        if (cameraMovement != null)
            StopCoroutine(cameraMovement);
        cameraMovement = StartCoroutine(CameraMovementAnim(transform.Find("Main Camera"), new Vector3(0, 0, -4f), false));
        beam.Enable(false);
    }

    IEnumerator CameraMovementAnim(Transform camera, Vector3 target, bool aiming)
    {
        if (aiming)
            GameState.Instance.aiming = aiming;

        Vector3 step = (target - camera.localPosition) / smooth;

        for (int i = 0; i < smooth; i++)
        {
            camera.localPosition += step;
            yield return new WaitForSeconds(animTime / smooth);
        }

        if (!aiming)
            GameState.Instance.aiming = aiming;
    }

    IEnumerator ShowStartMessages()
    {
        text.text = "Walk with WASD\nRun with SHIFT";
        text.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);
        text.text = "Aim with RMB\nFire with LMB";
        yield return new WaitForSeconds(5f);
        text.text = "It doesn't seem to work every time\nMaybe you can figure out why...";
        yield return new WaitForSeconds(5f);
        text.gameObject.SetActive(false);
    }
}
