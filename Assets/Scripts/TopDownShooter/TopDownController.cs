using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TopDownController : MonoBehaviour
{
    PlayerInput playerInput;
    InputAction movementInput;
    InputAction mousePosition;
    InputAction aimInput;

    Rigidbody2D rb;

    [SerializeField] float acceleration = 5f;
    [SerializeField] float maxSpeed = 7f;
    [SerializeField] float rotationSpeed = 45f;

    [SerializeField] Transform muzzlePosition;
    [SerializeField] GameObject laserDot;
    [SerializeField] LineRenderer laserLine;

    [SerializeField] GameObject bulletPrefab;

    // Start is called before the first frame update
    void Start()
    {
        // Set up the input system.
        playerInput = GetComponent<PlayerInput>();
        movementInput = playerInput.actions.FindAction("Movement");
        mousePosition = playerInput.actions.FindAction("MousePosition");
        aimInput = playerInput.actions.FindAction("AimPosition");
        movementInput.Enable();
        mousePosition.Enable();
        aimInput.Enable();

        // Get components.
        rb = GetComponent<Rigidbody2D>();

        maxSpeed = Mathf.Pow(maxSpeed, 2);

        laserLine.SetPosition(0, muzzlePosition.position);
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate the player.
        LookAtMousePosition();

        // Laser line of sight.
        RaycastHit2D hitInfo = Physics2D.Raycast(muzzlePosition.position, transform.up, 4.5f);
        if (hitInfo)
        {
            laserDot.transform.position = hitInfo.point;
            laserLine.SetPosition(0, muzzlePosition.position);
            laserLine.SetPosition(1, hitInfo.point);
        }
        else
        {
            laserLine.SetPosition(0, muzzlePosition.position);
            laserLine.SetPosition(1, muzzlePosition.position + muzzlePosition.up * 4.5f);
            laserDot.transform.position = muzzlePosition.position + muzzlePosition.up * 4.5f;
        }
    }

    private void FixedUpdate()
    {
        // Move the player.
        rb.AddForce(acceleration * movementInput.ReadValue<Vector2>());
        // Clamp velocity.
        //if (rb.velocity.sqrMagnitude > maxSpeed)
        //{
        //    rb.velocity = rb.velocity.normalized * Mathf.Sqrt(maxSpeed);
        //}
    }


    public void Shoot(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        Instantiate(bulletPrefab, muzzlePosition.position, transform.rotation);
    }

    private void LookAtMousePosition()
    {
        #region Method1
        //// Rotate the player.
        //Vector2 targetPosition = Camera.main.ScreenToWorldPoint(mousePosition.ReadValue<Vector2>());
        //Vector2 relativPosition = targetPosition - new Vector2(transform.position.x, transform.position.y);
        //var angle = Mathf.Atan2(relativPosition.y, relativPosition.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        #endregion

        #region Method2
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(mousePosition.ReadValue<Vector2>());
        transform.up = (mousePos - new Vector2(transform.position.x, transform.position.y));
        #endregion

    }
}
