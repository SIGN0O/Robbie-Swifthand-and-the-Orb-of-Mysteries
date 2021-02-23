using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject volumeMenu;
    public AudioMixer audioMixer;
    public GameObject UIMenu;

    private bool IsInMenu;

    private string masterVol, ambientVol, musicVol;

    private void Start()
    {
        masterVol = "MasterVolume";
        ambientVol = "SoundVolume";
        musicVol = "MusicVolume";

    }
    private void PauseGame()
    {
        Time.timeScale = 0f;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
        IsInMenu = false;
    }
    public void PlayGame()
    {
        GameManager.NextLevel();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PauseGameMenu()
    {
        if (IsInMenu )
        {
            return;
        }
        pauseMenu.SetActive(true);
        IsInMenu = true;
        PauseGame();
    }

    public void ResumeGameButtun()
    {
        pauseMenu.SetActive(false);
        ResumeGame();
    }

    public void VolumeMenu()
    {
        if(pauseMenu!=null)
            pauseMenu.SetActive(false);
        if (UIMenu != null)
            UIMenu.SetActive(false);
        volumeMenu.SetActive(true);
    }
    public void BackButton()
    {
        volumeMenu.SetActive(false);
        if(pauseMenu !=null)
           pauseMenu.SetActive(true);
        if (UIMenu != null)
            UIMenu.SetActive(true);
    }

    public void RestartGame()
    {
        GameManager. RestartLevel();

        ResumeGame();
    }

    public void SetMasterVolume(float value)
    {
        audioMixer.SetFloat(masterVol, value);
    }
    
    public void SetAmbientVolume(float value)
    {
        audioMixer.SetFloat(ambientVol, value);
    }

    public void SetMusicVolume(float value)
    {
        audioMixer.SetFloat(musicVol, value);
    }
    public void InMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGameMenu();
        }
    }
 
    void Update()
    {
        InMenu();
    }
}
