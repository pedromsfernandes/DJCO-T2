using System.Collections;
using Light;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class SceneCameraController : MonoBehaviourPun
{
    public float speed = 120.0f;
    public float inputSensitivity = 150.0f;
    public float clampAngle = 80.0f;
    public int smooth = 10;
    public float animTime = 0.5f;
    public GameObject follow;
    public GameObject target;

    GameObject camera;

    public Text text;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        camera = transform.Find("Main Camera").gameObject;
    }

    void Update()
    {
        if (follow != null)
        {
            Transform dir = follow.transform;

            float step = speed * Time.deltaTime;
            camera.transform.position = Vector3.MoveTowards(camera.transform.position, dir.position, step);
        }

        if (target != null)
        {
            var rotation = Quaternion.LookRotation(target.transform.position - camera.transform.position);
            camera.transform.rotation = Quaternion.Slerp(camera.transform.rotation, rotation, 0.01f);
        }
    }
}
