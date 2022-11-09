using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelMe : MonoBehaviour
{
    //[SerializeField] GameObject pointEffector;
    //[SerializeField] float movementSpeed = 5f;
    //[SerializeField] float airMovementSpeed = 3f;
    //[SerializeField] float jumpPower = 10f;
    //Rigidbody2D rb;

    //float horizontalMovement;
    //Vector2 slope;
    //Vector3 movementDirection;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    rb = GetComponent<Rigidbody2D>();
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    horizontalMovement = Input.GetAxis("Horizontal");

    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    //    }

    //    movementDirection = Vector3.Cross(new Vector3(slope.x, slope.y, 0f), Vector3.forward);

    //    //if (Input.GetKeyDown(KeyCode.E))
    //    //{
    //    //    rb.gravityScale = 0.2f;
    //    //}
    //    if (Input.GetKeyDown(KeyCode.F))
    //    {
    //        pointEffector.SetActive(true);
    //    }
    //    if (Input.GetKeyUp(KeyCode.F))
    //    {
    //        pointEffector.SetActive(false);
    //    }
    //}

    //private void FixedUpdate()
    //{
    //    //rb.AddForce(new Vector2(movementDirection.x, movementDirection.y) * horizontalMovement * movementSpeed);
    //    rb.velocity = horizontalMovement * movementSpeed * new Vector2(movementDirection.x, movementDirection.y);
    //    Debug.Log(rb.velocity);
    //}

    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    slope = collision.contacts[0].normal;
    //    Debug.Log(movementDirection);
    //}


    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawLine(transform.position, movementDirection);
    //}
}
