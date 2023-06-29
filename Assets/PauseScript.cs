using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{

    public List<StoreCardController> cardOptions;
    public StoreCardController CardPrefab;

    // Start is called before the first frame update
    void Start()
    {
        //Pick cards
        int sumtotalOfCardsInExistance = GameManager.Instance.deck.Count;
        for (int i = 0; i < sumtotalOfCardsInExistance; i++)
        {
            cardOptions.Add(Instantiate(CardPrefab, new Vector3(0, 2, 0), transform.rotation, transform));
            cardOptions[i].InitializeCard(GameManager.Instance.deck[i]);
            Debug.Log("i " + i.ToString());
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Lerp the cards
        int deckSize = GameManager.Instance.deck.Count;
        float xRange = 10;
        for (int i = 0; i < deckSize; i++)
        {
            cardOptions[i].SetPositionInOrder(new Vector3(-xRange / 2 + xRange / (deckSize - 1) * i, 2, 0));
        }
    }



}
