using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Music related:")]
    [SerializeField] AudioSource ambientSource;
    [SerializeField] AudioClip bgMusic;
    [Header("Sound clip related:")]
    [SerializeField] AudioSource effectSource;
    [SerializeField] AudioClip bubbleSound;
    [SerializeField] AudioClip rewardSound;

    #region Singleton
    public static AudioManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    public void PlayBackgroundMusic()
    {
        ambientSource.PlayOneShot(bgMusic);
    }

    public void PlayBubbleSound()
    {
        effectSource.PlayOneShot(bubbleSound);
    }

    public void PlayRewardSound()
    {
        effectSource.PlayOneShot(rewardSound);
    }
}
