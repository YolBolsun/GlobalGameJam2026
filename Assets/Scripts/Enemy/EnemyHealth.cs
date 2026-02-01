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

    public void TakeDamage(float damage, float knockback)
    {
        Debug.Log("enemy took damage " + damage);
        health -= damage;
        if(health <= 0)
        {
            health = 0;
            GetComponent<EnemyAI>().Die();
        }
        Vector3 playerLocation = GameObject.FindGameObjectWithTag("Player").transform.position;
        GetComponent<Rigidbody2D>().AddForce((transform.position - playerLocation).normalized * knockback, ForceMode2D.Impulse);
    }
}
