using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("ตั้งค่าด่าน")]
    public GameObject[] levelPrefabs;   // ช่องสำหรับใส่ Prefab ด่านต่างๆ ที่เราทำไว้
    public float chunkLength = 110f;     // ความยาวของด่านแต่ละชิ้น (ต้องตั้งให้พอดีกับความกว้างของ Tilemap ที่คุณวาด)
    public int initialChunks = 3;       // จำนวนชิ้นส่วนที่อยากให้สร้างรอไว้ตอนเริ่มเกม

    [Header("การอ้างอิง")]
    public Transform player;            // ตำแหน่งของผู้เล่น
    public float safeZone = 150f;        // ระยะห่างด้านหลังผู้เล่น (ถ้าผู้เล่นเดินเลยระยะนี้ไป ด่านจะถูกลบ)

    private float spawnX = 0f;          // จุด X ที่จะสร้างด่านชิ้นถัดไป
    private List<GameObject> activeChunks = new List<GameObject>(); // เก็บรายชื่อด่านที่กำลังโชว์อยู่

    void Start()
    {
        // เริ่มเกมมา ให้สุ่มสร้างด่านรอไว้ก่อนตามจำนวน initialChunks
        for (int i = 0; i < initialChunks; i++)
        {
            SpawnChunk();
        }
    }

    void Update()
    {
        // เช็กว่าตำแหน่งของผู้เล่น ลบด้วย safeZone มีค่ามากกว่า ตำแหน่งของด่านชิ้นแรกสุดในลิสต์หรือไม่
        // ถ้าใช่ แปลว่าผู้เล่นเดินผ่านชิ้นแรกไปไกลแล้ว ให้สร้างชิ้นใหม่ต่อท้าย แล้วลบชิ้นแรกทิ้ง
        if (player.position.x - safeZone > activeChunks[0].transform.position.x)
        {
            SpawnChunk();
            DeleteChunk();
        }
    }

    void SpawnChunk()
    {
        // สุ่มเลือก Prefab ด่านจากอาเรย์
        int randomIndex = Random.Range(0, levelPrefabs.Length);

        // สร้างด่านขึ้นมาที่ตำแหน่ง spawnX
        GameObject chunk = Instantiate(levelPrefabs[randomIndex], new Vector3(spawnX, 0, 0), Quaternion.identity);

        // เก็บด่านที่สร้างไว้ในลิสต์
        activeChunks.Add(chunk);

        // เลื่อนจุด spawnX ไปข้างหน้า ตามความยาวของด่าน เพื่อเตรียมสร้างชิ้นต่อไป
        spawnX += chunkLength;
    }

    void DeleteChunk()
    {
        // ทำลาย GameObject ของด่านชิ้นแรกสุดทิ้ง
        Destroy(activeChunks[0]);
        // ลบข้อมูลออกจากลิสต์
        activeChunks.RemoveAt(0);
    }
}