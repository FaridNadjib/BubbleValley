using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSController : MonoBehaviour
{
    [SerializeField] ParticleSystem ps;
    [SerializeField] ParticleSystem[] psToPlay;
    [SerializeField] float feedbackForceThreshold = 3f;
    private void OnCollisionEnter2D(Collision2D collision)
    {
       
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 contact = collision.GetContact(0).point;
            if (Vector2.Dot(Vector2.up, (contact - transform.position).normalized) > 0.1f && collision.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude >= feedbackForceThreshold)
            {
                for (int i = 0; i < psToPlay.Length; i++)
                {
                    psToPlay[i].Play();
                }
                ps.Play();
            }

            
        }
    }
}
