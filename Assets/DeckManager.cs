using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    private List<CardDescriptor> DrawPile;
    private List<CardDescriptor> DiscardPile;

    [SerializeField]
    private VoidEventSO OnTurnOverEvent;

    public static DeckManager Instance;

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    private void Awake()
    {
        if(DeckManager.Instance != null)
        {
            return;
        }
        DeckManager.Instance = this;
    }

    private void MoveDiscardToDrawPile()
    {
        print("Moving discard pile to draw pile. " + DiscardPile.Count + " cards.");
        DrawPile = new List<CardDescriptor>(DiscardPile);
        DiscardPile = new List<CardDescriptor>();
    }

    private void ShuffleDrawPile()
    {
        print("Shuffling draw pile");
        System.Random rng = new System.Random();
        for (int i = 0; i < DrawPile.Count; i++)
        {
            // NextDouble returns a random number between 0 and 1.
            // ... It is equivalent to Math.random() in Java.
            int r = i + (int)(rng.NextDouble() * (DrawPile.Count - i));
            CardDescriptor temp = DrawPile[r];
            DrawPile[r] = DrawPile[i];
            DrawPile[i] = temp;
        }
    }

    private void Start()
    {
        DrawPile = new List<CardDescriptor>(GameManager.Instance.deck);
        ShuffleDrawPile();
        DiscardPile = new List<CardDescriptor>();
        OnTurnOverEvent.RaiseEvent();
    }

    public void OnTurnOver()
    {

    }

    /**
     * Draws the next card off the draw pile.
     */
    public CardDescriptor DrawCard()
    {
        print("Drawing a card");
        if (DrawPile.Count == 0)
        {
            if(DiscardPile.Count == 0)
            {
                return null;
            }
            MoveDiscardToDrawPile();
            ShuffleDrawPile();
        }
        CardDescriptor topCard = DrawPile[0];
        DrawPile.RemoveAt(0);
        return topCard;
    }

    public void DiscardCard(CardDescriptor card)
    {
        print("Discarding a card");
        DiscardPile.Add(card);
    }
}
