using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance;
    public Animator transition;

    // Scenes
    public static String splashScreen = "SplashScreen";
    public static int splashScreenIndex = 0;
    public static String menu = "Menu";
    public static int menuIndex = 1;
    public static String tutorial = "Tutorial";
    public static int tutorialIndex = 2;
    public static String level0 = "Level0";
    public static int level0Index = 3;
    public static String store = "Store";
    public static int storeIndex = 4;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    void Start()
    {
        StartTransitions();
    }

    public void StartTransitions()
    {
        //Debug.Log("Start transitions");
        switch (SceneManager.GetActiveScene().name)
        {
            case "SplashScreen":
                StartCoroutine(WaitForTransition(2.0f, menuIndex));
                break;
            case "GameOver":
                GameManager.Instance.Reset();
                StartCoroutine(WaitForTransition(10.0f, menuIndex));
                break;
        }
    }

    public void LoadNextLevel()
    {
        int nextLevel = SceneManager.GetActiveScene().buildIndex + 1;
        StartCoroutine(WaitForTransition(0.0f, nextLevel));
    }

    public void LoadLevelWithIndex(int nextLevel)
    {
        StartCoroutine(WaitForTransition(0.0f, nextLevel));
    }

    IEnumerator WaitForTransition(float waitTime, int nextLevel)
    {
        // Wait time until fadeout
        yield return new WaitForSeconds(waitTime);
        // Fade out starts, yield time to allow the fadeout to happen
        transition.SetTrigger("StartCrossfade");
        yield return new WaitForSeconds(1.2f);
        PlayAudioForLevel(nextLevel);
        SceneManager.LoadScene(nextLevel);
    }

    void PlayAudioForLevel(int nextLevel)
    {
        Debug.Log("play audio for level");
        if (nextLevel == 4)
        {
            AudioManager.Instance.PlayShop();
        }
        else if (nextLevel == 3)
        {
            AudioManager.Instance.PlayBattle();
        }
        else if (nextLevel == 1)
        {
            AudioManager.Instance.PlayMenu();
        }
    }

}
