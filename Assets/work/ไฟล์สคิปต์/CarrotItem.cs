using UnityEngine;

public class CarrotItem : MonoBehaviour
{
    public int pointValue = 1; // จำนวนคะแนนที่จะได้รับ

    // 🌟 [ส่วนที่เพิ่มใหม่] ตัวแปรสำหรับจำตำแหน่งกระต่าย
    private Transform playerTransform;

    void Start()
    {
        // 🌟 [ส่วนที่เพิ่มใหม่] ทันทีที่แคร์รอตเกิดมา ให้มันค้นหากระต่ายในฉาก
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    void Update()
    {
        // 🌟 [ส่วนที่เพิ่มใหม่] เช็คตลอดเวลาว่ากระต่ายวิ่งเลยไป 30 บล็อกหรือยัง
        if (playerTransform != null)
        {
            // ถ้าตำแหน่งแกน X ของกระต่าย มากกว่าแคร์รอตเกิน 30 ให้ลบแคร์รอตทิ้ง
            if (playerTransform.position.x - transform.position.x > 30f)
            {
                Destroy(gameObject);
            }
        }
    }

    // โค้ดเดิมของคุณ (ระบบเก็บคะแนน)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ตรวจจับว่าวัตถุที่มาชนคือกระต่าย (Tag: Player) หรือไม่
        if (collision.CompareTag("Player"))
        {
            // ส่งคะแนนไปเพิ่มใน GameManager
            if (GameManager.instance != null)
            {
                GameManager.instance.AddScore(pointValue);
            }

            Debug.Log("+1 Carrot Collected!");

            // ลบตัวแคร์รอตออกจากฉาก
            Destroy(gameObject);
        }
    }
}