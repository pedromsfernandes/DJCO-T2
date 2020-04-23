using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
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
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        float targetSpeed = 0f;
        bool isWalkingSlow = Input.GetKey(KeyCode.LeftControl);
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        Vector2 movementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

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
        
        if(desiredMoveDirection != Vector3.zero)
        {
            //Rotate player in the desired direction
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), rotationSpeed);
        }

        targetSpeed = movementSpeed * movementInput.magnitude;

        if (isWalkingSlow)
        {
            targetSpeed /= 2;
        }

        if (isRunning)
        {
            targetSpeed *= 2;
        }

        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

        controller.Move(desiredMoveDirection * currentSpeed * Time.deltaTime);
        controller.Move(gravityVector * Time.deltaTime);
    }
}
