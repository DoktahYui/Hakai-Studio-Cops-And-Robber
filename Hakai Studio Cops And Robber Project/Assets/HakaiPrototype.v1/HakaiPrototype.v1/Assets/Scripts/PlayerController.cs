using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform player;
    public Rigidbody rb_player;
    public Transform platform;

    [Header("Enabled Mechanics")]
    public bool enableMove = true;
    public bool enableSprint = true;
    public bool enableAirAccel = false;
    public bool enableJump = true;
    public bool enableJumpTime = false;

    [Header("Movement Mechanics")]
    public float gravity = 25f;
    private Vector3 gravityVel;
    public float playerSpeed = 5f;
    public float jumpForce = 15f;

    [Header("Mouse Mechanics")]
    private Vector2 mouseInput = Vector2.zero;
    public float clampUp = -70f;
    public float clampDown = 30f;
    public float mouseSensitivity = 1f;

    [Header("Jump Mechanics")]
    public Transform groundDectector; // *Required, "Base/Feet"
    public LayerMask layerMask; // Standable Layer
    [SerializeField] private bool isGrounded;
    private bool isJumping;
    public float maxJumpTime = 0.2f;
    private float jumpTime;

    //public int maxJump; // Default = 1, Double = 2, etc.
    //private int jumpCount;

    [Header("Sprint Feature")]
    public float sprintSpeed = 6f;
    public float walkSpeed;

    [Header("Air Acceleration Modifiers")]
    public float defaultAccel = 1; // Default 1
    public float lightAccel = 0.8f; // Proper strafe
    public float heavyAccel = 0.3f; // Restricted strafe
    private float currentAccel = 1f;
    private bool breakLightAccel;

    [Header("Teleport Features")]
    public float teleportDelay = 0.5f;
    public float teleportCooldown = 0f;

    private void Awake()
    {
        player = transform;
        rb_player = player.GetComponent<Rigidbody>();
    }

    private void Start()
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
            sprintSpeed = playerSpeed * 1.3f;
            walkSpeed = playerSpeed;
        }
    }

    void Update()
    {
        if (enableMove) Movement();
    }

    #region MOVEMENT_CONTROLS

    private void Movement()
    {
        // Ground Check
        isGrounded = Physics.CheckSphere(groundDectector.position, 0.1f, layerMask);

        // Reset variables on land
        if (isGrounded)
        {
            isJumping = false;
        }

        // Air Accel Calculation
        if (enableAirAccel) AirAccel();

        #region XYZ Velocity
        // WASD Velocity
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputZ = Input.GetAxisRaw("Vertical");

        Vector3 temp = Vector3.zero;
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
                isGrounded = false;
            } 
        }

        // Gravity
        if (!isGrounded)
        {
            gravityVel += gravity * -player.up * Time.deltaTime;
        }
        else
        {
            gravityVel = Vector3.zero;
        }

        Vector3 velocity = temp * playerSpeed * currentAccel;

        rb_player.velocity = velocity + (gravityVel);

        #endregion

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

        // Camera
        mouseInput.x = Input.GetAxis("Mouse X") * mouseSensitivity;
        if (mouseInput.y < clampUp) mouseInput.y = clampUp;
        if (mouseInput.y > clampDown) mouseInput.y = clampDown;
        mouseInput.y += -Input.GetAxis("Mouse Y") * mouseSensitivity;
        player.Rotate(0f, mouseInput.x, 0f);
        Camera.main.transform.localRotation = Quaternion.Euler(mouseInput.y, 0f, 0f);
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
        }
    }

    #endregion
}
