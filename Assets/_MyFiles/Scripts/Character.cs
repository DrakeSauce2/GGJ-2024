using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Character : MonoBehaviour
{
    public HealthComponent healthComponent { get; private set; }
    private CharacterController characterController;

    [Header("Character")]
    [SerializeField] float movementSpeed;
    [SerializeField] float rotateSpeed;
    [Space]
    [SerializeField] private int health;
    [SerializeField] private int maxHealth;
    [Space]
    [SerializeField] private float invulnerableDuration;
    [Header("Team")]
    [SerializeField] private int teamIndex;

    public float MovementSpeed { get { return movementSpeed; } }
    public float RotateSpeed { get { return rotateSpeed; } }

    private bool isDamagable = true;

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

    public void TakeDamage(GameObject instigator, int damage, int team)
    {
        if (!isDamagable) return;
        if (instigator == gameObject) return;
        if (teamIndex == team) return;

        health -= damage;
        healthComponent.onHealthChanged?.Invoke(instigator, health, maxHealth);

        StartCoroutine(InvulnerabeCoroutine());


    }

    public void Heal(int healAmount)
    {
        health += healAmount;

        if(health >= maxHealth) health = maxHealth;

        healthComponent.onHealthChanged?.Invoke(null, health, maxHealth);
    }

    private IEnumerator InvulnerabeCoroutine()
    {
        isDamagable = false;

        yield return new WaitForSeconds(invulnerableDuration);

        isDamagable = true;
    }

}
