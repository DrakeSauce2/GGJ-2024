using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Character : MonoBehaviour
{
    public HealthComponent healthComponent { get; private set; }
    private CharacterController characterController;

    [Header("Character")]
    [SerializeField] float movementSpeed;
    float currentMovementSpeed;
    [SerializeField] float rotateSpeed;
    [Space]
    [SerializeField] private int health;
    [SerializeField] private int maxHealth;
    [Header("Animations")]
    [SerializeField] Animator animator;

    [Header("Audio")]
    [SerializeField] private AudioSource _SFXSource;
    [SerializeField] AudioClip gruntAudio;

    [Header("Death Sounds")]
    [SerializeField] List<AudioClip> deathSounds;

    public Animator _Animation { get { return animator; } }

    public int Health { get { return health; } protected set { health = value; } }
    public int MaxHealth { get { return maxHealth; } protected set { maxHealth = value; } }

    [Space]
    [SerializeField] private float invulnerableDuration;
    [Header("Team")]
    [SerializeField] private int teamIndex;
    public int Team { get { return teamIndex; } }

    public float MovementSpeed { get { return currentMovementSpeed; } }
    public float RotateSpeed { get { return rotateSpeed; } }

    [SerializeField] private bool isDamagable = true;

    protected bool isDead = false;

    public void Init(GameObject owner)
    {
        characterController = GetComponent<CharacterController>();
        healthComponent = GetComponent<HealthComponent>();

        if(healthComponent)
            healthComponent.Init(owner);

        currentMovementSpeed = movementSpeed;

        Debug.Log(_SFXSource);
        Debug.Log(AudioManager.Instance);
    }

    private void Update()
    {
        _SFXSource.volume = AudioManager.Instance.SoundSettings.soundVolume;
    }

    private void Start()
    {
        _SFXSource.volume = AudioManager.Instance.SoundSettings.soundVolume;
    }

    public void ProcessMove(Vector3 moveDir)
    {
        
        characterController.Move(moveDir.normalized * Time.fixedDeltaTime * movementSpeed);

        Quaternion targetRotation = Quaternion.LookRotation(moveDir, transform.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.fixedDeltaTime);
    }

    public virtual void TakeDamage(GameObject instigator, int damage, int team)
    {
        if (isDead) return;
        if (!isDamagable) return;
        if (teamIndex == team) return;

        health -= damage;
        healthComponent.onHealthChanged?.Invoke(instigator, health, maxHealth);

        StartCoroutine(InvulnerabeCoroutine());

        PlaySoundClip(gruntAudio);
    }

    public void Heal(int healAmount)
    {
        health += healAmount;

        if(health >= maxHealth) health = maxHealth;

        healthComponent.onHealthChanged?.Invoke(null, health, maxHealth);
    }

    public IEnumerator ApplySlow(float duration)
    {
        currentMovementSpeed = movementSpeed / 2;

        yield return new WaitForSeconds(duration);

        currentMovementSpeed = movementSpeed;

    }

    private IEnumerator InvulnerabeCoroutine()
    {
        isDamagable = false;

        yield return new WaitForSeconds(invulnerableDuration);

        isDamagable = true;
    }

    public void PlaySoundClip(AudioClip clip)
    {
        if (clip == null) return;

        _SFXSource.clip = clip;
        _SFXSource.Play();
    }

    public void PlayDeathSound()
    {
        _SFXSource.clip = deathSounds[0];
        _SFXSource.Play();
    }


}
