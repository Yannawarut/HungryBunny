using UnityEngine;

public class CarrotSpawner : MonoBehaviour
{
    public GameObject carrotPrefab;
    public Transform player;
    public float spawnInterval = 3f;
    public float spawnDistanceX = 12f;

    [Header("ระบบเช็คระยะห่าง (ป้องกันเกิดทับกัน)")]
    public float minSpacingX = 5f;

    [Header("Ground Check Settings")]
    public LayerMask groundLayer;
    public float carrotOffsetY = 0.5f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            if (SpawnCarrotOnGround())
            {
                timer = 0f;
            }
        }
    }

    bool SpawnCarrotOnGround()
    {
        // 🌟 เพิ่มการเช็ก GameManager.instance ป้องกัน Error
        if (carrotPrefab == null || player == null || GameManager.instance == null) return false;

        float rawX = player.position.x + spawnDistanceX;
        float targetX = Mathf.Floor(rawX) + 0.5f;

        // 🌟 แก้ Error เครื่องหมายลบ และเปลี่ยนไปใช้ความจำร่วมจาก GameManager
        if (Mathf.Abs(targetX - GameManager.instance.globalLastSpawnX) < minSpacingX)
        {
            return false;
        }

        Vector2 rayOrigin = new Vector2(targetX, 10f);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, 20f, groundLayer);

        if (hit.collider != null)
        {
            Vector3 spawnPosition = new Vector3(targetX, hit.point.y + carrotOffsetY, 0f);
            Instantiate(carrotPrefab, spawnPosition, Quaternion.identity);

            // 🌟 บันทึกตำแหน่งล่าสุดกลับไปที่ระบบส่วนกลาง
            GameManager.instance.globalLastSpawnX = targetX;

            return true;
        }

        return false;
    }
}