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
}
