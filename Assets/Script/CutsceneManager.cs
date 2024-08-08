using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CutsceneManager : MonoBehaviour
{
    public TMP_Text cutsceneText;
    public Canvas cutsceneCanvas;
    public Canvas mainmenuCanvas;
    public string[] cutsceneLines;
    public float textDisplayTime = 3.0f;
    public string sceneName;

    private void Awake()
    {
        StartCoroutine(PlayCutscene());
    }

    private IEnumerator PlayCutscene()
    {
        cutsceneCanvas.gameObject.SetActive(true);

        foreach (string line in cutsceneLines)
        {
            cutsceneText.text = line;
            yield return new WaitForSeconds(textDisplayTime);
        }

        cutsceneCanvas.gameObject.SetActive(false);
        StartGame();
    }

    private void StartGame()
    {
        SceneManager.LoadScene(sceneName);
        // Masukkan logika untuk memulai game di sini.
        Debug.Log("Game Dimulai!");
    }
}
