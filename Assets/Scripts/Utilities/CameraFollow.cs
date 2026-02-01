using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Follow Target Settings")]
    [Tooltip("Target to Follow")]
    [SerializeField] private Transform target;

    [Header("Camera Movement Settings")]
    [Tooltip("How quickly the camera catches up")]
    [SerializeField] private float smoothing;

    [Tooltip("How far to move before camera follow starts")]
    [SerializeField] private float deadZone;

    [Header("Camera Shake Settings")]
    [SerializeField] private float duration;

    [SerializeField] private float magnitude;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    void LateUpdate()
    {
        Vector3 targetPos = target.position;
        if((targetPos - transform.position).magnitude > deadZone)
        {
            transform.position = Vector2.Lerp(transform.position, targetPos, smoothing * Time.deltaTime);
        }
    }

    public void Screenshake()
    {
        StartCoroutine(Shake());
    }

    public IEnumerator Shake()
    {
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            // Generate a random offset within a unit sphere (3D) or circle (2D)
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            // For 2D, you might keep z at originalPos.z or 0, depending on your setup

            transform.localPosition = originalPos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;

            yield return null; // Wait until the next frame
        }

        transform.localPosition = originalPos; // Reset to original position when done
    }
}
