using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("ลาก Audio Source ทั้ง 3 อันมาใส่")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;
    public AudioSource walkSource;

    [Header("ลากไฟล์เสียงมาใส่ให้ครบ")]
    public AudioClip bgmClip;         // 1. เสียงคลอ
    public AudioClip walkClip;        // 2. เสียงเดิน
    public AudioClip jumpClip;        // 3. เสียงกระโดด
    public AudioClip collectCarrot;   // 4. เสียงเก็บแคร์รอต
    public AudioClip collectHeart;    // 5. เสียงเก็บหัวใจ
    public AudioClip hitTrap;         // 6. เสียงโดนกับดัก
    public AudioClip buttonClick;     // 7. เสียงปุ่มกด

    private void Awake()
    {
        // ทำให้มี AudioManager ตัวเดียวตลอดทั้งเกม
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // เริ่มเกมมาให้เล่นเสียงคลอ (BGM) ทันที
        if (bgmClip != null)
        {
            bgmSource.clip = bgmClip;
            bgmSource.Play();
        }

        // ตั้งค่าเสียงเดินเตรียมไว้ แต่ยังไม่เล่น
        if (walkClip != null)
        {
            walkSource.clip = walkClip;
        }
    }

    // ฟังก์ชันสำหรับเล่นเสียง Effect (ใช้ PlayOneShot เพื่อให้เสียงเล่นทับซ้อนกันได้)
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    // ระบบควบคุมเสียงเดิน (เล่นเฉพาะตอนเท้าติดพื้นและกดเดิน)
    public void ManageWalkSound(bool isWalking)
    {
        if (isWalking && !walkSource.isPlaying)
        {
            walkSource.Play();
        }
        else if (!isWalking && walkSource.isPlaying)
        {
            walkSource.Stop();
        }
    }
}