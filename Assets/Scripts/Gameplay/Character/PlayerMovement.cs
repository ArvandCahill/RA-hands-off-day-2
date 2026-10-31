using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Gun")]
    public Gun gun;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    private Rigidbody2D rb;
    private PlayerStats playerStats;
    private Vector2 moveInput;
    private Vector2 lastMoveDirection = Vector2.down;

    private static readonly int MoveX = Animator.StringToHash("moveX");
    private static readonly int MoveY = Animator.StringToHash("moveY");
    private static readonly int MoveMagnitude = Animator.StringToHash("moveMagnitude");
    private static readonly int LastMoveX = Animator.StringToHash("lastMoveX");
    private static readonly int LastMoveY = Animator.StringToHash("lastMoveY");

    public Vector2 LastMoveDirection => lastMoveDirection;

    private float CurrentMoveSpeed =>
        moveSpeed * (playerStats != null
            ? playerStats.MovementSpeedMultiplier
            : 1f);

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerStats = GetComponent<PlayerStats>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        moveInput = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        ).normalized;

        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveInput * CurrentMoveSpeed;
    }

    private void UpdateAnimation()
    {
        animator.SetFloat(MoveX, moveInput.x);
        animator.SetFloat(MoveY, moveInput.y);
        animator.SetFloat(MoveMagnitude, moveInput.sqrMagnitude);

        if (moveInput != Vector2.zero)
        {
            lastMoveDirection = moveInput;

            animator.SetFloat(LastMoveX, lastMoveDirection.x);
            animator.SetFloat(LastMoveY, lastMoveDirection.y);
        }
    }
}