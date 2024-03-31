using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Rigidbody rb_player;
    [SerializeField] private Transform platform;

    [Header("Enabled Mechanics")]
    [SerializeField] private bool freeze = false;
    [SerializeField] private bool enableGravity = true;
    [SerializeField] private bool enableMove = true;
    [SerializeField] private bool enableCameraLook = true;
    [SerializeField] private bool enableSprint = true;
    [SerializeField] private bool enableAirAccel = false;
    [SerializeField] private bool enableJump = true;
    [SerializeField] private bool enableJumpTime = false;

    [Header("Movement Mechanics")]
    [SerializeField] private float gravity = 25f;
    private Vector3 temp;
    private Vector3 gravityVel;
    [SerializeField] private bool isMoving = false;
    public bool IsMoving { get { return isMoving; } }
    [SerializeField] private float playerSpeed = 5f;
    [SerializeField] private float jumpForce = 15f;

    [Header("Mouse Mechanics")]
    private Vector2 mouseInput = Vector2.zero;
    [SerializeField] private float clampUp = -70f;
    [SerializeField] private float clampDown = 30f;
    [SerializeField] private float mouseSensitivity = 1f;

    [Header("Jump Mechanics")]
    [SerializeField] private Transform groundDectector; // *Required, "Base/Feet"
    [SerializeField] private LayerMask layerMask; // Standable Layer
    [SerializeField] private bool isGrounded;
    private bool isJumping;
    [SerializeField] private float maxJumpTime = 0.2f;
    private float jumpTime;

    //public int maxJump; // Default = 1, Double = 2, etc.
    //private int jumpCount;

    [Header("Sprint Feature")]
    [SerializeField] private float sprintSpeed = 6f;
    [SerializeField] private float walkSpeed;

    [Header("Air Acceleration Modifiers")]
    [SerializeField] private float defaultAccel = 1; // Default 1
    [SerializeField] private float lightAccel = 0.8f; // Proper strafe
    [SerializeField] private float heavyAccel = 0.3f; // Restricted strafe
    private float currentAccel = 1f;
    private bool breakLightAccel;

    [Header("Teleport Features")]
    public float teleportDelay = 0.5f;
    public float teleportCooldown = 0f;

    private void Awake()
    {
        Initalize();
    }

    private void Start()
    {
        CheckDeclare();
    }

    void Update()
    {
        if (freeze) return;

        CheckMoving();
        if (enableGravity) Gravity();
        if (enableMove) Movement();
        if (enableSprint) Sprint();
        if (enableJump) Jump();
        if (enableCameraLook) CameraLook();
    }

    private void Gravity()
    {
        // Ground Check
        isGrounded = Physics.CheckSphere(groundDectector.position, 0.2f, layerMask);

        // Reset variables on land
        if (isGrounded)
        {
            isJumping = false;
        }

        // Gravity
        if (!isGrounded)
        {
            isJumping = true;
            gravityVel += gravity * -player.up * Time.deltaTime;
        }
        //else
        //{
        //    gravityVel = Vector3.zero;
        //}

        Vector3 velocity = temp * playerSpeed * currentAccel;

        rb_player.velocity = velocity + (gravityVel);
    }

    private void Movement()
    {
        // Air Accel Calculation
        if (enableAirAccel) AirAccel();

        // WASD Velocity
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputZ = Input.GetAxisRaw("Vertical");

        temp = Vector3.zero;
        if (inputZ > 0)
        {
            temp += player.forward;
        }
        if (inputZ < 0)
        {
            temp += -player.forward;
        }
        if (inputX > 0)
        {
            temp += player.right;
        }
        if (inputX < 0)
        {
            temp += -player.right;
        }
    }

    private void CheckMoving() 
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputZ = Input.GetAxisRaw("Vertical");

        if (inputZ > 0 || inputZ < 0 || inputX > 0 || inputX < 0 || isJumping)
        {
            isMoving = true;
        }

        else
        {
            isMoving = false;
        }
    }

    private void CameraLook()
    {
        // Camera
        mouseInput.x = Input.GetAxis("Mouse X") * mouseSensitivity;
        if (mouseInput.y < clampUp) mouseInput.y = clampUp;
        if (mouseInput.y > clampDown) mouseInput.y = clampDown;
        mouseInput.y += -Input.GetAxis("Mouse Y") * mouseSensitivity;
        player.Rotate(0f, mouseInput.x, 0f);
        Camera.main.transform.localRotation = Quaternion.Euler(mouseInput.y, 0f, 0f);
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Sprint()
    {
        // Sprint
        if (enableSprint)
        {
            if (isGrounded && Input.GetKey(KeyCode.LeftShift))
            {
                playerSpeed = sprintSpeed;
            }
            else
            {
                playerSpeed = walkSpeed;
            }
        }
    }

    private void Jump()
    {
        // Jump
        if (enableJump)
        {
            // Jump Time
            if (enableJumpTime)
            {
                if (isGrounded && Input.GetKeyDown(KeyCode.Space))
                {
                    isJumping = true;
                    jumpTime = 0f;
                }
                if (isJumping)
                {
                    rb_player.AddForce(rb_player.transform.up * jumpForce * Time.deltaTime, ForceMode.Acceleration);
                    jumpTime += Time.deltaTime;
                }
                if (Input.GetKeyUp(KeyCode.Space) || jumpTime >= maxJumpTime)
                {
                    isJumping = false;
                }
            }

            else if (isGrounded && Input.GetKeyDown(KeyCode.Space))
            {
                gravityVel += player.up * jumpForce;
            }

        }
    }

    private void AirAccel()
    {
        if (Input.GetAxis("Vertical") != 0) breakLightAccel = true; // Whatever condition - e.g. If forward/back input, restricted accel until reset

        if (!isGrounded)
        {
            if (breakLightAccel)
            {
                currentAccel = heavyAccel;
            }
            else
            {
                currentAccel = lightAccel;
            }
        }
        else
        {
            currentAccel = defaultAccel;
            breakLightAccel = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == layerMask)
        {
            platform = collision.transform;
            gravityVel = Vector3.zero;
        }
    }

    private void Initalize()
    {
        player = this.gameObject.transform;
        rb_player = this.gameObject.GetComponent<Rigidbody>();
    }

    private void CheckDeclare()
    {
        if (rb_player == null)
        {
            rb_player = GetComponent<Rigidbody>();
            Debug.Log("Character controller not assigned. Defaulted to player, errors may persist.");
        }

        if (groundDectector == null)
        {
            groundDectector = gameObject.transform;
            Debug.Log("Ground detector not assigned. Defaulted to player, errors may persist.");
        }

        if (enableJumpTime) enableJump = true;
        if (!enableAirAccel) currentAccel = defaultAccel;

        if (enableSprint)
        {
            enableMove = true;
            sprintSpeed = playerSpeed * 1.3f;
            walkSpeed = playerSpeed;
        }
    }
}
