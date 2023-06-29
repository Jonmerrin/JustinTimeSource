using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    public GameObject[] powerUpList;
    public List<StoreCardController> cardOptions;
    public CardList allCards;
    public StoreCardController CardPrefab;
    public SpriteRenderer curtain;
    public List<StoreCardController> displayedCards;

    public int numOptions;
    public bool hasBoughtCard = false;
    public bool hasBoughtPowerup = false;
    public bool displayDeck = false;
    public bool deleteCard = false;

    private void Awake()
    {
        if (Instance != null)
        {
            return;
        }
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Pick powerups
        int randomInd;
        List<int> tempIndList = new List<int>();
        for (int j = 0; j < 5; j++) tempIndList.Add(j);
        for (int k = 0; k < 2; k++)
        {
            randomInd = Random.Range(0, 5 - k);
            powerUpList[k].GetComponent<PowerupController>().powerType = tempIndList[randomInd];
            tempIndList.Remove(randomInd);
        }

        //Pick cards
        numOptions = GameManager.Instance.optionsInStore;
        List<CardDescriptor> tempList = new List<CardDescriptor>(allCards.Cards);
        int sumtotalOfCardsInExistance = allCards.Cards.Count;
        for (int i = 0; i < numOptions; i++)
        {
            randomInd = Random.Range(0, sumtotalOfCardsInExistance - i);
            cardOptions.Add(Instantiate(CardPrefab, new Vector3(0, 1.5f, 0), transform.rotation, transform));
            cardOptions[i].InitializeCard(tempList[randomInd]);
            tempList.RemoveAt(randomInd);
        }
        //Set up deck display (more than 18 doesn't work)
        List<CardDescriptor> deckList = GameManager.Instance.deck;
        int numCards = deckList.Count;
        float maxCardsPerRow = 6.0f;
        print("NumCards:" + numCards.ToString());
        int numRows = (int)Mathf.Ceil((float)numCards / maxCardsPerRow);
        print("NumRows:" + numRows.ToString());
        float xRange = 14;
        float yRange = 3;
        for (int i = 0; i < numCards; i++)
        {
            if(numCards > maxCardsPerRow)
            {
                displayedCards.Add(Instantiate(CardPrefab, new Vector3(-xRange / 2 + xRange / (maxCardsPerRow - 1) * (i % maxCardsPerRow), 1.5f + (yRange / 2) - (yRange / (numRows - 1) * Mathf.Floor((float)i / (float)maxCardsPerRow)), 0), transform.rotation, transform));
            }
            else
            {
                displayedCards.Add(Instantiate(CardPrefab, new Vector3(-xRange / 2 + xRange / (numCards - 1) * i, 1.5f, 0), transform.rotation, transform));
            }
            displayedCards[i].InitializeCard(deckList[i]);
            displayedCards[i].gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        //Lerp the cards
        float xRange = 10;
        for (int i = 0; i < numOptions; i++)
        {
            cardOptions[i].SetPositionInOrder(new Vector3(-xRange/2 + xRange/(numOptions-1)*i, 1.5f, 0));
        }
        //Grey out options after selecting
        if(hasBoughtCard)
        {
            for (int i = 0; i < numOptions; i++)
            {
                cardOptions[i].GreyOut();
            }
        }
        if (hasBoughtPowerup)
        {
            for (int i = 0; i < powerUpList.Length; i++)
            {
                powerUpList[i].GetComponent<PowerupController>().GreyOut();
            }
        }

        if(hasBoughtCard && hasBoughtPowerup && !deleteCard)
        {
            //TO DO: Pick the right scene to go to
            LevelLoader.Instance.LoadLevelWithIndex(3);
        }
        //Display decklist if needed
        foreach (StoreCardController card in cardOptions)
        {
            card.gameObject.SetActive(!displayDeck);
        }
        curtain.gameObject.SetActive(displayDeck);
        foreach (StoreCardController card in displayedCards)
        {
            card.gameObject.SetActive(displayDeck);
        }
    }

    public void RemoveCard()
    {
        displayDeck = true;
        deleteCard = true;
    }
}
