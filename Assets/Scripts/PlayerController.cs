using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Transform cameraTransform;

    private Animator playerAnim;

    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;

    private void Start()
    {
        
        cameraTransform = GetComponentInChildren<Camera>().transform;
        playerAnim = GetComponentInChildren<Animator>();

    }

    private void FixedUpdate()
    {
        
        HandleInput();

    }

    private void HandleInput()
    {

        Move();
        Look();

    }

    private void Move()
    {

        Vector3 input = new Vector3
        (

            Input.GetAxis("LeftStickX"),
            0f,
            Input.GetAxis("LeftStickY")

        );

        Vector3.Normalize(input);

        if(input.magnitude > 0f)
        {
            Vector3 movementDirection = transform.TransformDirection(input);
            transform.position += movementDirection * movementSpeed * Time.fixedDeltaTime;

            playerAnim.SetBool("isMoving", true);
        }
        else playerAnim.SetBool("isMoving", false);

    }

    private void Look()
    {

        Vector2 input = new Vector2
        (

            Input.GetAxis("RightStickX"),
            Input.GetAxis("RightStickY")

        );

        input.Normalize();
        input *= rotationSpeed * Time.fixedDeltaTime;

        Vector3 horizontalRotation = new Vector3(0f, input.x, 0f);
        transform.Rotate(horizontalRotation);

        Vector3 verticalRotation =
            cameraTransform.rotation.eulerAngles +
            new Vector3(input.y, 0f, 0f);
        verticalRotation.x = ClampAngle(verticalRotation.x, -30f, 30f);
        cameraTransform.eulerAngles = verticalRotation;

    }

    private float ClampAngle(float angle, float minAngle, float maxAngle)
    {
        if (angle < 0f) angle = 360f + angle;
        if (angle > 180f) return Mathf.Max(angle, 360f + minAngle);
        return Mathf.Min(angle, maxAngle);
    }

}