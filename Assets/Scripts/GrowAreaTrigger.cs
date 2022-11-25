using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Will grow the bubbles once inside this trigger area.
/// </summary>
public class GrowAreaTrigger : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Bubble"))
        {
            collision.GetComponent<BubbleController>().GrowBubble();
        }
    }
}
