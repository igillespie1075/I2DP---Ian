using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject player;
    private Vector3 offset = new Vector3(0, 0.5f, -1);
    void Start()
    {
        
    }

    void Update()
    {
        transform.position = player.transform.position + offset;
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPosition = transform.localPosition;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = originalPosition + new Vector3(x, y, 0f);

            elapsed += Time.unscaledDeltaTime;
            yield return null; 
        }

        transform.localPosition = originalPosition;
    }
}
