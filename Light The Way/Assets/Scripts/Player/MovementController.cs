using Light;
using UnityEngine;
using Photon.Pun;
using LightType = Light.LightType;

public class MovementController : MonoBehaviourPun
{
    [SerializeField] private float movementSpeed = 5f;
    private float currentSpeed = 0f;
    private float speedSmoothVelocity = 0f;
    private float speedSmoothTime = 0.1f;
    private float rotationSpeed = 0.1f;
    [SerializeField] private float gravity = 10f;

    private Transform mainCameraTransform;

    private CharacterController controller;

    private Rigidbody playerRigidbody;

    private GameObject idle;
    private GameObject walking;
    private GameObject running;

    void Start()
    {
        playerRigidbody = GetComponentInChildren<Rigidbody>();

        controller = GetComponent<CharacterController>();

        idle = transform.Find("Idle").gameObject;
        walking = transform.Find("Walking").gameObject;
        running = transform.Find("Running").gameObject;

        mainCameraTransform = Camera.main.transform;
        transform.Find("Laser").gameObject.GetComponent<PlayerBeam>().camera = transform.Find("Model").Find("CameraSource").gameObject;
        if (GetComponent<PhotonView>() == null || GetComponent<PhotonView>().IsMine)
        {
            mainCameraTransform.gameObject.GetComponentInParent<CameraController>().follow = transform.Find("Model").Find("CameraFollow").gameObject;
            mainCameraTransform.gameObject.GetComponentInParent<CameraController>().player = this.gameObject;
            mainCameraTransform.gameObject.GetComponentInParent<CameraController>().beam = transform.Find("Laser").gameObject.GetComponent<PlayerBeam>();
        }

        mainCameraTransform.parent.transform.position = transform.position;
    }

    void Update()
    {
        if ((GetComponent<PhotonView>() == null || GetComponent<PhotonView>().IsMine) && !GameState.Instance.pause)
            Move();
    }

    private void Move()
    {
        // change laser color - temporary

        bool isWalkingSlow = Input.GetKey(KeyCode.LeftControl);
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        Vector2 movementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        float targetSpeed;

        if(movementInput.x == 0 && movementInput.y == 0)
        {
            idle.SetActive(true);
            walking.SetActive(false);
            running.SetActive(false);
        }
        else if(isRunning)
        {
            idle.SetActive(false);
            walking.SetActive(false);
            running.SetActive(true);
        }
        else
        {
            idle.SetActive(false);
            walking.SetActive(true);
            running.SetActive(false);
        }

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

        //Velocity
        targetSpeed = movementSpeed * movementInput.magnitude;

        if(targetSpeed != 0)
        {
            GameState.Instance.moving = true;
        }
        else
        {
            GameState.Instance.moving = false;
        }


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
        Vector3 desiredMove = desiredMoveDirection * (currentSpeed * Time.deltaTime);
        Vector3 gravityMove = gravityVector * Time.deltaTime;

        controller.Move(desiredMove);
        controller.Move(gravityMove);

        //Update Rigidbody Velocity
        //playerRigidbody.velocity = desiredMove;
    }
}
