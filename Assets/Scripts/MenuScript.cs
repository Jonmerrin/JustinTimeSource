using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    public static MenuScript instance;

    public GameObject mainCanvas;
    public GameObject creditsPanel;

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

    public void Play()
    {
        LevelLoader.Instance.LoadNextLevel();
    }
    
    public void Tutorial()
    {
        LevelLoader.Instance.LoadLevelWithIndex(2);
    }

    public void ToggleCredits()
    {
        bool creditsOn = creditsPanel.activeInHierarchy;
        if (creditsOn)
        {
            creditsPanel.SetActive(false);
            mainCanvas.SetActive(true);
        } else
        {
            creditsPanel.SetActive(true);
            mainCanvas.SetActive(false);
        }
    }
}
