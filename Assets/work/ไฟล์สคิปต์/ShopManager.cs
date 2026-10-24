using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ShopManager : MonoBehaviour
{
    [Header("UI Settings")]
    public TextMeshProUGUI[] statusTexts; // ลาก Text ของปุ่ม Buy/Equip มาใส่ให้ครบ 5 ช่อง
    public TextMeshProUGUI myCoinText;    // ลาก MyCoinText มาใส่

    [Header("Skin Prices")]
    // ราคาของสกินเรียงตามลำดับ (สกินแรกฟรี=0, นอกนั้น 20)
    public int[] skinPrices = { 0, 20, 20, 20, 20 };

    void Start()
    {
        // สกินช่องที่ 0 (สกินเริ่มต้น) ให้ปลดล็อกฟรีเสมอ
        PlayerPrefs.SetInt("Skin0Unlocked", 1);
        UpdateUI();
    }

    // 🌟 ฟังก์ชันเดียวใช้กับปุ่มสกินได้ทั้งหมด!
    public void ClickSkin(int skinIndex)
    {
        // เช็กว่าสกินนี้เคยซื้อไปหรือยัง (1 = ซื้อแล้ว, 0 = ยัง)
        bool isUnlocked = PlayerPrefs.GetInt("Skin" + skinIndex + "Unlocked", 0) == 1;

        if (isUnlocked)
        {
            // ถ้าปลดล็อกแล้ว -> สั่งสวมใส่
            PlayerPrefs.SetInt("EquippedSkin", skinIndex);
        }
        else
        {
            // ถ้ายังไม่ปลดล็อก -> เข้าสู่ระบบซื้อ
            int totalCarrots = PlayerPrefs.GetInt("TotalCarrots", 0);

            if (totalCarrots >= skinPrices[skinIndex])
            {
                // หักเงิน
                PlayerPrefs.SetInt("TotalCarrots", totalCarrots - skinPrices[skinIndex]);
                // ปลดล็อกสกิน
                PlayerPrefs.SetInt("Skin" + skinIndex + "Unlocked", 1);
                // สวมใส่ทันที
                PlayerPrefs.SetInt("EquippedSkin", skinIndex);
            }
            else
            {
                Debug.Log("แครอทไม่พอซื้อสกินนี้!");
                return; // เงินไม่พอ ให้ออกจากฟังก์ชัน ไม่ต้องอัปเดตหน้าจอ
            }
        }

        PlayerPrefs.Save();
        UpdateUI(); // รีเฟรชหน้าจอหลังจากซื้อหรือสวมใส่เสร็จ
    }

    // ระบบเปลี่ยนข้อความและสี
    private void UpdateUI()
    {
        // 1. อัปเดตยอดเงินแครอท
        if (myCoinText != null)
        {
            int totalCarrots = PlayerPrefs.GetInt("TotalCarrots", 0);
            myCoinText.text = "My Carrots: " + totalCarrots;
        }

        // 2. อัปเดตข้อความบนปุ่มทั้ง 5 สกิน
        int equippedSkin = PlayerPrefs.GetInt("EquippedSkin", 0);

        for (int i = 0; i < statusTexts.Length; i++)
        {
            if (statusTexts[i] != null)
            {
                bool isUnlocked = PlayerPrefs.GetInt("Skin" + i + "Unlocked", 0) == 1;

                if (i == equippedSkin)
                {
                    statusTexts[i].text = "Equipped";
                    statusTexts[i].color = Color.green; // กำลังใส่อยู่ -> สีเขียว
                }
                else if (isUnlocked)
                {
                    statusTexts[i].text = "Equip";
                    statusTexts[i].color = Color.black; // ซื้อแล้วแต่ไม่ได้ใส่ -> สีดำ
                }
                else
                {
                    statusTexts[i].text = "Buy";
                    statusTexts[i].color = Color.black; // ยังไม่ซื้อ -> สีดำ
                }
            }
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}