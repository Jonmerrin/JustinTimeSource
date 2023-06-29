using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;
    public static bool gamePaused = false;
    public GameObject pauseUI;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(gamePaused)
        {
            pauseUI.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            pauseUI.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void Resume()
    {
        //Debug.Log("Resumed");
        gamePaused = false;
    }

    public static void Restart()
    {
        //Debug.Log("Restarted");
        gamePaused = false;
        LevelLoader.Instance.LoadLevelWithIndex(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        //Debug.Log("Quit");
        gamePaused = false;
        LevelLoader.Instance.LoadLevelWithIndex(1);
    }
}
