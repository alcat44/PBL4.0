using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI2 : MonoBehaviour
{
    public NavMeshAgent ai;
    public float chaseSpeed, catchDistance, jumpscareTime, maxChaseDistance; // Batas jarak pengejaran
    public Transform player;
    public Animator aiAnim;
    public bool chasing, roaring;
    public Camera mainCamera; // Referensi ke kamera utama
    public Camera jumpscareCamera; // Referensi ke kamera jumpscare
    private Vector3 initialPlayerPosition; // Menyimpan posisi awal pemain
    public Vector3 rayCastOffset;
    public SoundEffectsPlayer1 Audio; // Menggunakan Audio untuk sound effects
    private AudioSource audioSource; // Referensi ke AudioSource
    private string previousAnimationState = ""; // Menyimpan status animasi sebelumnya

    void Start()
    {
        chasing = true;
        initialPlayerPosition = player.position; // Simpan posisi awal pemain

        if (jumpscareCamera != null)
        {
            jumpscareCamera.gameObject.SetActive(false); // Nonaktifkan kamera jumpscare pada awalnya
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found on the GameObject.");
        }
    }

    void Update()
    {
        float distance = Vector3.Distance(player.position, ai.transform.position);

        // Hentikan pengejaran jika jarak melebihi batas maksimum
        if (distance > maxChaseDistance)
        {
            StopChasing();
            return; // Keluar dari Update untuk menghindari pengejaran lebih lanjut
        }

        // Pengejaran aktif
        if (chasing)
        {
            ai.destination = player.position;
            ai.speed = chaseSpeed;

            // Pastikan animasi dalam mode sprint
            if (previousAnimationState != "sprint")
            {
                aiAnim.ResetTrigger("roar");
                aiAnim.SetTrigger("sprint");
                PlayChaseSound();
                previousAnimationState = "sprint";
            }

            // Memastikan NavMeshAgent mengikuti jalur
            if (ai.pathPending || ai.pathStatus == NavMeshPathStatus.PathPartial)
            {
                return; // Jika path belum siap atau tidak lengkap, keluar
            }

            // Tangkap pemain jika berada dalam jarak tangkap
            if (distance <= catchDistance)
            {
                Debug.Log("Player caught!");
                player.gameObject.SetActive(false);
                aiAnim.ResetTrigger("sprint");
                aiAnim.ResetTrigger("roar");
                StartCoroutine(deathRoutine());
                chasing = false; // Hentikan pengejaran
            }
        }
    }

    void PlayChaseSound()
    {
        // Mainkan sound effect saat musuh mulai mengejar
        if (!audioSource.isPlaying)
        {
            Audio.bgm2();
            Audio.sfx2(); // Mainkan sound effect pengejaran
        }
    }

    void StopChasing()
    {
        ai.destination = transform.position; // Set destination ke posisi musuh untuk berhenti
        ai.speed = 0;

        if (audioSource.isPlaying)
        {
            audioSource.Stop(); // Hentikan sound effect
        }
    }

    IEnumerator deathRoutine()
    {
        if (jumpscareCamera != null && mainCamera != null)
        {
            Debug.Log("Activating jumpscare camera");
            Audio.SFXSource.Stop();
            Audio.sfx3(); // Mainkan sound effect jumpscare
            chasing = false;
            mainCamera.gameObject.SetActive(false); // Nonaktifkan kamera utama
            jumpscareCamera.gameObject.SetActive(true); // Aktifkan kamera jumpscare
        }
        else
        {
            Debug.LogWarning("Jumpscare camera or main camera not assigned.");
        }

        yield return new WaitForSeconds(jumpscareTime);

        if (jumpscareCamera != null && mainCamera != null)
        {
            Debug.Log("Deactivating jumpscare camera and reactivating main camera");
            roaring = false;
            jumpscareCamera.gameObject.SetActive(false); // Nonaktifkan kamera jumpscare
            mainCamera.gameObject.SetActive(true); // Aktifkan kembali kamera utama
        }

        Audio.Chasemusicbg.Stop(); // Hentikan musik pengejaran

        // Kembalikan posisi pemain ke posisi awal
        player.position = initialPlayerPosition;
        player.gameObject.SetActive(true); // Aktifkan kembali pemain

        // Kembalikan musuh ke posisi awal
        StopChasing(); // Berhentikan pengejaran dan sound effect

        Debug.Log("Death routine completed.");
    }
}
