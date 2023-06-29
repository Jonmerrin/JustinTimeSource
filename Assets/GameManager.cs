using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<CardDescriptor> deck;
    public GameObject UIDamageInd;

    // Overall information
    public bool isGameOver = false;
    public bool isPaused = false;
    public int level = 0;
    public bool hasCompletedTutorial = false;
    public int health = 50;
    public int startingHandSize = 4;
    public int timePerRound = 10; //Seconds, yknow like the theme
    public int optionsInStore = 3;
    public CardList startingDeck;

    // Information for restarts
    public static int startingHealth = 5;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);
    }


    // Start is called before the first frame update
    void Start()
    {
        deck = new List<CardDescriptor>(startingDeck.Cards);
    }

    // Update is called once per frame
    void Update()
    {
        // Check pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseOrUnpauseTime();
        }
    }

    void PauseOrUnpauseTime()
    {
        isPaused = !isPaused;
    }

    public void LevelComplete()
    {
        print("WHY");
        level++;
        LevelLoader.Instance.LoadLevelWithIndex(4); // shop
    }

    public void LevelLost()
    {
        LevelLoader.Instance.LoadLevelWithIndex(5);
    }

    public void Reset()
    {
        level = 0;
        health = 5;
        hasCompletedTutorial = true;
        startingHandSize = 4;
        timePerRound = 10;
        optionsInStore = 3;
        deck = new List<CardDescriptor>(startingDeck.Cards);
    }

    public void TakeDamage(int amount)
    {
        health = Mathf.Clamp(health - amount, 0, health);
        Instantiate(UIDamageInd);
        if(health <= 0)
        {
            GameManager.Instance.LevelLost();
        }
    }
}
