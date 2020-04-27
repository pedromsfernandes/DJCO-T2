using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public LightBeam beam;

    void Start()
    {
        Vector3 rot = transform.localRotation.eulerAngles;
        rotX = rot.x;
        rotY = rot.y;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        GetMouseInputs();

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rotX -= mouseY * inputSensitivity * Time.deltaTime;
        rotY += mouseX * inputSensitivity * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

        Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
        transform.rotation = localRotation;
        player.transform.rotation = Quaternion.Euler(0, rotY, 0);
    }

    void LateUpdate()
    {
        Transform target = follow.transform;

        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    }

    void GetMouseInputs()
    {
        if (Input.GetMouseButtonDown(1)) SwitchCameraAim();

        if (Input.GetMouseButtonUp(1)) SwitchCameraNormal();

        if (GameState.Instance.aiming && Input.GetMouseButtonDown(0)) beam.Enable(true);

        if (GameState.Instance.aiming && Input.GetMouseButtonUp(0)) beam.Enable(false);
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
}
