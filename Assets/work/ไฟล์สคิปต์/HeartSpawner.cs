using UnityEngine;

public class HeartSpawner : MonoBehaviour
{
    public GameObject heartPrefab;
    public Transform player;

    [Header("ตั้งค่าความถี่ (ให้ออกยากกว่าแครอท)")]
    // แครอทเกิดทุก 3 วิ, กับดัก 5 วิ เราตั้งหัวใจไว้ที่ 15 วินาที (น้อยกว่า 3-5 เท่า)
    public float spawnInterval = 15f;
    public float spawnDistanceX = 20f; // เกิดไกลกว่าเดิมนิดนึงเพื่อให้เห็นล่วงหน้า

    [Header("ระบบเช็คระยะห่าง (ป้องกันเกิดทับกัน)")]
    public float minSpacingX = 5f;
    private float lastSpawnX = -9999f;

    [Header("Ground Check Settings")]
    public LayerMask groundLayer;
    public float offsetY = 0.5f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            if (SpawnHeartOnGround())
            {
                timer = 0f;
            }
        }
    }

    bool SpawnHeartOnGround()
    {
        if (heartPrefab == null || player == null) return false;

        float rawX = player.position.x + spawnDistanceX;
        float targetX = Mathf.Floor(rawX) + 0.5f;

        // เช็คระยะห่างป้องกันทับซ้อน
        if (Mathf.Abs(targetX - lastSpawnX) < minSpacingX)
        {
            return false;
        }

        Vector2 rayOrigin = new Vector2(targetX, 10f);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, 20f, groundLayer);

        if (hit.collider != null)
        {
            Vector3 spawnPosition = new Vector3(targetX, hit.point.y + offsetY, 0f);
            Instantiate(heartPrefab, spawnPosition, Quaternion.identity);

            lastSpawnX = targetX;
            return true;
        }

        return false;
    }
}