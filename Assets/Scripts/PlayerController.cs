using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Fields

    [SerializeField] float movementSpeed = 5f;
    [SerializeField] float jumpPower = 5f;
    [SerializeField] int maxJumps = 2;
    int currentJumps;


    Rigidbody2D rb;

    float horizontalInput;

    bool isFloating;
    float defaultGravityScale;
    [SerializeField] float floatingGravityScale = 0.2f;
    [SerializeField] float floatingTime = 3f;
    float floatTimer = 0f;
    [SerializeField] AnimationCurve floatingCurve;

    [SerializeField] Transform groundCheckPosition;
    [SerializeField] float groundCheckRadius = 0.1f;
    [SerializeField] LayerMask groundCheckLayers;

    bool isGrounded => Physics2D.OverlapCircle(new Vector2(transform.position.x,transform.position.y - 0.5f), groundCheckRadius, groundCheckLayers); 

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        // Initialize components.
        rb = GetComponent<Rigidbody2D>();
        defaultGravityScale = rb.gravityScale;
        maxJumps--;
        currentJumps = maxJumps;
    }

    // Update is called once per frame
    void Update()
    {
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
    }

    private void FixedUpdate()
    {
        // Handle player movement.
        rb.AddForce(horizontalInput * movementSpeed * Vector2.right);
    }


    #region InputMethods
    public void HorizontalInput(InputAction.CallbackContext ctx)
    {
        horizontalInput = ctx.ReadValue<float>();
    }

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
    #endregion

    #region UtilityMethods

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        

        if (isFloating)
        {
            rb.AddForce(Vector2.up * jumpPower * floatingGravityScale, ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
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
