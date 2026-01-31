using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    [Serializable]
    public class Attack
    {
        public float attackCooldown;
        public float attackDamage;
        public bool followPlayerMovement = false;
        public GameObject attackPrefab;
        public float attackSpawnDistance;
        public List<Transform> targetLocations;

        public float timeOfLastAttack = 0;

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
    [SerializeField] List<Attack> attacks;

    InputAction moveAction;
    InputAction dashAction;
    private float dashStartTime = -10000f;
    private Vector2 dashDirection;
    //facing direction
    private Vector3 facingDirection;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        dashAction = InputSystem.actions.FindAction("Jump");
        // Place holder for dash target
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleAttack();
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
        foreach(Attack attack in attacks)
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
                else
                {
                    spawnLocations.Add(transform.position + facingDirection * attack.attackSpawnDistance);
                }
                Transform parentTransform = null;
                if (attack.followPlayerMovement)
                {
                    parentTransform = this.transform;
                }
                foreach (Vector3 spawnLocation in spawnLocations)
                {
                    GameObject.Instantiate(attack.attackPrefab, spawnLocation, Quaternion.identity, parentTransform);

                }
            }
        }
    }
}
