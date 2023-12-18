// CHANGE LOG
// 
// CHANGES || version VERSION
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
    using UnityEditor;
    using System.Net;
#endif

public class FirstPersonController : MonoBehaviour
{
    private Rigidbody rb;

    // Camera Movement Variables

    public Camera playerCamera;

    public float fov = 60f;
    public bool invertCamera = false;
    public bool cameraCanMove = true;
    public float mouseSensitivity = 2f;
    public float maxLookAngle = 50f;

    // Internal Variables
    private float yaw = 0.0f;
    private float pitch = 0.0f;

    // Movement Variables

    public bool playerCanMove = true;
    public float walkSpeed = 5f;
    public float maxVelocityChange = 10f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        playerCamera = GetComponentInChildren<Camera>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // Control camera movement
        if(cameraCanMove)
        {
            yaw = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivity;

            if (!invertCamera)
            {
                pitch -= mouseSensitivity * Input.GetAxis("Mouse Y");
            }
            else
            {
                // Inverted Y
                pitch += mouseSensitivity * Input.GetAxis("Mouse Y");
            }

            // Clamp pitch between lookAngle
            pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);

            transform.localEulerAngles = new Vector3(0, yaw, 0);
            playerCamera.transform.localEulerAngles = new Vector3(pitch, 0, 0);
        }

        //click objects
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = playerCamera.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // Use the hit variable to determine what was clicked on.
                Debug.Log("click");
            }
        }
    }

    void FixedUpdate()
    {
        if (playerCanMove)
        {
            // Calculate how fast we should be moving
            Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            // All movement calculations while walking

            targetVelocity = transform.TransformDirection(targetVelocity) * walkSpeed;

            // Apply a force that attempts to reach our target velocity
            Vector3 velocity = rb.velocity;
            Vector3 velocityChange = (targetVelocity - velocity);
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            velocityChange.y = 0;

            rb.AddForce(velocityChange, ForceMode.VelocityChange);
        }
    }
}

