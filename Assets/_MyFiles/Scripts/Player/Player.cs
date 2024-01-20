using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class Player : Character
{
    public static Player Instance;

    private PlayerInputActions playerInputActions;

    [Header("Attack LayerMask")]
    [SerializeField] private LayerMask attackMask;

    bool isAttacking = false;   

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);

        Init(gameObject);

        playerInputActions = new PlayerInputActions();

        playerInputActions.Enable();
        playerInputActions.Main.Attack.performed += ProcessAttack;

    }

    private void FixedUpdate()
    {
        if (isAttacking) return;

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
        if (isAttacking) return;

        Vector2 mousePosition = playerInputActions.Main.MousePosition.ReadValue<Vector2>();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        if(Physics.Raycast(ray, out RaycastHit hit, 1000f, attackMask))
        {
            StartCoroutine(AttackCoroutine(hit.point));
            Debug.Log(hit.point);
        }
    }

    private IEnumerator AttackCoroutine(Vector3 direction)
    {
        isAttacking = true;

        //Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

        //transform.rotation = targetRotation;

        yield return new WaitForSeconds(0.7f);

        isAttacking = false;
    }

    

}
