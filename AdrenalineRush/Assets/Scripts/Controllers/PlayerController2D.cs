using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController2D : MonoBehaviour
{
    private new Rigidbody2D rigidbody;  // Movement is set with rigidbody.velocity
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private PlayerControls controls;    // New input system action map

    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private ParticleSystem footstepsParticles;
    [SerializeField] private ParticleSystem groundImpactParticles;

    // Movement values
    [SerializeField] private float movementSpeed = 7f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float hangTime = .05f;         // Hang time (coyote effect)
    private float hangCounter;
    [SerializeField] private float jumpBufferTime = .1f;    // Expanded time slot to let the player jump before landing
    private float jumpBufferCounter;
    [SerializeField] private float lookAtDistance = 3.5f;  // Distance that the target of the camera moves on input

    private float lookVertical;         // Vertical axis (left stick) value
    private float moveHorizontal;       // Horizontal axis (left stick) value
    private int facingDirection = 1;    // Facing direction of the player (1: right, -1: left)

    private float groundCheckCircleRadius;  // Radius of CircleCollider2D that checks if player is grounded
    private bool isGrounded;

    private ParticleSystem.EmissionModule footstepsEmission;
    private bool wasOnGround;   // Last frame's isGrounded value

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        controls = new PlayerControls();
        groundCheckCircleRadius = groundCheck.GetComponent<CircleCollider2D>().radius;
        footstepsEmission = footstepsParticles.emission;
    }

    private void Update()
    {
        // Check if player is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckCircleRadius, whatIsGround);  // Check every frame if player is grounded
        animator.SetBool("Grounded", isGrounded);

        // Reset camera target position if player is moving or not grounded
        if(moveHorizontal != 0 || !isGrounded) {
            cameraTarget.localPosition = Vector2.zero;
        }

        #region Jumps

        // Manage hang time (Coyote Effect)
        if(isGrounded) {
            hangCounter = hangTime;
        }
        else {
            hangCounter -= Time.deltaTime;
        }

        // Manage jump buffer counter
        if(jumpBufferCounter > 0) {
            jumpBufferCounter -= Time.deltaTime;
        }

        // Manage jumps
        if(jumpBufferCounter > 0 && hangCounter > 0) {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpForce);
            jumpBufferCounter = 0;
            groundImpactParticles.Stop();
        }

        #endregion

        // Manage player horizontal velocity (mantain y velocity and modify x velocity)
        rigidbody.velocity = new Vector2(moveHorizontal * movementSpeed, rigidbody.velocity.y);
        
        // Manage animator variables
        animator.SetFloat("Horizontal speed", Mathf.Abs(moveHorizontal));
        animator.SetFloat("Y velocity", rigidbody.velocity.y);

        #region Particle systems

        // Manage footsteps particle system
        if(moveHorizontal != 0 && isGrounded) {
            footstepsEmission.rateOverTime = 20f;
        }
        else {
            footstepsEmission.rateOverTime = 0f;
        }

        // Manage ground impact particle system
        if(!wasOnGround && isGrounded) {
            if(groundImpactParticles.gameObject.activeInHierarchy) {
                groundImpactParticles.Stop();
            }
            groundImpactParticles.gameObject.SetActive(true);
            groundImpactParticles.Play();
        }
        wasOnGround = isGrounded;   // wasOnGround is the last frame's grounded status

        #endregion

    }

    public void Move(InputAction.CallbackContext context)
    {
        moveHorizontal = context.ReadValue<Vector2>().x;    // moveHorizontal changes value on every input on the horizontal axis
        
        if(moveHorizontal > -0.4f && moveHorizontal < 0.4f)
            moveHorizontal = 0;

        if(moveHorizontal > 0) {
            if(facingDirection == -1) {
                transform.Rotate(0, 180, 0);
                facingDirection = 1;
            }
        }
        else if(moveHorizontal < 0) {
            if(facingDirection == 1) {
                transform.Rotate(0, 180, 0);
                facingDirection = -1;
            }
        }
    }

    /**
     * The jump button interaction is the default interaction. 
     * When the button is pressed, the action is performed, when it's released the action is canceled.
     * If the player's velocity is positive and the action is canceled, the vertical velocity is decreased to make smaller jumps.
     * 
     * OLD REMINDER: If the TapInteraction is started but not performed, it is automatically canceled.
     */

    public void Jump(InputAction.CallbackContext context)
    {
        // Reset jump buffer counter
        if(context.performed) {
            jumpBufferCounter = jumpBufferTime;
        }

        // Small jumps based on jump button release
        if(context.canceled && rigidbody.velocity.y > 0) {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, rigidbody.velocity.y * .3f);
        }

    }

    /**
     * On left stick held down move the camera target down, on release move it back up
     */
    public void MoveCameraVertical(InputAction.CallbackContext context)
    {
        if(moveHorizontal == 0 && isGrounded && context.performed) {
            lookVertical = context.ReadValue<Vector2>().y;

            if(lookVertical > 0) {
                cameraTarget.localPosition = new Vector2(0, lookAtDistance);
            }
            else if(lookVertical < 0) {
                cameraTarget.localPosition = new Vector2(0, -lookAtDistance);
            }
        }
        else {
            cameraTarget.localPosition = Vector2.zero;
        }
    }
}


/**
 * Gamepad class use, example with LookUp & LookDown method
 * 

    private Gamepad gamepad;

    private float moveCameraHoldDuration = .4f; // Time in seconds to hold the lookUp/Down key to move the camera target
    private float moveCameraBuffer;

    if(Gamepad.current != null) {
            gamepad = Gamepad.current;
    }


// Accessing gamepad input directly, in gamepad not saved search for a connected gamepad and save it
if(gamepad != null) {

    // Look up & down (move camera target when left stick is held up or down)
    if(moveHorizontal == 0 && isGrounded) {

        lookVertical = gamepad.leftStick.y.ReadValue();

        /*
        if(moveCameraBuffer >= moveCameraHoldDuration) {
            if(lookVertical >= 0.5f) {
                cameraTarget.localPosition = new Vector2(0, lookAtDistance);
            }
            else if(lookVertical <= -0.5f) {
                cameraTarget.localPosition = new Vector2(0, -lookAtDistance);
            }
        }
        else if(lookVertical >= 0.5f || lookVertical <= -0.5f) {
            moveCameraBuffer += Time.deltaTime;
        }
        else if(lookVertical > -0.5f && lookVertical < 0.5f) {
            cameraTarget.localPosition = Vector2.zero;
            moveCameraBuffer = 0;
        }

        */

/* right part

if(lookVertical < -0.5f) {
    if(moveCameraBuffer >= moveCameraHoldDuration) {
        cameraTarget.localPosition = new Vector2(0, -lookAtDistance);
    }
    else {
        moveCameraBuffer += Time.deltaTime;
    }
}
else if(lookVertical > 0.5f) {
    if(moveCameraBuffer >= moveCameraHoldDuration) {
        cameraTarget.localPosition = new Vector2(0, lookAtDistance);
    }
    else {
        moveCameraBuffer += Time.deltaTime;
    }
}
else {
    cameraTarget.localPosition = Vector2.zero;
    moveCameraBuffer = 0;
}
}
else {
cameraTarget.localPosition = Vector2.zero;
moveCameraBuffer = 0;
}
}
else if(Gamepad.current != null) {
gamepad = Gamepad.current;
}

*/