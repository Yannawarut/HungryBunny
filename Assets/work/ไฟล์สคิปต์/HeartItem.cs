using UnityEngine;

public class HeartItem : MonoBehaviour
{
    public int healAmount = 1; // จำนวนเลือดที่จะเพิ่ม
    private Transform playerTransform;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    void Update()
    {
        // ระบบลบตัวเองเมื่อกระต่ายวิ่งผ่านไป 30 บล็อก
        if (playerTransform != null)
        {
            if (playerTransform.position.x - transform.position.x > 30f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (GameManager.instance != null)
            {
                // ส่งคำสั่งไปบอก GameManager ให้เพิ่มเลือด
                GameManager.instance.Heal(healAmount);
            }

            // ลบตัวหัวใจออกจากฉากทันทีที่เก็บ (ลบเสมอแม้เลือดจะเต็มแล้วก็ตาม)
            Destroy(gameObject);
        }
    }
}