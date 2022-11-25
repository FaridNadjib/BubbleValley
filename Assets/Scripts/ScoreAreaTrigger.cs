using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An animated text for showing the score.
/// </summary>
public class ScoreAreaTrigger : MonoBehaviour
{
    [SerializeField] float scoreMultiplier = 10f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bubble"))
        {
            collision.gameObject.GetComponent<BubbleController>().SetScoreMultiplier(scoreMultiplier);
            collision.gameObject.GetComponent<BubbleController>().DestroyBubble();
        }
    }
}
