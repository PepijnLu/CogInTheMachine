using System.Collections;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    float mouseX, mouseY, moveX,  moveZ;
    private Rigidbody rb;
    private float verticalLookRotation;
    public Transform playerCamera;
    public float moveSpeed;
    public float mouseSensitivity;

    void Start()
    {
        // Lock the cursor to the center of the screen and make it invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Handle mouse look
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);
        verticalLookRotation -= mouseY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);
        playerCamera.localEulerAngles = Vector3.right * verticalLookRotation;

        // Handle movement
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        Vector3 move;
        move = (transform.right * moveX + transform.forward * moveZ).normalized * moveSpeed * Time.deltaTime;
    }
}