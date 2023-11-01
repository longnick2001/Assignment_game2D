using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused;
    public Transform playerTramform;
    public GameObject btnMute;
    public GameObject btnMuted;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        if (AudioListener.pause)
        {
            btnMute.SetActive(false);  
        }
        else
        {
            btnMuted.SetActive(false);
        }
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
    }
    
    public void QuitGame()
    {
        if (!Player.isWin && isPaused && GameObject.FindGameObjectWithTag("Player"))
        {
            // Lấy scene hiện tại
            Scene currentScene = SceneManager.GetActiveScene();

            // Lấy tên của scene hiện tại
            string sceneName = currentScene.name;
            PlayerPrefs.SetFloat("indexPlayerX", playerTramform.position.x);
            PlayerPrefs.SetFloat("indexPlayerY", playerTramform.position.y);
            PlayerPrefs.SetString("scene", sceneName);
            PlayerPrefs.SetInt("scoreContinue", Player.score);
            PlayerPrefs.SetInt("numberOfHeart", Player.numberOfHearts);
            
            PlayerPrefs.Save();
        }
        Application.Quit();
    }
    
    public void BackToMain()
    {
        GameObject music = GameObject.FindGameObjectWithTag("music");
        Destroy(music);
        if (!Player.isWin && isPaused && GameObject.FindWithTag("Player"))
        {
            // Lấy scene hiện tại
            Scene currentScene = SceneManager.GetActiveScene();

            // Lấy tên của scene hiện tại
            string sceneName = currentScene.name;
            PlayerPrefs.SetFloat("indexPlayerX", playerTramform.position.x);
            PlayerPrefs.SetFloat("indexPlayerY", playerTramform.position.y);
            PlayerPrefs.SetString("scene", sceneName);
            PlayerPrefs.SetInt("scoreContinue", Player.score);
            PlayerPrefs.SetInt("numberOfHeart", Player.numberOfHearts);
            
            PlayerPrefs.Save();
        }
        SceneManager.LoadSceneAsync(0); 
    }
    
    public void Restart()
    {
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1f;
    }

    public void mute()
    {
        if (AudioListener.pause)
        {
            AudioListener.pause = false;
        }
        else
        {
            AudioListener.pause = true;
        }
    }
}