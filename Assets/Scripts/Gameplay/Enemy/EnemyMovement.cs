using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    [Header("Combat")]
    [SerializeField] private int damage = 1;
    [SerializeField] private float damageCooldown = 1f;
    [SerializeField] private int exp = 1;

    private Transform player;
    private Rigidbody2D rb;

    private Vector2 moveDirection;
    private Vector2 lastMoveDirection = Vector2.down;

    private float nextDamageTime;
    private static readonly int MoveX = Animator.StringToHash("moveX");
    private static readonly int MoveY = Animator.StringToHash("moveY");
    private static readonly int MoveMagnitude = Animator.StringToHash("moveMagnitude"); 
    private static readonly int LastMoveX = Animator.StringToHash("lastMoveX");
    private static readonly int LastMoveY = Animator.StringToHash("lastMoveY");

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
            player = playerObject.transform;
    }

    private void Update()
    {
        CalculateMovement();
        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveDirection * moveSpeed;
    }

    private void OnDestroy()
    {
        if (LevelManager.instance != null)
            LevelManager.instance.AddExp(exp);
    }

    private void CalculateMovement()
    {
        if (player == null)
        {
            moveDirection = Vector2.zero;
            return;
        }

        moveDirection =
            ((Vector2)player.position - rb.position).normalized;
    }

    private void UpdateAnimation()
    {
        if (animator == null)
            return;

        animator.SetFloat(MoveX, moveDirection.x);
        animator.SetFloat(MoveY, moveDirection.y);
        animator.SetFloat(MoveMagnitude, moveDirection.sqrMagnitude);

        if (moveDirection.sqrMagnitude > 0.001f)
        {
            lastMoveDirection = moveDirection;

            animator.SetFloat(LastMoveX, lastMoveDirection.x);
            animator.SetFloat(LastMoveY, lastMoveDirection.y);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        if (Time.time < nextDamageTime)
            return;

        Health health = collision.GetComponent<Health>();

        if (health == null)
            return;

        health.TakeDamage(damage);
        nextDamageTime = Time.time + damageCooldown;
    }
}