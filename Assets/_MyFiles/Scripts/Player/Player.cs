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

    [Header("Death Sound")]
    [SerializeField] AudioClip deathSound;

    [Header("Bug Fix")]
    [SerializeField] Transform jojo;

    [Header("Spotlight")]
    private Transform spotLight;
    private Vector3 refVel;
    [SerializeField, Range(0, 10)] float followSpeed;
    [SerializeField] float spotlightHeight = 10f;

    [Header("Death Cam")]
    [SerializeField] Camera deathCam;
    public Camera DeathCamera { get { return deathCam; } }

    bool isAttacking = false;

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

        spotLight = GameManager.Instance.spotLight;
    }

    private void InitiatePause(InputAction.CallbackContext context)
    {
        PauseMenu.Instance.Pause();
    }

    private void StartDeath()
    {
        isDead = true;

        GameManager.Instance.DisableMainUI();

        _Animation.SetBool("isDead", isDead);
        PlaySoundClip(deathSound);

        CameraManager.Instance.ShowJojoDeathCam();
        StartCoroutine(DeathCoroutine());

    }

    private IEnumerator DeathCoroutine()
    {
        yield return new WaitForSeconds(3f);

        LoseScreen.Instance.Lose();
    }

    private void Update()
    {


        if (spotLight)
        {
            Vector3 target = new Vector3(transform.position.x, spotlightHeight, transform.position.z);

            spotLight.position = Vector3.SmoothDamp(spotLight.position, target, ref refVel, followSpeed);
        }

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
        _Animation.SetTrigger("Attack");

        damager.StartDamage(0.1f);

        yield return new WaitForSeconds(0.5f);

        _Animation.ResetTrigger("Attack");

        isAttacking = false;
    }

    

}
