using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public Transform player;
    public float spawnInterval = 5f;
    public float spawnDistanceX = 15f;

    [Header("ระบบเช็คระยะห่าง (ป้องกันเกิดทับกัน)")]
    public float minSpacingX = 5f;

    [Header("Ground Check Settings")]
    public LayerMask groundLayer;
    public float offsetY = 0.2f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            if (SpawnObstacleOnGround())
            {
                timer = 0f;
            }
        }
    }

    bool SpawnObstacleOnGround()
    {
        // 🌟 เพิ่มการเช็ก GameManager.instance ป้องกัน Error
        if (obstaclePrefab == null || player == null || GameManager.instance == null) return false;

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
            Vector3 spawnPosition = new Vector3(targetX, hit.point.y + offsetY, 0f);
            Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);

            // 🌟 บันทึกตำแหน่งล่าสุดกลับไปที่ระบบส่วนกลาง
            GameManager.instance.globalLastSpawnX = targetX;
            return true;
        }

        return false;
    }
}