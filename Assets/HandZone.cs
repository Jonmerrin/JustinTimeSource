using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandZone : MonoBehaviour
{

    public static HandZone Instance;

    [SerializeField]
    Vector2 dimensions = new Vector2(2, 2);
    [SerializeField]
    int maxHandSize = 10;

    [SerializeField]
    DeckManager deck;

    [SerializeField]
    CardController CardPrefab;

    [SerializeField]
    VoidEventSO OnTurnOverEvent;

    public List<CardController> hand;
    public CardController pinnedCard = null;

    private void OnEnable()
    {
        OnTurnOverEvent.Event += OnTurnOver;
    }

    private void OnDisable()
    {
        OnTurnOverEvent.Event -= OnTurnOver;
    }

    private void OnTurnOver()
    {
        DiscardHand();
        RefillHand();
    }

    private void Awake()
    {
        if(Instance != null)
        {
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        if(hand == null || hand.Count == 0)
        {
            hand = new List<CardController>();
        }
    }

    public void DrawCard()
    {
        if (hand.Count < maxHandSize)
        {
            CardDescriptor card = deck.DrawCard();
            CardController newCard = Instantiate(CardPrefab, new Vector3(transform.position.x - 2*dimensions.x, transform.position.y, transform.position.z), transform.rotation, transform);
            newCard.InitializeCard(card);
            //Initialize card based on description
            hand.Add(newCard);
        } else
        {
            print("BURNINATING ALL THE PEOPLE WHO LIVE IN THATCHED-ROOF COTTAGES");
            //Burn a card
            CardDescriptor card = deck.DrawCard();
            deck.DiscardCard(card);
            return;
        }
    }

    public void AddCardToHand(CardController card)
    {
        hand.Add(card);
    }

    public void DiscardHand()
    {
        foreach(CardController card in hand)
        {
            deck.DiscardCard(card.cardDetails);
            Destroy(card.gameObject);
        }
        hand = new List<CardController>();
    }

    public void RefillHand()
    {
        while(hand.Count < GameManager.Instance.startingHandSize)
        {
            print("I draw this many times --------->");
            DrawCard();
        }
    }

    private void OrganizeCards()
    {
        float offset = dimensions.x / hand.Count;
        for (int i = 0; i < hand.Count; i++)
        {
            hand[i].SetPositionInOrder(new Vector3(offset * i - (0.5f * offset * (hand.Count-1)), transform.position.y, 0), i);
        }
    }


    public void RemoveCardFromHand(CardController card)
    {
        hand.Remove(card);
        //deck.DiscardCard(card.cardDetails);
    }

    private void Update()
    {
        OrganizeCards();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(transform.position, new Vector3(dimensions.x, dimensions.y, 1));
    }

}
