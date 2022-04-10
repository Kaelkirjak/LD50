using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    // ------------------------------------------------------------------------------------------------------------------------------------------

    #region Shaker Variables

    [Header("Shaker Variables")]
    [Tooltip("Amount of shakes")][SerializeField]                               int shakeCount;
    [Tooltip("Shake length")][SerializeField]                                   float shakeLength;
    [Tooltip("Shake intensity")][SerializeField]                                float shakeIntensity;
    [Tooltip("Shake yRandomness")][SerializeField]                              float yRandomness;
    [Tooltip("Shake intensity curve")][SerializeField]                          AnimationCurve shakeIntensityCurve;
    [Tooltip("Shake animation curve")][SerializeField]                          AnimationCurve shakeAnimationCurve;
    [Tooltip("Transform to shake")][SerializeField]                             Transform objectToShake;

    [Header("Shaker Functionality Variables")]
    [Tooltip("Shake handler")]                                                  Coroutine shakeHandler;
    [Tooltip("Shaked objects start position")]                                  Vector3 startPosition;

    #endregion Shaker Variables

    // ------------------------------------------------------------------------------------------------------------------------------------------
    // ------------------------------------------------------------------------------------------------------------------------------------------
    // ------------------------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Shake the object assigned to this shaker
    /// </summary>
    public void Shake()
    {
        if(shakeHandler != null)
        {
            StopCoroutine(shakeHandler);
            objectToShake.localPosition = startPosition;
        }
        shakeHandler = StartCoroutine(ShakeHandler());
    }

    /// <summary>
    /// Handle the shaking of the object
    /// </summary>
    IEnumerator ShakeHandler()
    {
        startPosition = objectToShake.localPosition;

        float startTime = Time.time;
        Vector3 shakeLeftPos = new Vector3(startPosition.x - shakeIntensity, startPosition.y + Random.Range(-yRandomness, yRandomness), startPosition.z);
        Vector3 shakeRightPos = new Vector3(startPosition.x + shakeIntensity, startPosition.y + Random.Range(-yRandomness, yRandomness), startPosition.z);
        int r = Random.Range(0, 2);
        for (int shake = 0+r; shake < shakeCount+r; shake++)
        {
            float intensity = shakeIntensityCurve.Evaluate((Time.time-startTime)/shakeLength);
            if (shake % 2 == 0) {
                if(shake == shakeCount + r - 1) { shakeRightPos = startPosition; }
                else { shakeRightPos = new Vector3(startPosition.x + (shakeIntensity * intensity), startPosition.y + Random.Range(-yRandomness * intensity, yRandomness * intensity), startPosition.z); }
            }
            else {
                if (shake == shakeCount + r - 1) { shakeLeftPos = startPosition; }
                else { shakeLeftPos = new Vector3(startPosition.x - (shakeIntensity * intensity), startPosition.y + Random.Range(-yRandomness * intensity, yRandomness * intensity), startPosition.z); }
            }

            float shakeStart = Time.time;
            while (Time.time - shakeStart < shakeLength/shakeCount)
            {
                float progression = shakeAnimationCurve.Evaluate((Time.time - shakeStart)/(shakeLength/shakeCount));
                if(shake%2 == 0) { objectToShake.localPosition = Vector3.Lerp(shakeLeftPos, shakeRightPos, progression); }
                else { objectToShake.localPosition = Vector3.Lerp(shakeRightPos, shakeLeftPos, progression); }
                yield return new WaitForEndOfFrame();
            }
        }

        objectToShake.localPosition = startPosition;
        shakeHandler = null;
    }

    // ------------------------------------------------------------------------------------------------------------------------------------------

    public static CameraShaker instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
}
