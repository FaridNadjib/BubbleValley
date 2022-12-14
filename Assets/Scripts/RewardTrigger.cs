using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardTrigger : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // ToDO: Add points and effects.
            Destroy(gameObject);
        }
    }
}
