using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateObjective : MonoBehaviour
{
    public TextMeshProUGUI objectiveText;
    public GameObject pickupText;
    public GameObject icon;
    public GameObject bahan;
    public GameObject music;
    public AudioSource audioSource; // Referensi untuk AudioSource
    public AudioClip updateSound;   // Referensi untuk AudioClip
     public bool interactable;

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
            UpdateObjectiveText("⊙ Don't get caught by the Ondel-ondel\n⊙ Mission 1: Match the traditional clothes in the traditional clothes gallery with the information on the front");
        }
        if (other.CompareTag("Bahan"))
        {
            Destroy(bahan);
            UpdateObjectiveText("⊙ Mission 2: Find the lost ingredients of the kerak telor on the 2nd floor");
        }
        if (other.CompareTag("Music"))
        {
            Destroy(music);
            UpdateObjectiveText("⊙ Mission 3: Find the right melody");
        }
    }
    public GameObject inspectCanvas;
    bool isActive;

    private void Start()
    {
        interactable = false;
        if (inspectCanvas != null)
        {
            inspectCanvas.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Inspect") )
        {
            if(isActive || interactable)
            {
                pickupText.SetActive(true);
            }
            if(!isActive || !interactable)
            {
                pickupText.SetActive(false);
            }
            interactable = true;
            if (inspectCanvas != null && interactable && Input.GetKeyDown(KeyCode.E))
            {
                isActive = inspectCanvas.activeSelf;
                inspectCanvas.SetActive(!isActive);
                pickupText.SetActive(false);
                interactable = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Inspect"))
        {
            pickupText.SetActive(false);
            interactable = false;
            if (inspectCanvas != null && inspectCanvas.activeSelf)
            {
                inspectCanvas.SetActive(false);
            }
        }
    }
}
