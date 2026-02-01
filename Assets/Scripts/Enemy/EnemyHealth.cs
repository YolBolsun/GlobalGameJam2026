using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [Tooltip("Enemy Health")]
    [SerializeField] private float health;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("enemy took damage " + damage);
        health -= damage;
        if(health <= 0)
        {
            health = 0;
            GetComponent<EnemyAI>().Die();
        }
    }
}
