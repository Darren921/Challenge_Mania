using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, Controls.IPlayerActions, IDamageable
{
    Controls _controls;
    internal Rigidbody2D Rb2d;
    [SerializeField] internal PlayerStatsSO _playerStatsSO;
    internal bool IsWalking;
    internal bool IsRunning;
    private bool weaponShootToogle;
    [SerializeField] WeaponBase curWeapon;
    internal Vector3 mousePos;
    internal Vector3 localMousePos;

    public Action playerAttackAction;
    public Action playerHitAction;
    public Action playerOnDeath;

    private Camera _camera;
    public float WalkSpeed { get; private set; }
    public float RunSpeed { get; private set; }
    public Vector2 Movement { get; private set; }
    internal float MaxSprintTimer;
    internal bool RefuelSprint;
    private Coroutine sprintCoolDown;
    internal float Health;
    internal float SprintTimer;
    internal int enemiesKilled;

    private void Awake()
    {
        Rb2d = GetComponent<Rigidbody2D>();
        SetUpCharacters();
        EnemySpawner.OnDeath += enemyKilled;
    }

    private void Start()
    {
        MaxSprintTimer = 5;
        SprintTimer = MaxSprintTimer;
        _camera = Camera.main;
    }

    private void SetUpCharacters()
    {
        WalkSpeed = _playerStatsSO.walkSpeed;
        RunSpeed = _playerStatsSO.runSpeed;
        Health = _playerStatsSO.health + GameModifiers.playerHealthModifer * 10;
    }

    private void Update()
    {
        UpdateRotation();

        CheckForSprintRecharge();
    }

    private void CheckForSprintRecharge()
    {
        if (!RefuelSprint || !(SprintTimer < MaxSprintTimer)) return;
        SprintTimer += Time.deltaTime;
        if (SprintTimer >= MaxSprintTimer)
        {
            RefuelSprint = false;
        }
    }

    private void UpdateRotation()
    {
        localMousePos = _camera.ScreenToWorldPoint(mousePos);
        var rotation = localMousePos - transform.position;
        var rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }


    private void OnEnable()
    {
        _controls = new Controls();
        _controls.Player.Enable();
        _controls.Player.Move.performed += OnMove;
        _controls.Player.Move.canceled += OnMove;
        _controls.Player.Attack.performed += OnAttack;
        _controls.Player.Attack.canceled += OnAttack;
        _controls.Player.Reload.performed += OnReload;
        _controls.Player.Running.performed += OnRunning;
        _controls.Player.Running.canceled += OnRunning;
        _controls.Player.GetMousePoint.performed += OnGetMousePoint;
    }

    private void OnDisable() => OnDisablePlayer();

    private void OnDisablePlayer()
    {
        _controls.Player.Disable();
        enemiesKilled = 0;
        StopAllCoroutines();
    }

    public void enemyKilled()
    {
        enemiesKilled++;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Movement = context.ReadValue<Vector2>();
        if (!IsRunning && Movement != Vector2.zero)
        {
            IsWalking = true;
        }
        else if (Movement == Vector2.zero)
        {
            IsWalking = false;
        }
    }


    public void OnAttack(InputAction.CallbackContext context)
    {
        weaponShootToogle = !weaponShootToogle;
        if (weaponShootToogle && !curWeapon.isJammed)
        {
            curWeapon.startAttacking();
            playerAttackAction?.Invoke();
        }
        else curWeapon.stopAttacking();
    }


    public void OnInteract(InputAction.CallbackContext context)
    {
    }

    public void OnRunning(InputAction.CallbackContext context)
    {
        IsRunning = true;
        IsWalking = false;
        RefuelSprint = false;
        if (!context.canceled && !(SprintTimer <= 0)) return;
        IsRunning = false;
        if (!(SprintTimer <= MaxSprintTimer)) return;
        sprintCoolDown ??= StartCoroutine(EnforceCd());
    }

    private IEnumerator EnforceCd()
    {
        yield return new WaitForSeconds(2);
        RefuelSprint = true;
        sprintCoolDown = null;
    }

    public void OnGetMousePoint(InputAction.CallbackContext context)
    {
        mousePos = context.ReadValue<Vector2>();
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        if (!curWeapon.isReloading && !curWeapon._isMelee)
        {
            curWeapon.StartCoroutine(curWeapon.GetTryReload());
        }

        playerAttackAction.Invoke();
    }


    public void TakeDamage(float damage)
    {
        Rb2d.linearVelocity = Vector2.zero;
        playerHitAction?.Invoke();
        Health -= damage;
        if (Health <= 0)
        {
            gameObject.SetActive(false);
            playerOnDeath?.Invoke();
        }
    }
}