using UnityEngine;
using System.Collections;

public enum WeaponType
{
    Single,
    Auto,
    Shotgun
}

public class Gun : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] private WeaponType weaponType = WeaponType.Single;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 0.2f;

    [Header("Shotgun Settings")]
    [SerializeField, Min(1)] private int pelletsPerShot = 6;
    [SerializeField, Min(0f)] private float spreadAngle = 45f;

    [Header("Ammo Settings")]
    [SerializeField, Min(1)] private int maxAmmo = 12;
    [SerializeField, Min(0f)] private float reloadTime = 1.5f;

    [Header("Orbit Settings")]
    [SerializeField] private Transform player;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private float orbitRadius = 0.5f;
    [SerializeField] private float orbitSpeed = 720f;
    [SerializeField] private float orbitLimit = 90f;

    private float currentAngle;

    private int currentAmmo;
    private float nextFireTime;
    private bool isReloading;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip pistolShootSound;
    [SerializeField] private AudioClip reloadSound;

    public int CurrentAmmo => currentAmmo;
    public int MaxAmmo => maxAmmo;
    public bool IsReloading => isReloading;

    private void Awake()
    {
        currentAmmo = maxAmmo;
    }

    private void Update()
    {
        RotateToMouse();
        HandleInput();
    }

    private void HandleInput()
    {
        if (isReloading)
            return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartReload();
            return;
        }

        if (currentAmmo <= 0)
        {
            GameManager.instance.audioManager.PlaySFX("EmptyGun");
            return;
        }

        switch (weaponType)
        {
            case WeaponType.Single:
            case WeaponType.Shotgun:
                if (Input.GetMouseButtonDown(0))
                    TryShoot();
                break;

            case WeaponType.Auto:
                if (Input.GetMouseButton(0))
                    TryShoot();
                break;
        }
    }

    private void RotateToMouse()
    {
        Camera mainCamera = Camera.main;

        if (mainCamera == null || player == null)
            return;

        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = player.position.z;

        Vector2 direction = mousePosition - player.position;

        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        currentAngle = Mathf.MoveTowardsAngle(
            currentAngle,
            targetAngle,
            orbitSpeed * Time.deltaTime
        );

        Vector2 orbitPosition = new Vector2(
            Mathf.Cos(currentAngle * Mathf.Deg2Rad),
            Mathf.Sin(currentAngle * Mathf.Deg2Rad)
        ) * orbitRadius;

        transform.position = (Vector2)player.position + orbitPosition;

        transform.rotation = Quaternion.Euler(0f, 0f, currentAngle);

        Vector3 scale = transform.localScale;

        if (currentAngle > 90f || currentAngle < -90f)
            scale.y = -Mathf.Abs(scale.y);
        else
            scale.y = Mathf.Abs(scale.y);

        transform.localScale = scale;
    }

    private void TryShoot()
    {
        if (Time.time < nextFireTime)
            return;

        if (currentAmmo <= 0)
            return;

        if (projectilePrefab == null || firePoint == null)
            return;

        Camera mainCamera = Camera.main;

        if (mainCamera == null)
            return;

        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - firePoint.position).normalized;

        if (weaponType == WeaponType.Shotgun)
        {
            ShootShotgun(direction);
        }
        else
        {
            ShootProjectile();
        }

        currentAmmo--;
        nextFireTime = Time.time + fireRate;

        if (audioSource != null && pistolShootSound != null)
            audioSource.PlayOneShot(pistolShootSound);

        if (currentAmmo <= 0)
            StartReload();
    }

    private void ShootProjectile()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(
            Input.mousePosition
        );

        Vector2 direction = (
            (Vector2)mousePosition - (Vector2)firePoint.position
        ).normalized;

        GameObject projectileObject = Instantiate(
            projectilePrefab,
            firePoint.position,
            Quaternion.Euler(0f, 0f, currentAngle - 90f)
        );

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Initialize(direction);
    }

    private void ShootShotgun(Vector2 direction)
    {
        float baseAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        for (int i = 0; i < pelletsPerShot; i++)
        {
            float spread = Random.Range(-spreadAngle / 2f, spreadAngle / 2f);
            float shotAngle = baseAngle + spread;

            Vector2 shotDirection = new Vector2(
                Mathf.Cos(shotAngle * Mathf.Deg2Rad),
                Mathf.Sin(shotAngle * Mathf.Deg2Rad)
            );

            ShootProjectile();
        }
    }

    public void StartReload()
    {
        if (isReloading || currentAmmo >= maxAmmo)
            return;

        StartCoroutine(ReloadRoutine());
    }

    private IEnumerator ReloadRoutine()
    {
        isReloading = true;

        if (audioSource != null && reloadSound != null)
            audioSource.PlayOneShot(reloadSound);

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReloading = false;
    }
}