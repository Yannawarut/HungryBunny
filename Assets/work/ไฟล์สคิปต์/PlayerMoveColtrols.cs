using UnityEngine;

public class PlayerMoveColtrols : MonoBehaviour
{
    [Header("ระบบความเร็ว")]
    public float speed = 5f;
    public float maxSpeed = 15f;
    public float speedIncreaseRate = 0.5f;

    [Header("ระบบกระโดด")]
    public float jumpForce = 10f;
    public float rayLength = 0.2f;
    public LayerMask groundLayer;
    public Transform LeftPoint;
    public Transform RightPoint;

    private int direction = 1;
    private GatherInput gatherInput;
    private Rigidbody2D rigidbody2d;
    private Animator animator;
    private bool grounded = false;
    private bool canDoubleJump = false;

    public bool isMoving = false;

    void Start()
    {
        gatherInput = GetComponent<GatherInput>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        animator = GetComponentInChildren<Animator>();
        if (animator == null) return;

        CheckStatus();
        Move();
        Jump();
        SetAnimatorValues();

        if (rigidbody2d.linearVelocity.y < 0)
        {
            rigidbody2d.linearVelocity += Vector2.up * Physics2D.gravity.y * 2.5f * Time.deltaTime;
        }

        // 🌟 [เพิ่มระบบเสียง] จัดการเสียงเดินตามสถานะการเคลื่อนที่และการเหยียบพื้น
        if (AudioManager.instance != null)
        {
            AudioManager.instance.ManageWalkSound(isMoving && grounded);
        }
    }

    private void CheckStatus()
    {
        bool leftCheck = Physics2D.Raycast(LeftPoint.position, Vector2.down, rayLength, groundLayer);
        bool rightCheck = Physics2D.Raycast(RightPoint.position, Vector2.down, rayLength, groundLayer);

        grounded = leftCheck || rightCheck;

        if (grounded)
        {
            canDoubleJump = true;
        }
    }

    private void SetAnimatorValues()
    {
        if (animator == null) return;

        float currentSpeed = Mathf.Abs(rigidbody2d.linearVelocity.x);
        animator.SetFloat("Speed", currentSpeed);
        animator.SetFloat("vSpeed", rigidbody2d.linearVelocity.y);
        animator.SetBool("Grounded", grounded);
    }

    private void Move()
    {
        if (gatherInput.valueX != 0)
        {
            isMoving = true;
            if (speed < maxSpeed)
            {
                speed += speedIncreaseRate * Time.deltaTime;
            }
        }
        else
        {
            isMoving = false;
        }

        Flip();
        rigidbody2d.linearVelocity = new Vector2(gatherInput.valueX * speed, rigidbody2d.linearVelocity.y);
    }

    private void Flip()
    {
        if (gatherInput.valueX * direction < 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            direction *= -1;
        }
    }

    private void Jump()
    {
        if (gatherInput.jumpInput)
        {
            if (grounded)
            {
                rigidbody2d.linearVelocity = new Vector2(rigidbody2d.linearVelocity.x, jumpForce);

                // 🌟 [เพิ่มระบบเสียง] เล่นเสียงกระโดดครั้งแรก
                if (AudioManager.instance != null) AudioManager.instance.PlaySFX(AudioManager.instance.jumpClip);
            }
            else if (canDoubleJump)
            {
                rigidbody2d.linearVelocity = new Vector2(rigidbody2d.linearVelocity.x, jumpForce);
                canDoubleJump = false;

                // 🌟 [เพิ่มระบบเสียง] เล่นเสียงกระโดดกลางอากาศ
                if (AudioManager.instance != null) AudioManager.instance.PlaySFX(AudioManager.instance.jumpClip);
            }
        }
        gatherInput.jumpInput = false;
    }
}