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
    [SerializeField] private GameObject attackPrefab;
    [SerializeField] private float attackAnimationTime;


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
            Debug.Log("spawn attack");
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
    }

    public void Die()
    {
        currState = State.Dying;
        Destroy(gameObject, deathAnimationTime);
        Debug.Log("implement enemy death animation");
    }
}
