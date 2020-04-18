using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float Duration = 1f;
    public float Magnitude = 1f;

    public bool Debug = false;

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.position;

        float elapsed = 0.0f;

        while(elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = originalPos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
    }

    public void StartShake()
    {
        StartCoroutine(Shake(Duration, Magnitude));
    }

    public void Update()
    {
        if (Debug) {
            
            StartCoroutine(Shake(Duration, Magnitude));
            Debug = false;
        }
    }
}
