using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraShake : MonoBehaviour
{
    public static CameraShake _instance;

    private Coroutine currentCoroutine = null;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void _Shake(float duration, float magnitude)
    {
        if (currentCoroutine == null)
            currentCoroutine = StartCoroutine(Shake(duration, magnitude));
    }

    IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 orignalPosition = transform.position;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            print(x +" - " + y);
            transform.position = Vector3.Lerp(new Vector3(orignalPosition.x + x, orignalPosition.y + y, -10f),
                orignalPosition, elapsed / duration);
            elapsed += Time.fixedDeltaTime;
            yield return 0;
        }
        transform.position = orignalPosition;

        currentCoroutine = null;
    }
}