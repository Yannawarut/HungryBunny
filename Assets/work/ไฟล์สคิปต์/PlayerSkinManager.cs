using UnityEngine;

public class PlayerSkinManager : MonoBehaviour
{
    // ลากตัวละครแต่ละชุดมาใส่ในกล่องนี้แทน
    public GameObject[] skinObjects;

    void Awake()
    {
        // 🌟 แก้จาก "SelectedSkin" เป็น "EquippedSkin" ให้ตรงกับสคริปต์ ShopManager
        int selectedSkin = PlayerPrefs.GetInt("EquippedSkin", 0);

        // เช็คว่ามีชุดให้เลือกจริงๆ ไหม
        if (selectedSkin >= 0 && selectedSkin < skinObjects.Length)
        {
            // ทำการซ่อนชุดทั้งหมดก่อน
            for (int i = 0; i < skinObjects.Length; i++)
            {
                if (skinObjects[i] != null)
                {
                    skinObjects[i].SetActive(false);
                }
            }

            // แล้วค่อยเปิดโชว์เฉพาะชุดที่เลือก
            if (skinObjects[selectedSkin] != null)
            {
                skinObjects[selectedSkin].SetActive(true);
                Debug.Log("เปลี่ยนสกินเป็นเบอร์: " + selectedSkin);
            }
        }
    }
}