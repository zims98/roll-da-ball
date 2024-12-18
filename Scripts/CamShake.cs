using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamShake : MonoBehaviour
{
    public bool camShakeActive = true;

    [Range(0, 1)] [SerializeField] float shake; // Status/length of shake (0 - 1)
    [SerializeField] float shakeMultiplier = 5f; // Amount of shakes
    [SerializeField] float shakeMagnitude = 0.8f; // How "intense" each shake should be
    [SerializeField] float shakeRotationMagnitude = 17f; // How much rotation on the shakes
    [SerializeField] float shakeDecay = 1f; // Amount of shake ceasing over time
    [SerializeField] float shakeDepthMagnitude = 1.3f; // How much depth on the shakes
    [SerializeField] float shakeFallOff = 0.3f; // The fall off of the shake

    float timeCounter;

    public float Shake
    {
        get
        {
            return shake; // Returns the shake when called from another script
        }
        set
        {
            shake = Mathf.Clamp01(value); // Clamps the shake between 0 and 1
        }
    }

    float GetFloat(float seed)
    {
        return (Mathf.PerlinNoise(seed, timeCounter) - 0.5f) * 2;
    }

    Vector3 GetVector3()
    {
        return new Vector3(GetFloat(1), GetFloat(10), GetFloat(100) * shakeDepthMagnitude);
    }

    private void Update()
    {
        if (camShakeActive && Shake > 0.001f) // Shake is active
        {
            timeCounter += Time.deltaTime * Mathf.Pow(shake, shakeFallOff) * shakeMultiplier;
            Vector3 newPos = GetVector3() * shakeMagnitude * Shake;
            transform.localPosition = newPos;
            transform.localRotation = Quaternion.Euler(newPos * shakeRotationMagnitude);
            Shake -= Time.deltaTime * shakeDecay * Shake;

            
        }
        else  // Shake duration is over - TODO: Reset positions to 0
        {
            Vector3 newPos = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime); 
            transform.localPosition = newPos; // Lerps the position back to original
            transform.localRotation = Quaternion.Euler(newPos * shakeRotationMagnitude); // Lerps the rotation back to original
        }
    }
}