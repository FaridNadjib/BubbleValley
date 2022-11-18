using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleManager : MonoBehaviour
{

    [SerializeField] GameObject bubblePrefab;
    [SerializeField] float startSizeMultiplier = 1f;
    [Tooltip("The absolute x value the new bubbles will randomly spawn in."), Range(0f,5f)]
    [SerializeField] float spawnRangeX;
    [Tooltip("The absolute x value the new bubbles will randomly spawn in."), Range(0f, 5f)]
    [SerializeField] float spawnRangeY;
    [SerializeField] float spawnInterval;
    [SerializeField] int maxStartBubbles;
    [SerializeField] static int maxBubbles;
    [SerializeField] Transform initialForce;
    float spawnTimer;
    static int bubbleCounter;
    static int spawnPointsCounter = 0;

    Vector2 spawnPosition = new Vector2();

    // Start is called before the first frame update
    void Start()
    {
        // ToDo: eventually keep all spawn points in a list
        spawnPointsCounter++;
        for (int i = 0; i < maxStartBubbles; i++)
        {
            // Randomize the spawnposition.
            //spawnPosition.x = Random.Range()
            //GameObject tmp = Instantiate(bubblePrefab,)
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeBubbleCounter(int amount)
    {
        bubbleCounter += amount;
    }
}
