using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


//TODO: Remove magic numbers
public class RoundManager : MonoBehaviour
{
    //Static
    public static RoundManager Instance;

    // Public
    public Image timer;
    public Image usedManaIndicator;
    public TextMeshProUGUI graceTimer;
    public GameObject pauseCanvas;
    public GameObject graceTimerCanvas;

    public TextMeshProUGUI hudText;

    // Information for timer
    public float timeElapsed = 0f;
    public float timeSpent = 0f;
    public float nextRoundTimeOffset = 0;
    public float graceTimeLeft = 4.0f;
    public bool inGracePeriod = false;

    // Information for enemies
    //private int difficultyLevel = 0;

    [SerializeField]
    private VoidEventSO EndRoundEvent;
    [SerializeField]
    private VoidEventSO StartRoundEvent;
    public float timeOffset = 0;
    public GameObject playerHealthBar;
    public GameObject enemyCounter;
    public GameObject turnClock;

    // Start is called before the first frame update
    void Start()
    {
        // Create initial enemies? Here or grid manager?
        if(RoundManager.Instance != null)
        {
            return;
        }
        RoundManager.Instance = this;

        StartGracePeriod();
    }



    // Update is called once per frame
    void Update()
    {
        float roundTime = GameManager.Instance.timePerRound + timeOffset;

        if (GameManager.Instance.isPaused)
        {
            pauseCanvas.SetActive(true);
            return;
        } else
        {
            pauseCanvas.SetActive(false);
        }
        if (!inGracePeriod)
        {
            timeElapsed += 1f * Time.deltaTime;
            timer.fillAmount = timeElapsed / roundTime;
            if(timeSpent < timeElapsed)
            {
                timeSpent = timeElapsed;
            }
            usedManaIndicator.fillAmount = timeSpent / roundTime;

            if (timeElapsed >= roundTime)
            {
                timeOffset = nextRoundTimeOffset;
                nextRoundTimeOffset = 0;
                Cursor.visible = true;
                EndRoundEvent.RaiseEvent();
                EnemyManager.Instance.TakeEnemyTurns();
                StartGracePeriod();
            } 

        }
        else // We are in grace period
        {
            graceTimeLeft -= 1f * Time.deltaTime;
            graceTimer.text = Mathf.CeilToInt(graceTimeLeft).ToString();

            if (graceTimeLeft <= 0)
            {
                EndGracePeriod();
            }
        }
        //Update the player health indicator
        playerHealthBar.GetComponent<TMPro.TextMeshPro>().text = GameManager.Instance.health.ToString();
        //Update the enemy counter
        print("Enemies remaining: " + EnemyManager.Instance.EnemiesRemaining());
        enemyCounter.GetComponent<TMPro.TextMeshPro>().text = EnemyManager.Instance.EnemiesRemaining().ToString();
        //Update the turn clock
        turnClock.GetComponent<TMPro.TextMeshPro>().text = Mathf.Ceil(GetTimeRemaining()).ToString();
    }

    public Vector2Int GetGridByPosition(Vector3 mousePosition)
    {
        RaycastHit hit;
        bool hasHit = Physics.Raycast(mousePosition, Camera.main.transform.forward, out hit);
        //Vector3 distanceFromCenter = hit.collider.transform.TransformPoint;
        // transform point in world space into local space
        // hit.collider.transform.TransformPoint

        Vector2Int return_value = new Vector2Int(1, 2);
        return return_value;
    }

    internal void SpendTime(int cost)
    {
        timeSpent = Mathf.Floor(RoundManager.Instance.timeSpent) + cost;
    }

    private string GetHUDText()
    {
        return 
            "Time Left: " + (GameManager.Instance.timePerRound + timeOffset - timeElapsed).ToString().Substring(0,3) +
            "\nHealth: " + GameManager.Instance.health.ToString() +
            "\nEnemies Left: " + EnemyManager.Instance.EnemiesRemaining().ToString();
    }

    private void StartGracePeriod()
    {
        inGracePeriod = true;
        graceTimerCanvas.SetActive(true);
        graceTimeLeft = 3.0f;
        timeElapsed = 0f;
        timeSpent = 0f;
    }

    private void EndGracePeriod()
    {
        inGracePeriod = false;
        graceTimerCanvas.SetActive(false);
    }

    public float GetTimeRemaining()
    {
        return GameManager.Instance.timePerRound + timeOffset - timeSpent;
    }

    void DrawDeck()
    {
		/*
        HorizontalLayoutGroup gridLayoutGroup = deckGridPanel.GetComponent<HorizontalLayoutGroup>();
        CardController newCard = Instantiate(CardPrefab, new Vector3(0, 20, 0), Quaternion.Euler(new Vector3(-45, 0, 0)), deckGridPanel.transform);
        newCard.InitializeCard(GameManager.Instance.deck[0]); // todo This should be not just deck[0] but all of them.
        newCard.transform.SetParent(gridLayoutGroup.transform, true);*/

        /*
        LayoutRebuilder.MarkLayoutForRebuild((RectTransform) deckGridPanel.transform);
        LayoutRebuilder.MarkLayoutForRebuild((RectTransform) gridLayoutGroup.transform);
        CardController newCard = Instantiate(CardPrefab, new Vector3(0, 12, 0), Quaternion.Euler(new Vector3(-45, 0, 0)), deckGridPanel.transform);
        newCard.InitializeCard(GameManager.Instance.deck[0]);
        newCard.transform.SetParent(gridLayoutGroup.transform, true);
        gridLayoutGroup.CalculateLayoutInputHorizontal();
        gridLayoutGroup.CalculateLayoutInputVertical();
        gridLayoutGroup.SetLayoutHorizontal();
        gridLayoutGroup.SetLayoutVertical();
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform) deckGridPanel.transform);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform) gridLayoutGroup.transform);
        //newCard.transform.localScale = new Vector3(200, 200, 200);*/
    }

    


}
