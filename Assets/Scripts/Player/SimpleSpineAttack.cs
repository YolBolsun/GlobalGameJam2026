using UnityEngine;

public class SimpleSpineAttack : MonoBehaviour
{
    private float rotationSpeed = 40f;

    public bool reverse = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (reverse)
        {
            rotationSpeed *= -1f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
