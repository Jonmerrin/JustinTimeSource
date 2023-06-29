using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreCardController : MonoBehaviour
{


    [SerializeField]
    protected SpriteRenderer glowSprite;

    [SerializeField]
    Vector3 mouseOverScale = new Vector3(1.1f, 1.1f, 1.1f);

    [SerializeField]
    private TextMeshPro costText;
    [SerializeField]
    private TextMeshPro cardName;
    [SerializeField]
    private TextMeshPro descriptionText;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private SpriteRenderer sprite;


    public CardDescriptor cardDetails;

    private bool pinned;
    public bool wasBought = false;

    private void Start()
    {
        glowSprite.enabled = false;
    }
    private void OnMouseOver()
    {
        if (canAfford())
        {
            transform.localScale = mouseOverScale;
            SetGlowEnabled(true);
        }
    }

    private void Update()
    {
        if (canAfford())
        {
            SetGlowEnabled(true);
        }
        else
        {
            SetGlowEnabled(false);
        }
    }

    private void OnMouseExit()
    {
        SetGlowEnabled(false);
        transform.localScale = new Vector3(1, 1, 1);
    }

    private void OnMouseDown()
    {
        pinned = true;
    }

    private void OnMouseUp()
    {
        if(ShopManager.Instance.deleteCard)
        {
            foreach(CardDescriptor card in ShopManager.Instance.allCards.Cards)
            {
                if(card.Name == cardName.text)
                {
                    GameManager.Instance.deck.Remove(card);
                }
            }
            ShopManager.Instance.displayDeck = false;
            ShopManager.Instance.deleteCard = false;
            return;
        }
        pinned = false;
        if (!canAfford())
        {
            return;
        }
        BuyCard();
    }

    public void SetGlowEnabled(bool enabled)
    {
        glowSprite.enabled = enabled;
    }

    public void SetPositionInOrder(Vector3 position)
    {
        if (!pinned)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, position, 0.1f);
        }
    }

    public void InitializeCard(CardDescriptor card)
    {
        cardDetails = card;
        costText.text = cardDetails.cost.ToString();
        cardName.text = cardDetails.Name;
        descriptionText.text = cardDetails.Description;
        sprite.sprite = cardDetails.image;
        audioSource.clip = cardDetails.soundEffect;
    }

    private bool canAfford()
    {
        return !ShopManager.Instance.hasBoughtCard;
    }

    private void BuyCard()
    {
        ShopManager.Instance.hasBoughtCard = true;
        wasBought = true;
        GameManager.Instance.deck.Add(cardDetails);
    }

    public void GreyOut()
    {
        if(!wasBought)
        {
            SpriteRenderer[] spriteList = GetComponents<SpriteRenderer>();
            for (int i = 0; i < spriteList.Length; i++)
            {
                spriteList[i].color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            }
        }
    }
}
