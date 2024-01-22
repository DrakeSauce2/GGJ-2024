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

        if(moveInput.magnitude > 0)
            ProcessMove(moveDirection);
    }

    private void ProcessAttack(InputAction.CallbackContext context)
    {
        if (isAttacking || isDead) return;

        /*
        Vector2 mousePosition = playerInputActions.Main.MousePosition.ReadValue<Vector2>();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        if(Physics.Raycast(ray, out RaycastHit hit, 1000f, attackMask))
        {
            StartCoroutine(AttackCoroutine(hit.point));
        }
        */

        StartCoroutine(AttackCoroutine(Vector3.zero));
    }

    private IEnumerator AttackCoroutine(Vector3 direction)
    {
        isAttacking = true;

        //Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

        //transform.rotation = targetRotation;

        damager.StartDamage(0.1f);

        yield return new WaitForSeconds(0.3f);

        isAttacking = false;
    }

    

}
