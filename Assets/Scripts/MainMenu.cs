using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject btnMuted;
    public GameObject btnMute;
    public GameObject music;
    public InputField input;

    public string playerName;
    public float indexPlayerX;
    public float indexPlayerY;
    public string sceneName;
    public GameObject btnContinue;
    public static bool isContinue;
    void Start()
    {
        input.text = PlayerPrefs.GetString("playerName", "");
        sceneName = PlayerPrefs.GetString("scene", null);
        DontDestroyOnLoad(music);
    }
    
    void Update()
    {
        playerName = input.text;
        PlayerPrefs.SetString("playerName", playerName);
        if (sceneName != "" && PauseMenu.isPaused)
        {
            btnContinue.SetActive(true);
        }
        if (AudioListener.pause)
        {
            btnMute.SetActive(false);
            btnMuted.SetActive(true);
        }
        else
        {
            btnMute.SetActive(true);
            btnMuted.SetActive(false);
        }
    }
    
    public void PlayGame()
    {
        PlayerPrefs.SetInt("score", 0);
        SceneManager.LoadSceneAsync(1); 
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Continue()
    {
        if (sceneName != "")
        {
            isContinue = true;
            SceneManager.LoadScene(sceneName);
            Time.timeScale = 1f;
        }
    }
    
    public void GameOne()
    {
        SceneManager.LoadSceneAsync(1); 
        Time.timeScale = 1f;
    }
    public void GameTwo()
    {
        SceneManager.LoadSceneAsync(2); 
        Time.timeScale = 1f;
    }
    public void GameThree()
    {
        SceneManager.LoadSceneAsync(3); 
        Time.timeScale = 1f;
    }

    public void Mute()
    {
        AudioListener.pause = true;
    }
    public void Muted()
    {
        AudioListener.pause = false;
    }
}
