using System.Collections;
using UnityEngine;

public class JumpscareLight : MonoBehaviour
{
    public AudioSource scareSound;
    public Collider collision;
    public GameObject Jumpscare;
    public GameObject light; // Lampu yang akan dimatikan
    public Renderer lightBulb;
    public Material offlight;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Jumpscare.SetActive(true);
            collision.enabled = false;
            MatikanLampu();
            MainkanSuaraTakut();
            StartCoroutine(NonaktifkanJumpscareSetelahDelay(4.0f)); // Memanggil Coroutine
            NonaktifkanTrigger();
        }
    }

    void MatikanLampu()
    {
        if (light != null)
        {
            light.SetActive(false);
            if (lightBulb != null && offlight != null)
            {
                lightBulb.material = offlight;
            }
        }
    }

    void MainkanSuaraTakut()
    {
        if (scareSound != null)
        {
            scareSound.Play();
        }
    }

    IEnumerator NonaktifkanJumpscareSetelahDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Jumpscare.SetActive(false); // Menghilangkan jumpscare setelah delay
    }

    void NonaktifkanTrigger()
    {
        if (collision != null)
        {
            collision.enabled = false; // Menonaktifkan trigger
        }
    }
}
