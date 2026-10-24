using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [Header("--- หน้าต่าง UI ---")]
    public GameObject shopPanel;
    public TMP_Text coinText;

    private int totalCarrots = 0;

    void Start()
    {
        totalCarrots = PlayerPrefs.GetInt("TotalCarrots", 0);
        // ชุดเริ่มต้น (เบอร์ 0) จะซื้อไว้ตั้งแต่เริ่มเกมเสมอ
        PlayerPrefs.SetInt("SkinOwned_0", 1);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void OpenShop()
    {
        if (shopPanel != null) shopPanel.SetActive(true);
        totalCarrots = PlayerPrefs.GetInt("TotalCarrots", 0);
        UpdateCoinUI();
    }

    public void CloseShop()
    {
        if (shopPanel != null) shopPanel.SetActive(false);
    }

    void UpdateCoinUI()
    {
        if (coinText != null) coinText.text = "My Carrots: " + totalCarrots.ToString();
    }

    // 🌟 ระบบกดปุ่มสำหรับสกิน (รับรหัสสกิน และ ราคา)
    public void BuyOrEquipSkin(int skinIndex, int price)
    {
        // เช็คก่อนว่ามีสกินนี้ในครอบครองหรือยัง (1 = มีแล้ว, 0 = ยังไม่มี)
        int isOwned = PlayerPrefs.GetInt("SkinOwned_" + skinIndex, 0);

        if (isOwned == 1)
        {
            // ถ้ามีแล้ว -> ถือว่าเป็นการกดเพื่อ "เลือกใส่ (Equip)"
            PlayerPrefs.SetInt("SelectedSkin", skinIndex);
            Debug.Log("เลือกใส่สกินเบอร์: " + skinIndex);
        }
        else
        {
            // ถ้ายังไม่มี -> ถือว่าเป็นการกดเพื่อ "ซื้อ (Buy)"
            if (totalCarrots >= price)
            {
                totalCarrots -= price;
                PlayerPrefs.SetInt("TotalCarrots", totalCarrots);
                PlayerPrefs.SetInt("SkinOwned_" + skinIndex, 1); // บันทึกว่าซื้อสกินนี้แล้ว
                PlayerPrefs.SetInt("SelectedSkin", skinIndex); // ซื้อปุ๊บก็เลือกใส่ให้เลย
                UpdateCoinUI();
                Debug.Log("ซื้อและเลือกใส่สกินเบอร์: " + skinIndex);
            }
            else
            {
                Debug.Log("แครอทไม่พอจ้า!");
            }
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}