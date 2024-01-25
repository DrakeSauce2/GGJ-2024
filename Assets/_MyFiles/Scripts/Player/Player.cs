using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    public static Player Instance;

    private PlayerInputActions playerInputActions;
    private Damager damager;
    
    [SerializeField] HealthGUI healthGUI;

    [Header("Attack LayerMask")]
    [SerializeField] private LayerMask attackMask;

    [Header("Hat")]
    [SerializeField] private GameObject headObj;
    [SerializeField] private Transform headPoint;

    [Header("Bug Fix")]
    [SerializeField] Transform jojo;

    public Transform HeadPoint { get { return headPoint; } }

    bool isAttacking = false;
    bool isDead = false;

    private void Start()
    {
        if (Instance == null) Instance = this;
        else Destroy(Instance);

        Init(gameObject);
        
        healthGUI = HealthGUI.Instance;
        healthGUI.Init(healthComponent);

        damager = GetComponent<Damager>();

        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();

        playerInputActions.Main.Attack.performed += ProcessAttack;
        playerInputActions.Main.Pause.performed += InitiatePause;
        healthComponent.onDeath += StartDeath;
    }

    private void InitiatePause(InputAction.CallbackContext context)
    {
        PauseMenu.Instance.Pause();
    }

    private void StartDeath()
    {
        isDead = true;

        StartCoroutine(DeathCoroutine());

    }

    private IEnumerator DeathCoroutine()
    {
        yield return new WaitForSeconds(3f);

        LoseScreen.Instance.Lose();
    }

    private void Update()
    {
        jojo.position = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
    }

    private void FixedUpdate()
    {
        if (isAttacking || isDead) return;

        PlayerMove();
    }

    private void PlayerMove()
    {
        Vector2 moveInput = playerInputActions.Main.Move.ReadValue<Vector2>();
        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);

        _Animation.SetFloat("speed", moveDirection.magnitude);

        if (moveInput.magnitude > 0)
            ProcessMove(moveDirection);
    }

    private void ProcessAttack(InputAction.CallbackContext context)
    {
        if (isAttacking || isDead) return;

        StartCoroutine(AttackCoroutine());
    }

    private IEnumerator AttackCoroutine()
    {
        isAttacking = true;

        damager.StartDamage(0.1f);

        yield return new WaitForSeconds(0.3f);

        isAttacking = false;
    }

    

}
