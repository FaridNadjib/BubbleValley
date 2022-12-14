using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the basic bubbles.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class BubbleController : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] ParticleSystem ps;
    float currentSize;
    Vector2 tmpScale = new Vector2();
    [SerializeField] float growRate = 0.1f;
    float scoreMultiplier = 10f;
    [SerializeField] GameObject scorePrefab;

    [SerializeField] GameObject rewardPrefab;
    [SerializeField] GameObject bubbleExplosion;
    bool doesGiveReward = true;
    bool isBeingDestroyed;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentSize = 0.1f;
    }

    public void InitializeBubble(float sizeMultiplier, float gravityMultiplier, Vector2 initialForce)
    {
        currentSize = sizeMultiplier;
        tmpScale = transform.localScale;
        tmpScale *= sizeMultiplier;
        transform.localScale = tmpScale;
        rb.gravityScale = gravityMultiplier;
        rb.AddForce(initialForce, ForceMode2D.Impulse);
    }

    public void GrowBubble()
    {
        currentSize += growRate * Time.deltaTime;
        // Clamp the size to a maximum.
        currentSize = currentSize > 1.5f ? 1.5f : currentSize;
        tmpScale.x = currentSize;
        tmpScale.y = currentSize;
        transform.localScale = tmpScale;
    }

    public void SetScoreMultiplier(float value)
    {
        scoreMultiplier = value;
    }

    public void DestroyBubble()
    {
        // Only run this method once.
        if (isBeingDestroyed)
            return;
        if (!isBeingDestroyed)
            isBeingDestroyed = true;

        if (doesGiveReward)
        {
            GameManager.Instance.ChangeScoreValue((int)(transform.localScale.x * scoreMultiplier));
            if ((int)(transform.localScale.x * scoreMultiplier) != 0)
            {
                Instantiate(scorePrefab, transform.position, Quaternion.identity).GetComponent<ScoreDisplay>().SetScoreValue((int)(transform.localScale.x * scoreMultiplier));
                AudioManager.Instance.PlayRewardSound();
            }
            // Instantiate player reward.
            //int rewardAmount = currentSize >= 1.5f ? 5 : currentSize > 1f ? 3 : currentSize > 0.5f ? 2 : 0;
            //for (int i = 0; i < rewardAmount; i++)
            //{
            //    Instantiate(rewardPrefab, transform.position, Quaternion.identity);
            //}
        }

        // Instantiate a particle effect.
        Destroy(Instantiate(bubbleExplosion, transform.position, Quaternion.identity), 3f);

        AudioManager.Instance.PlayBubbleSound();

        Destroy(gameObject);
        BubbleSpawner.ChangeBubbleCounter(-1);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            doesGiveReward = false;
            DestroyBubble();
        }
    }

}
