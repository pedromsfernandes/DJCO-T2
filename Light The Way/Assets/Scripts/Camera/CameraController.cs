using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    float rotX = 0.0f;
    float rotY = 0.0f;

    public float speed = 120.0f;
    public float inputSensitivity = 150.0f;
    public float clampAngle = 80.0f;
    public GameObject follow;
    public GameObject player;

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
}
