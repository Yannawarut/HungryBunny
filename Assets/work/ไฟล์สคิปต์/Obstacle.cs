using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public int damage = 1; // ดาเมจที่จะลด
    private Transform playerTransform;

    private void Start()
    {
        // ทันทีที่เกิดมา ให้มันมองหาผู้เล่น
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    private void Update()
    {
        // เช็คตลอดเวลาว่าผู้เล่นเดินเลยหน้าเราไปหรือยัง
        if (playerTransform != null)
        {
            // เปลี่ยนจาก 50 เป็น 30 เพื่อให้ลบตัวเองพร้อมๆ กับแคร์รอต
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
                GameManager.instance.TakeDamage(damage);
            }
            // ป้องกันการชนซ้ำซ้อน ลบตัวมันเองทิ้งหลังโดนชน
            Destroy(gameObject);
        }
    }
}