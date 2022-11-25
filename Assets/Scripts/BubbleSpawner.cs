using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the bubble spawning.
/// </summary>
public class BubbleSpawner : MonoBehaviour
{

    [SerializeField] GameObject bubblePrefab;
    [Header("Spawn position related:")]
    [SerializeField] bool randomSpawnArea;
    [Tooltip("The absolute x and y value the new bubbles will randomly spawn in.")]
    [SerializeField] Vector2 spawnRange;
    [SerializeField] Transform[] spawnPositions;
    Vector2 spawnPosition = new Vector2();
    
    [Header("Bubble initilization related:")]
    [SerializeField] float bubbleSizeMultiplier = 0.1f;
    [SerializeField] bool applyInitialForce = false;
    [SerializeField] Transform initialForceDirection;
    [SerializeField] float initialForcePower = 5f;
    Vector2 initialForce = new Vector2();
    [SerializeField] float bubbleGravityMultiplyer = -0.1f;

    [Header("Spawn amount related:")]
    [SerializeField] float spawnInterval = 2.5f;
    [Tooltip("Amount of bubbles to start with.")]
    [SerializeField] int maxStartBubbles = 3;
    [Tooltip("Maximum amount of bubbles for the entire scene.")]
    public static int maxBubbles = 10;
    float spawnTimer;

    // Counts the bubbles in the scene.
    static int bubbleCounter;
    // Counts the bubbleManagers in the scene.
    static int spawnPointsCounter;


    // Start is called before the first frame update
    void Start()
    {
        // ToDo: eventually keep all spawn points in a list
        spawnPointsCounter++;
        for (int i = 0; i < maxStartBubbles; i++)
        {
            SpawnBubble();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Keep spawning new bubbles.
        if (bubbleCounter < maxBubbles)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnInterval)
            {
                SpawnBubble();
                spawnTimer = 0f;
            }
        }
        
    }

    public static void ChangeBubbleCounter(int amount)
    {
        bubbleCounter += amount;
    }

    private Vector2 GetSpawnPosition()
    {
        // Randomize the spawnposition.
        if (randomSpawnArea)
        {
            spawnPosition.x = transform.position.x + Random.Range(-spawnRange.x, spawnRange.x);
            spawnPosition.y = transform.position.y + Random.Range(-spawnRange.y, spawnRange.y);
            return spawnPosition;
        }
        else
        {
            if (spawnPositions.Length != 0)
            {
                spawnPosition = spawnPositions[Random.Range(0, spawnPositions.Length)].position;
            }
            else
            {
                spawnPosition = transform.position;
            }
            return spawnPosition;
        }
    }

    private Vector2 GetInitialForce()
    {
        if (applyInitialForce)
        {
            initialForce = (initialForceDirection.position - transform.position).normalized;
            initialForce *= initialForcePower;
            return initialForce;
        }

        return Vector2.zero;
    }

    private void SpawnBubble()
    {
        BubbleController tmp = Instantiate(bubblePrefab, GetSpawnPosition(), Quaternion.identity, transform).GetComponent<BubbleController>();
        tmp.InitializeBubble(bubbleSizeMultiplier, bubbleGravityMultiplyer, GetInitialForce());
        bubbleCounter++;
    }
}
