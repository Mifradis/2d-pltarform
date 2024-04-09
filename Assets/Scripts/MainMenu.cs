using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    AudioManager audioManager;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        Debug.Log("Starting");
    }
    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quiting");
    }
}
