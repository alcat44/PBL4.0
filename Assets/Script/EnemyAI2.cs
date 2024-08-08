using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI2 : MonoBehaviour
{
    public NavMeshAgent ai;
    public float chaseSpeed, catchDistance, jumpscareTime;
    public Transform player;
    public Camera mainCamera; // Referensi ke kamera utama
    public Camera jumpscareCamera; // Referensi ke kamera jumpscare
    private Vector3 initialPlayerPosition; // Menyimpan posisi awal pemain
    public Vector3 rayCastOffset;
    public AudioClip chaseSound; // Referensi ke sound effect saat musuh mengejar
    private AudioSource audioSource; // Referensi ke AudioSource
    public float maxChaseDistance; // Batas maksimum jarak pengejaran

    void Start()
    {
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

        // Hentikan pengejaran jika jarak melebihi batas yang ditentukan
        if (distance > maxChaseDistance)
        {
            // Hentikan NavMeshAgent dan sound effect
            ai.destination = transform.position;
            ai.speed = 0;

            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            return; // Keluar dari update jika di luar jarak maksimum
        }
        
        // Lanjutkan pengejaran jika jarak di dalam batas maksimum
        ai.destination = player.position;
        ai.speed = chaseSpeed;

        // Memastikan NavMeshAgent mengikuti jalur
        if (ai.pathPending || ai.pathStatus == NavMeshPathStatus.PathPartial)
        {
            return; // Jika path belum siap atau tidak lengkap, keluar
        }

        if (distance <= catchDistance)
        {
            Debug.Log("Player caught!");
            player.gameObject.SetActive(false);
            StartCoroutine(deathRoutine());
        }

        // Mainkan sound effect saat musuh mulai mengejar
        if (!audioSource.isPlaying && chaseSound != null)
        {
            audioSource.clip = chaseSound;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    IEnumerator deathRoutine()
    {
        if (jumpscareCamera != null && mainCamera != null)
        {
            mainCamera.gameObject.SetActive(false); // Nonaktifkan kamera utama
            jumpscareCamera.gameObject.SetActive(true); // Aktifkan kamera jumpscare
        }
        yield return new WaitForSeconds(jumpscareTime);
        if (jumpscareCamera != null && mainCamera != null)
        {
            jumpscareCamera.gameObject.SetActive(false); // Nonaktifkan kamera jumpscare
            mainCamera.gameObject.SetActive(true); // Aktifkan kembali kamera utama
        }
        player.position = initialPlayerPosition; // Kembalikan posisi pemain ke posisi awal
        player.gameObject.SetActive(true); // Aktifkan kembali pemain
        // Kembalikan musuh ke posisi awal jika diperlukan
        ai.destination = transform.position; // Set destination ke posisi musuh untuk berhenti
        ai.speed = 0;

        // Hentikan sound effect saat musuh berhenti mengejar
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}