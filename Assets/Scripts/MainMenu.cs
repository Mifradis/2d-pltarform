using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    AudioManager audioManager;
    [Header("Main Menu")]
    [SerializeField] GameObject options;
    [SerializeField] GameObject mainMenu;
    [Header("Options")]
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] GameObject Back;
    [Header("Audio")]
    [SerializeField] AudioSource music;
    [SerializeField] AudioSource sfx;
    
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        musicSlider.value = music.volume;
        sfxSlider.value = sfx.volume;
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
    public void OpenOptions()
    {
        mainMenu.SetActive(false);
        options.SetActive(true);
        Back.SetActive(true);
    }
    public void ChangeMusicVolume()
    {
        music.volume = musicSlider.value;
    }
    public void ChangeSFXVolume()
    {
        sfx.volume = sfxSlider.value;
    }
    public void Continue()
    {
        if (SceneManager.GetActiveScene().name.Equals("Game"))
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
            Time.timeScale = 1;
            gameObject.SetActive(false);
        }
    }
    public void BackToMenu()
    {
        mainMenu.SetActive(true);
        options.SetActive(false);
        Back.SetActive(false);
    }
}
