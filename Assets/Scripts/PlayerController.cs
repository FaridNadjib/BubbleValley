using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Fields

    [Header("Basic movement related:")]
    [SerializeField] float maxSpeed = 7f;
    [SerializeField] float movementSpeed = 7f;
    [SerializeField] float jumpPower = 5f;
    [SerializeField] int maxJumps = 2;
    public int currentJumps;

    Rigidbody2D rb;

    [Header("Floating related:")]
    [SerializeField] float floatingSpeed = 4f;
    [SerializeField] float floatingGravityScale = 0.2f;
    [SerializeField] float maxFloatingTime = 3f;
    [SerializeField] ParticleSystem floatingPlayerPS;
    bool isFloating;
    bool canFloat = true;
    float defaultGravityScale;
    float floatTimer = 0f;
    Vector2 tmpVelocity;

    [Header("Ground check related:")]
    [SerializeField] float groundCheckRadius = 0.1f;
    [SerializeField] LayerMask groundCheckLayers;
    Vector3 groundCheckPosition;
    bool isGrounded => Physics2D.OverlapCircle(groundCheckPosition, groundCheckRadius, groundCheckLayers);


    [Header("Power charge related:")]
    [SerializeField] float powerChargePowerMultiplier = 5f;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] float maxChargeTime = 3f;
    bool isPowerCharging;
    bool canceledPowerCharge;
    float chargeTimer;
    
    [SerializeField] AnimationCurve floatingCurve;

    // Input action asset.
    PlayerInput playerInput;
    InputAction movementInput;

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        // Initialize components.
        rb = GetComponent<Rigidbody2D>();
        defaultGravityScale = rb.gravityScale;

        maxSpeed = Mathf.Pow(maxSpeed, 2);
        currentJumps = maxJumps;
        lineRenderer.enabled = false;

        // Input action related.
        playerInput = GetComponent<PlayerInput>();
        movementInput = playerInput.actions.FindAction("Movement");
        movementInput.Enable();

    }

    // Update is called once per frame
    void Update()
    {
        // Reset current jumps if the character touches the ground.

        // Handle floating mechanic.
        if (isFloating)
        {
            floatTimer += Time.deltaTime;
            if(floatTimer<= 0.53f)
            {
                rb.velocity = Mathf.SmoothStep(defaultGravityScale, floatingGravityScale, Mathf.Clamp(floatTimer * 2, 0, 1f)) * tmpVelocity;
            }           
            if (floatTimer >= maxFloatingTime)
            {
                StopFloating();
            }            
        }

        //Debug.Log(movementInput.ReadValue<Vector2>());

        // Handle power charge.
        if (isPowerCharging)
        {
            chargeTimer += Time.deltaTime;

            Vector2 tmpPosition = transform.position;
            lineRenderer.SetPosition(0, tmpPosition + movementInput.ReadValue<Vector2>());
            lineRenderer.SetPosition(1, tmpPosition);
            
            if (chargeTimer >= maxChargeTime || canceledPowerCharge && chargeTimer > maxChargeTime * 0.2f)
            {
                // ToDo: eventually normalize the input.
                if (movementInput.ReadValue<Vector2>() != Vector2.zero)
                    rb.AddForce((chargeTimer / maxChargeTime) * powerChargePowerMultiplier * -1 * movementInput.ReadValue<Vector2>().normalized, ForceMode2D.Impulse);
                
                StopPowerCharging();
            }
            else if (canceledPowerCharge)
            {
                StopPowerCharging();
            }
        }

    }

    private void FixedUpdate()
    {
        // ToDo: better or no clamp.
        if (!isPowerCharging && rb.velocity.sqrMagnitude < maxSpeed)
        {
            // Handle player movement.
            if (isFloating)
            {
                rb.AddForce(movementInput.ReadValue<Vector2>().normalized * floatingSpeed);
            }
            else
            {
                rb.AddForce(movementInput.ReadValue<Vector2>().x * movementSpeed * Vector2.right);
            } 
        }
        
        // Jump handling.
    }


    #region InputMethods
    public void Jump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && currentJumps > 0)
        {
            //ToDo: decide if no jumping while floating.
            StopFloating();
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
            StopFloating();
            canFloat = false;
            isPowerCharging = true;
            rb.velocity *= 0.1f;
            rb.gravityScale = 0.01f;
            lineRenderer.enabled = true;
        }
        else if (ctx.canceled)
        {
            canceledPowerCharge = isPowerCharging ? true: false;
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
        if (canFloat)
        {
            isFloating = true;
            tmpVelocity = rb.velocity;
            rb.gravityScale = floatingGravityScale;
            //rb.velocity = rb.velocity * floatingGravityScale;

            // ToDo: Enable floating particles and sound. Modify horizontal movement speed. 
            floatingPlayerPS.Clear();
            floatingPlayerPS.Play();
        }
    }
    private void StopFloating()
    {
        isFloating = false;
        floatTimer = 0f;
        if (!isPowerCharging)
            rb.gravityScale = defaultGravityScale;


        // Deactivate the visual stuff.
        floatingPlayerPS.Stop();
        //floatingPlayerPS.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }


    private void StopPowerCharging()
    {
        isPowerCharging = false;
        chargeTimer = 0f;
        canceledPowerCharge = false;
        canFloat = true;
        rb.gravityScale = defaultGravityScale;
        lineRenderer.enabled = false;
    }

    #endregion


    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Stop floating.
        StopFloating();

        // Handle groundchecks.
        groundCheckPosition = collision.GetContact(0).point;
        // ToDo: extra method for this?
        // Update current number of jumps. Jumping on a bubble from above gives back 1 jump. On the ground restores all jumps.
        if (collision.gameObject.CompareTag("Bubble"))
        {
            if (Vector2.Dot(Vector2.up, (groundCheckPosition - transform.position).normalized) < -0.5f)
            {
                currentJumps = currentJumps < maxJumps ? currentJumps + 1 : currentJumps;
            }
        }
        else
        {
            if (Vector2.Dot(Vector2.up, (groundCheckPosition - transform.position).normalized) < -0.5f)
            {
                currentJumps = isGrounded ? maxJumps : currentJumps;
            }         
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(groundCheckPosition, groundCheckRadius);

    }
}
