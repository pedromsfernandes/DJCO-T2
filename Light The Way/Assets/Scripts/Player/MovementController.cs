using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MovementController : MonoBehaviourPun
{
    [SerializeField] private float movementSpeed = 5f;
    private float currentSpeed = 0f;
    private float speedSmoothVelocity = 0f;
    private float speedSmoothTime = 0.1f;
    private float rotationSpeed = 0.1f;
    private float gravity = 10f;

    private Transform mainCameraTransform;

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        mainCameraTransform = Camera.main.transform;
        if (GetComponent<PhotonView>() == null || GetComponent<PhotonView>().IsMine)
        {
            transform.Find("Laser").gameObject.GetComponent<LightBeam>().camera = mainCameraTransform.gameObject;
            mainCameraTransform.gameObject.GetComponentInParent<CameraController>().follow = transform.Find("Model").Find("CameraFollow").gameObject;
            mainCameraTransform.gameObject.GetComponentInParent<CameraController>().player = this.gameObject;
            mainCameraTransform.gameObject.GetComponentInParent<CameraController>().beam = transform.Find("Laser").gameObject.GetComponent<LightBeam>();
        }
    }

    void Update()
    {
        if (GetComponent<PhotonView>() == null || GetComponent<PhotonView>().IsMine)
            Move();
    }

    private void Move()
    {
        bool isWalkingSlow = Input.GetKey(KeyCode.LeftControl);
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        Vector2 movementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        float targetSpeed;

        //Update GameState
        GameState.Instance.walkingSlow = isWalkingSlow;
        GameState.Instance.running = isRunning;

        //Get Direction
        Vector3 forward = mainCameraTransform.forward;
        Vector3 right = mainCameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 desiredMoveDirection = (forward * movementInput.y + right * movementInput.x).normalized;

        //Gravity
        Vector3 gravityVector = Vector3.zero;

        if (!controller.isGrounded)
        {
            gravityVector.y -= gravity;
        }

        if (desiredMoveDirection != Vector3.zero)
        {
            //Rotate player in the desired direction
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), rotationSpeed);
        }

        //Velocity
        targetSpeed = movementSpeed * movementInput.magnitude;

        if (isWalkingSlow)
        {
            targetSpeed /= 2;
        }

        if (isRunning && !GameState.Instance.aiming)
        {
            targetSpeed *= 2;
        }

        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

        //Apply changes
        controller.Move(desiredMoveDirection * currentSpeed * Time.deltaTime);
        controller.Move(gravityVector * Time.deltaTime);
    }
}
