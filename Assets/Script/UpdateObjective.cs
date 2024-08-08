using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateObjective : MonoBehaviour
{
    public TextMeshProUGUI objectiveText;
    public GameObject icon;
    public GameObject bahan;
    public GameObject music;
    public AudioSource audioSource; // Referensi untuk AudioSource
    public AudioClip updateSound;   // Referensi untuk AudioClip

    public void UpdateObjectiveText(string newObjective)
    {
        objectiveText.text = newObjective;
        if (audioSource != null && updateSound != null) // Cek apakah AudioSource dan AudioClip sudah diatur
        {
            audioSource.PlayOneShot(updateSound); // Mainkan efek suara
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("8Icon"))
        {
            Destroy(icon);
            UpdateObjectiveText("Don't get caught by the Ondel-ondel\n Mission 1 : Match the traditional cloth in the Traditional cloth gallery with the information on the front");
        }
        if (other.CompareTag("Bahan"))
        {
            Destroy(bahan);
            UpdateObjectiveText("Mission 2 : Find the lost ingredients of the kerak telor on the second floor");
        }
        if (other.CompareTag("Music"))
        {
            Destroy(music);
            UpdateObjectiveText("Mission 3 : Find the right melody");
        }
    }
}
