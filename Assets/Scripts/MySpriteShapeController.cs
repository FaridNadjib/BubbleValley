using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

[RequireComponent(typeof(Rigidbody2D))]
public class MySpriteShapeController : MonoBehaviour, IShakeable
{
    #region Fields
    [SerializeField] float shakeSpeed = 5f;
    [SerializeField] float shakeAmount = 5f;
    [SerializeField] float shakeTime = 1f;
    SpriteShapeController shape;
    bool startShaking;
    float shakeTimer = 0;

    Rigidbody2D rb;

    Vector2 shakeDirection;
    // ToDo: make shaketime and amount relative to objects velo and mass.
    Vector2 tmpPosition;
    #endregion


    public void StartShaking()
    {
        //tmpPosition = transform.position;
        //tmpPosition.x = Mathf.Sin(Time.deltaTime * shakeSpeed) * shakeAmount;
        //transform.position = tmpPosition;

        rb.MovePosition(Time.deltaTime * 5f * shakeDirection);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Initialize components.
        shape = GetComponent<SpriteShapeController>();
        shape.fillPixelsPerUnit = 555f;

        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (startShaking)
        {
            shakeTimer += Time.deltaTime;
            StartShaking();
            if (shakeTimer > shakeTime)
            {
                startShaking = false;
                shakeTimer = 0f;
            }
            
        }
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        startShaking = true;
        if (collision.gameObject.GetComponent<IShakeable>() != null)
        {
            shakeDirection = (collision.transform.position - transform.position).normalized * -1f;
        }
    }
}
