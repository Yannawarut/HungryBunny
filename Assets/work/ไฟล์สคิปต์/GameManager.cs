using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("--- ตัวละครกระต่าย ---")]
    public PlayerMoveColtrols playerScript;
    [Header("--- ระบบความจำ Spawner ---")]
    public float globalLastSpawnX = -9999f; // ให้ทุก Spawner จำตำแหน่งร่วมกัน
    [Header("--- คะแนน (Score) ---")]
    public int score = 0;
    public TMP_Text scoreText;

    [Header("--- ระยะทาง (Distance) ---")]
    public float distance = 0f;
    public TMP_Text distanceText;

    [Header("--- พลังชีวิต (Health) ---")]
    public int maxHealth = 3;
    public int currentHealth;

    [Header("--- ลากหัวใจ 3 ดวงมาใส่ 3 ช่องนี้ ---")]
    public GameObject heart1;
    public GameObject heart2;
    public GameObject heart3;

    [Header("--- หน้าจอ Game Over ---")]
    public GameObject gameOverPanel;
    public TMP_Text finalScoreText;

    private FoxChaser fox;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        Time.timeScale = 1f;
        currentHealth = maxHealth;
        distance = 0f;
        UpdateScoreUI();
        UpdateHealthUI();

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        fox = FindAnyObjectByType<FoxChaser>();

        if (playerScript == null)
        {
            playerScript = FindAnyObjectByType<PlayerMoveColtrols>();
        }
    }

    void Update()
    {
        if (currentHealth > 0 && playerScript != null && playerScript.isMoving == true)
        {
            distance += Time.deltaTime * 10f;

            if (distanceText != null)
            {
                distanceText.text = "Distance: " + Mathf.FloorToInt(distance).ToString() + " m";
            }
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();

        // 🌟 [เพิ่มระบบเสียง] เล่นเสียงเก็บแครอท
        if (AudioManager.instance != null) AudioManager.instance.PlaySFX(AudioManager.instance.collectCarrot);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        UpdateHealthUI();

        // 🌟 [เพิ่มระบบเสียง] เล่นเสียงโดนกับดัก
        if (AudioManager.instance != null) AudioManager.instance.PlaySFX(AudioManager.instance.hitTrap);

        if (fox != null)
        {
            fox.MoveCloser(3f);
        }

        if (currentHealth <= 0)
        {
            GameOver();
        }
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }

    void UpdateHealthUI()
    {
        if (heart1 != null) heart1.SetActive(currentHealth >= 1);
        if (heart2 != null) heart2.SetActive(currentHealth >= 2);
        if (heart3 != null) heart3.SetActive(currentHealth >= 3);
    }

    void GameOver()
    {
        Debug.Log("GAME OVER! พลังชีวิตหมดแล้ว");
        Time.timeScale = 0f;

        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        if (finalScoreText != null) finalScoreText.text = "Score: " + score.ToString();

        PlayerPrefs.SetInt("TotalCarrots", PlayerPrefs.GetInt("TotalCarrots", 0) + score);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void Heal(int amount)
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += amount;

            if (currentHealth > maxHealth) currentHealth = maxHealth;

            Debug.Log("เก็บหัวใจได้! เลือดเพิ่มเป็น: " + currentHealth);

            UpdateHealthUI();

            // 🌟 [เพิ่มระบบเสียง] เล่นเสียงเก็บหัวใจ
            if (AudioManager.instance != null) AudioManager.instance.PlaySFX(AudioManager.instance.collectHeart);
        }
        else
        {
            Debug.Log("เลือดเต็ม 3 อยู่แล้ว เก็บหัวใจไปก็ไม่เพิ่ม");
        }
    }
}