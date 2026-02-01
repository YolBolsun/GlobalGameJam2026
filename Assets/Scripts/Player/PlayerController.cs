using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController: MonoBehaviour
{
    [Serializable]
    public class AttackData
    {
        public float attackCooldown;
        public int attackDamage;
        public bool followPlayerMovement = false;
        public GameObject attackPrefab;
        public float attackSpawnDistance;
        public List<Transform> targetLocations;

        public bool useRandomAttack = false;
        public float minRandomAttackRadius;
        public float maxRandomAttackRadius;
        public float timeOfLastAttack = 0;

        public float timeToDestroy = 1f;
    }

    [Header("Movement Settings")]
    [Tooltip("Movement Speed - Player Speed")]
    [SerializeField] private float movementSpeed;

    [Header("Dash setup variables")]
    [Tooltip("Dash Distance - How far the dash is aiming")]
    [SerializeField] private float dashDistance;
    [Tooltip("Dash Cooldown - Length Between dashes")]
    [SerializeField] private float dashCooldown;
    [Tooltip("Dash Duration - How long the players move towards that target... maybe?")]
    [SerializeField] private float dashDuration;

    [Header("Attack Settings")]
    [SerializeField] List<AttackData> attacks;

    InputAction moveAction;
    InputAction lookAction;
    InputAction dashAction;
    private float dashStartTime = -10000f;
    private Vector2 dashDirection;
    //facing direction
    private Vector3 facingDirection;
    //mouse/rightstick direction
    private Vector3 aimDirection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        dashAction = InputSystem.actions.FindAction("Jump");
        lookAction = InputSystem.actions.FindAction("Look");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAimDirection();
        HandleMovement();
        HandleAttack();

    }

    private void UpdateAimDirection()
    {
        Vector2 mousePosition = lookAction.ReadValue<Vector2>();
        Vector3 worldPositionOfMouse = Camera.main.ScreenToWorldPoint(mousePosition);
        aimDirection = (worldPositionOfMouse - transform.position).normalized;
    }

    private void HandleMovement()
    {
        Vector3 moveValue = moveAction.ReadValue<Vector2>();
        float calculatedMoveSpeed = movementSpeed;

        //if isdashing
        if (Time.time < dashStartTime + dashDuration)
        {
            moveValue = dashDirection;
            calculatedMoveSpeed = dashDistance / dashDuration;
        }
        else if (Time.time > dashStartTime + dashCooldown && dashAction.triggered)
        {
            dashStartTime = Time.time;
            dashDirection = facingDirection;
        }

        if (moveValue.magnitude > .1f)
        {
            facingDirection = moveValue.normalized;
        }

        transform.Translate(calculatedMoveSpeed * Time.deltaTime * moveValue);
    }

    private void HandleAttack()
    {
        foreach(AttackData attack in attacks)
        {
            if(Time.time > attack.timeOfLastAttack + attack.attackCooldown)
            {
                attack.timeOfLastAttack = Time.time;
                List<Vector3> spawnLocations = new List<Vector3>();
                
                if(attack.targetLocations.Count > 0)
                {
                    foreach(Transform targetTransform in attack.targetLocations)
                    {
                        spawnLocations.Add(targetTransform.position);
                    }
                }
                else if (attack.useRandomAttack)
                {
                    float randX = UnityEngine.Random.Range(attack.minRandomAttackRadius, attack.maxRandomAttackRadius);
                    int flip = UnityEngine.Random.Range(0, 2);
                    if(flip == 1)
                    {
                        randX *= -1f;
                    }
                    float randY = UnityEngine.Random.Range(attack.minRandomAttackRadius, attack.maxRandomAttackRadius);
                    flip = UnityEngine.Random.Range(0, 2);
                    if (flip == 1)
                    {
                        randY *= -1f;
                    }
                    Vector3 offset = new Vector3(randX, randY, 0f);
                    spawnLocations.Add(transform.position + offset);
                }
                else {
                    spawnLocations.Add(transform.position + aimDirection * attack.attackSpawnDistance);
                }
                Transform parentTransform = null;
                if (attack.followPlayerMovement)
                {
                    parentTransform = this.transform;
                }
                foreach (Vector3 spawnLocation in spawnLocations)
                {
                    GameObject attackObject = GameObject.Instantiate(attack.attackPrefab, spawnLocation, Quaternion.identity, parentTransform);
                    attackObject.GetComponent<Attack>().attackData = attack;
                }
            }
        }
    }
}
