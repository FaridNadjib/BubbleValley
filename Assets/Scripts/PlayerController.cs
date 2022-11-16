using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Fields

    [Header("Basic movement related:")]
    [SerializeField] float movementSpeed = 5f;
    [SerializeField] float jumpPower = 5f;
    [SerializeField] int maxJumps = 2;
    int currentJumps;

    Rigidbody2D rb;

    [Header("Floating related:")]
    [SerializeField] float floatingGravityScale = 0.2f;
    [SerializeField] float floatingTime = 3f;
    bool isFloating;
    float defaultGravityScale;
    float floatTimer = 0f;

    [Header("Ground check related:")]
    [SerializeField] Transform groundCheckPosition;
    [SerializeField] float groundCheckRadius = 0.1f;
    [SerializeField] LayerMask groundCheckLayers;
    bool isGrounded => Physics2D.OverlapCircle(new Vector2(transform.position.x,transform.position.y - 0.5f), groundCheckRadius, groundCheckLayers);

    [Header("Power charge related:")]
    [SerializeField] float powerChargePowerMultiplier = 5f;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] float maxChargeTime = 3f;
    bool isPowerCharging;
    float chargeTimer;
    

    
    [SerializeField] AnimationCurve floatingCurve;

    // Input action asset.
    PlayerInput playerInput;
    InputAction movementInput;
    float startTime = 0;


    #endregion


    // Start is called before the first frame update
    void Start()
    {
        // Initialize components.
        rb = GetComponent<Rigidbody2D>();
        defaultGravityScale = rb.gravityScale;

        // Todo: fix maxjumps.
        maxJumps--;
        currentJumps = maxJumps;

        // Input action related.
        playerInput = GetComponent<PlayerInput>();
        movementInput = playerInput.actions.FindAction("MovementInput");
        movementInput.Enable();

    }

    // Update is called once per frame
    void Update()
    {
        // Reset current jumps if the character touches the ground.
        currentJumps = isGrounded ? maxJumps : currentJumps;

        // Handle floating mechanic.
        if (isFloating)
        {
            floatTimer += Time.deltaTime;
            if (floatTimer >= floatingTime)
            {
                StopFloating();
            }            
        }

        //Debug.Log(movementInput.ReadValue<Vector2>());

        // Handle power charge.
        if (isPowerCharging)
        {
            if (chargeTimer != 0)
            {
                rb.AddForce(Vector2.up * chargeTimer / maxChargeTime * powerChargePowerMultiplier);
                isPowerCharging = false;
                chargeTimer = 0f;
            }
            
        }
    }

    private void FixedUpdate()
    {

        // Handle player movement.
        rb.AddForce(movementInput.ReadValue<Vector2>().x * movementSpeed * Vector2.right);
    }


    #region InputMethods
    public void Jump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && currentJumps > 0)
        {
            Jump();
        }
    }

    public void Float(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            StartFloating();
        }
        else if (ctx.canceled)
        {
            StopFloating();
        }
        
    }


    public void PowerCharge(InputAction.CallbackContext ctx)
    {
        
        if (ctx.performed)
        {
            isPowerCharging = true;
            startTime = (float)ctx.time;
        }
        else if (ctx.canceled)
        {
            //isPowerCharging = false;
            chargeTimer = (float)(ctx.time - startTime);
            Math.Clamp(chargeTimer, 0, maxChargeTime);
            Debug.Log(chargeTimer);
        }
    }      
    #endregion

    #region UtilityMethods

    private void Jump()
    {
        // Todo: eventually fix zero and add jumpower based on button press length.
        rb.velocity = new Vector2(rb.velocity.x, 0f);    

        if (isFloating)
        {
            rb.AddForce(jumpPower * floatingGravityScale * Vector2.up, ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(jumpPower * defaultGravityScale * Vector2.up, ForceMode2D.Impulse);
        }

        currentJumps--;
        // ToDo: Enable particles and sound.
    }

    private void StartFloating()
    {
        isFloating = true;
        rb.gravityScale = floatingGravityScale;

        // ToDo: Enable floating particles and sound. Modify horizontal movement speed.
    }
    private void StopFloating()
    {
        isFloating = false;
        floatTimer = 0f;
        rb.gravityScale = defaultGravityScale;
    }

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(new Vector2(transform.position.x, transform.position.y - 0.5f), groundCheckRadius);
    }
}
