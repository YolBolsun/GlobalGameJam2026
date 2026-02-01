using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Enemy AI Movement Settings")]
    [Tooltip("How close to get to the player")]
    [SerializeField] private float desiredMinimumDistanceToPlayer;
    [SerializeField] private float desiredMaximumDistanceToPlayer;

    [SerializeField] private EnemyRepositionType repositionType;

    [Tooltip("How close to get to the desired destination")]
    [SerializeField] private float destinationWiggleRoom;

    [Tooltip("How close to detect player")]
    [SerializeField] private float aggroRange;

    [SerializeField] private float movementSpeed;

    [Header("Enemy AI Attack Settings")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private int attackDamage;
    [SerializeField] private GameObject attackPrefab;
    [SerializeField] private float attackPrefabSpawnDistance;
    [SerializeField] private float attackAnimationTime;
    [SerializeField] private float attackPrefabSpeed;
    [SerializeField] private float attackPrefabLifetime;


    [Header("Death Settings")]
    [Tooltip("Enemy Death Animation Time")]
    [SerializeField] private float deathAnimationTime;


    enum State
    {
        Repositioning,
        Attacking,
        Dying,
        Idle
    }

    public enum EnemyRepositionType
    {
        SimpleTowardsPlayer,
        ErraticWithinAttackRange,
    }

    private State currState = State.Idle;
    private Transform player;
    private Vector3 destination;
    private float lastAttackTime = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (currState == State.Dying) {
            return;
        }
        if (currState == State.Idle)
        {
            if((player.position-transform.position).magnitude < aggroRange)
            {
                currState = State.Repositioning;
                PickDestination();
            }
        }
        if(currState == State.Repositioning && (transform.position - destination).magnitude > destinationWiggleRoom)
        {
            // move to destination
            transform.Translate((destination - transform.position).normalized * Time.deltaTime * movementSpeed);
        }
        else if(currState == State.Repositioning)
        {
            currState = State.Attacking;
        }

        if(currState == State.Attacking)
        {
            HandleAttack();
        }


    }

    private void HandleAttack()
    {
        
        if(Time.time > lastAttackTime + attackCooldown)
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            GameObject spawnedAttack = GameObject.Instantiate(attackPrefab, transform.position + directionToPlayer*attackPrefabSpawnDistance, Quaternion.identity);
            spawnedAttack.GetComponent<Rigidbody2D>().linearVelocity = directionToPlayer * attackPrefabSpeed;
            spawnedAttack.GetComponent<EnemyAttack>().damage = attackDamage;
            Destroy(spawnedAttack, attackPrefabLifetime);
            lastAttackTime = Time.time;
        }
        if (Time.time > lastAttackTime + attackAnimationTime)
        {
            currState = State.Repositioning;
            PickDestination();
        }
    }

    private void PickDestination()
    {
        if(repositionType == EnemyRepositionType.SimpleTowardsPlayer)
        {
            destination = (transform.position - player.position).normalized * desiredMaximumDistanceToPlayer + player.position;
        }
        if (repositionType == EnemyRepositionType.ErraticWithinAttackRange)
        {
            float randX = UnityEngine.Random.Range(desiredMinimumDistanceToPlayer, desiredMaximumDistanceToPlayer);
            int flip = UnityEngine.Random.Range(0, 2);
            if (flip == 1)
            {
                randX *= -1f;
            }
            float randY = UnityEngine.Random.Range(desiredMinimumDistanceToPlayer, desiredMaximumDistanceToPlayer);
            flip = UnityEngine.Random.Range(0, 2);
            if (flip == 1)
            {
                randY *= -1f;
            }
            Vector3 offset = new Vector3(randX, randY, 0f);

            destination = player.position + offset;

            //destination = (transform.position - player.position).normalized * desiredMaximumDistanceToPlayer + player.position;
        }
    }

    public void Die()
    {
        currState = State.Dying;
        Destroy(gameObject, deathAnimationTime);
        Debug.Log("implement enemy death animation");
    }
}
