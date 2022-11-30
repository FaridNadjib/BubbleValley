using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float projectileSpeed;
    [SerializeField] GameObject impactPrefab;
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.up * projectileSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(Instantiate(impactPrefab, new Vector2(transform.position.x, transform.position.y) - rb.velocity.normalized * .15f, Quaternion.identity),5f);
        Destroy(gameObject);
    }
}
