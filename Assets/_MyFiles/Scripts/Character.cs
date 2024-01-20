using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class Character : MonoBehaviour
{
    public HealthComponent healthComponent { get; private set; }
    private CharacterController characterController;

    [Header("Character")]
    [SerializeField] float movementSpeed;
    [SerializeField] float rotateSpeed;

    public float MovementSpeed { get { return movementSpeed; } }
    public float RotateSpeed { get { return rotateSpeed; } }    

    public void Init(GameObject owner)
    {
        characterController = GetComponent<CharacterController>();
        healthComponent = GetComponent<HealthComponent>();
        healthComponent.Init(owner);

    }

    public void ProcessMove(Vector3 moveDir)
    {
        characterController.Move(moveDir * Time.fixedDeltaTime * movementSpeed);

        Quaternion targetRotation = Quaternion.LookRotation(moveDir, transform.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.fixedDeltaTime);
    }

    public void TakeDamage(GameObject instigator, int damage)
    {
        healthComponent.onDamageTaken?.Invoke(instigator, damage);
    }
}
