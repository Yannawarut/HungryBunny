using UnityEngine;

public class FoxChaser : MonoBehaviour
{
    public Transform player;
    public float jumpForce = 11f;
    public float catchDistance = 1.0f;
    public LayerMask groundLayer;
    public float wallCheckDistance = 0.5f;

    [Header("ระบบวิ่งไล่ (ความเร็วอิสระ)")]
    public float currentSpeed = 2f;
    public float speedPenaltyRate = 0.1f;

    [Header("ระบบหยุดรอและหันหลัง")]
    public float stopDistance = 0.5f;

    private Rigidbody2D rb;
    private Rigidbody2D playerRb;
    private PlayerMoveColtrols playerScript;
    private Animator animator;
    private Collider2D foxCollider;

    private bool isGrounded;
    private bool isGameOver = false;
    private bool isChasing = false;
    private float jumpCooldown = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        foxCollider = GetComponent<Collider2D>();

        if (player != null)
        {
            playerRb = player.GetComponent<Rigidbody2D>();
            playerScript = player.GetComponent<PlayerMoveColtrols>();
        }
    }

    void Update()
    {
        if (player == null || isGameOver) return;

        if (jumpCooldown > 0)
        {
            jumpCooldown -= Time.deltaTime;
        }

        float playerVelocityX = playerRb != null ? playerRb.linearVelocity.x : 0f;

        if (!isChasing && playerVelocityX > 0.1f)
        {
            isChasing = true;
        }

        if (!isChasing)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            SetAnimationSpeed(0f);
            return;
        }

        float distanceX = player.position.x - transform.position.x;
        float moveDirection = 0f;

        if (Mathf.Abs(distanceX) <= stopDistance)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            SetAnimationSpeed(0f);
            moveDirection = 0f;
        }
        else if (distanceX > 0)
        {
            rb.linearVelocity = new Vector2(currentSpeed, rb.linearVelocity.y);
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            SetAnimationSpeed(currentSpeed);
            moveDirection = 1f;
        }
        else
        {
            rb.linearVelocity = new Vector2(-currentSpeed, rb.linearVelocity.y);
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            SetAnimationSpeed(currentSpeed);
            moveDirection = -1f;
        }

        if (moveDirection != 0f && foxCollider != null)
        {
            Vector2 center = foxCollider.bounds.center;
            Vector2 feetPos = new Vector2(center.x, foxCollider.bounds.min.y);

            Vector2 rayStart = new Vector2(moveDirection > 0 ? foxCollider.bounds.max.x : foxCollider.bounds.min.x, center.y);
            Vector2 rayDir = moveDirection > 0 ? Vector2.right : Vector2.left;

            isGrounded = Physics2D.OverlapCircle(feetPos, 0.2f, groundLayer);
            bool wallAhead = Physics2D.Raycast(rayStart, rayDir, wallCheckDistance, groundLayer);

            Debug.DrawRay(rayStart, rayDir * wallCheckDistance, Color.red);

            // 🌟 ลบเงื่อนไข playerIsHigher ออกไปแล้ว ให้กระโดดเฉพาะตอนเจอ wallAhead เท่านั้น
            if (wallAhead && isGrounded && jumpCooldown <= 0f)
            {
                Jump();
                jumpCooldown = 0.8f;
            }
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= catchDistance)
        {
            TriggerGameOver();
        }
    }

    public void MoveCloser(float amount)
    {
        if (isGameOver) return;

        if (playerScript != null)
        {
            float speedBoost = playerScript.speed * speedPenaltyRate;
            currentSpeed += speedBoost;
        }
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerMoveColtrols>() != null)
        {
            TriggerGameOver();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerMoveColtrols>() != null)
        {
            TriggerGameOver();
        }
    }

    void TriggerGameOver()
    {
        if (isGameOver) return;
        isGameOver = true;
        rb.linearVelocity = Vector2.zero;
        SetAnimationSpeed(0f);
        if (GameManager.instance != null) GameManager.instance.TakeDamage(99);
    }

    void SetAnimationSpeed(float speedValue)
    {
        if (animator == null) return;
        animator.SetFloat("speed", speedValue);
    }
}