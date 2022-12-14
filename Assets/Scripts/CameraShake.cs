using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
/// Simple script to make the camera shake.
/// </summary>
public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    CinemachineVirtualCamera cinemachineCamera;
    CinemachineBasicMultiChannelPerlin shakePerlin;
    float shakeTimer;
    float shakeTime;
    float startingIntensity;

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

        cinemachineCamera = GetComponent<CinemachineVirtualCamera>();
        shakePerlin = cinemachineCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ShakeCamera(float intensity, float time)
    {
        shakePerlin.m_AmplitudeGain = intensity;
        startingIntensity = intensity;
        shakeTimer = time;
        shakeTime = time;
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            //shakePerlin.m_AmplitudeGain = Mathf.Lerp(startingIntensity, 0f, shakeTimer / shakeTime);
            if (shakeTimer <= 0f)
            {
                shakePerlin.m_AmplitudeGain = 0f;
            }
        }
    }
}
