using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player_Controller : MonoBehaviour
{
    public int health = 10;
    public float speed = 15f;
    public float gravity = -9.81f;
    public float jumpHeight = 3;
    public Transform groundCheck;
    public float groundDistance = 0.1f;
    public LayerMask groundMask;
    public float mouseSensitivity = 100f;
    public Transform CameraHolder;
    public TextMeshProUGUI HealthUI;

    private static bool isGrounded;
    private Vector3 Velocity;
    private CharacterController controller;
    private float xRotation = 0f;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        HealthUI.text = "Health: " + health;
        PlayerMovement();
        PlayerRotation();
    }

    void PlayerMovement()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && Velocity.y < 0)
        {
            Velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed = speed + 6;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = speed - 6;
        }
        Velocity.y += gravity * Time.deltaTime;
        controller.Move(Velocity * Time.deltaTime);
    }

    void PlayerRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -50f, 50f);
        CameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        this.transform.Rotate(Vector3.up * mouseX);

    }
}