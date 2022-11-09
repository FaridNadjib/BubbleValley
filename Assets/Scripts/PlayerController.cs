using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Fields

    [SerializeField] float movementSpeed = 5f;
    [SerializeField] float jumpPower = 5f;


    Rigidbody2D rb;

    float horizontalInput;
    bool startFloating;

    Vector2 defaultGravity;
    [SerializeField] Vector2 floatingGravity;
    Vector2 currentGravity;


    #endregion


    // Start is called before the first frame update
    void Start()
    {
        // Initialize components.
        rb = GetComponent<Rigidbody2D>();
        currentGravity = defaultGravity;
    }

    // Update is called once per frame
    void Update()
    {
        
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
        //horizontalInput = ctx.ReadValue<float>();
    }

    public void Float(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Performed)
        {
            startFloating = true;
        }
        else if (ctx.phase == InputActionPhase.Canceled)
        {
            startFloating = false;
        }
        
    }
    #endregion
}
