using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class CardController : MonoBehaviour
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

    private bool isMousedOver = false;


    public CardDescriptor cardDetails;

    private bool pinned;

    private void Start()
    {
        glowSprite.enabled = false;
    }
    private void OnMouseOver()
    {
        transform.localScale = mouseOverScale;
        isMousedOver = true;
    }

    private void Update()
    {
        if(canAfford(cardDetails.cost))
        {
            SetGlowEnabled(true);
        } else
        {
            SetGlowEnabled(false);
        }
        if (pinned &&GridManager.Instance.isMouseOnGrid())
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
    }

    private void OnMouseExit()
    {
        isMousedOver = false;
        transform.localScale = new Vector3(1,1,1);
    }

    private void OnMouseDown()
    {
        if (!canAfford(cardDetails.cost) || GameManager.Instance.isPaused)
        {
            return;
        }
        pinned = true;
        HandZone.Instance.pinnedCard = this;
        // transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        // also do position
        Cursor.visible = false;
    }

    private void OnMouseUp()
    {
        if (GameManager.Instance.isPaused)
        {
            return;
        }
        pinned = false;
        HandZone.Instance.pinnedCard = null;
        transform.localScale = new Vector3(1, 1, 1);
        Cursor.visible = true;
        if (IsValidTarget() && !RoundManager.Instance.inGracePeriod)
        {
            PlayCard();
        } else
        {
        }
    }

    //Fill this in more later
    private bool IsValidTarget()
    {
        return GridManager.Instance.isMouseOnGrid();
        //return transform.parent.InverseTransformPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)).y > 2;
    }

    private void OnMouseDrag()
    {
        if(pinned)
        {
            //transform.parent = null;
            Vector3 mousePos = transform.parent.InverseTransformPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            gameObject.transform.localPosition = new Vector3(mousePos.x, mousePos.y, 0);
            
        }
    }

    public void SetGlowEnabled(bool enabled)
    {
        glowSprite.enabled = enabled;
    }

    public void SetPositionInOrder(Vector3 position, int layer)
    {
        SortingGroup sg = gameObject.GetComponent<SortingGroup>();
        if (!pinned)
        {

            Vector3 offset = new Vector3(0,
                (HandZone.Instance.pinnedCard != null ? -3 : isMousedOver ? 3 : 0), 0);

            transform.localPosition = Vector3.Lerp(transform.localPosition, position + offset, 0.1f);

            sg.sortingOrder = isMousedOver ? 11 : layer;
        } else
        {
            sg.sortingOrder = 11;
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

    private bool canAfford(int cost)
    {
        return cost <= Mathf.Ceil(RoundManager.Instance.GetTimeRemaining());
    }

    public void PlayCard()
    {
        // Check costs for legality
        if(!canAfford(cardDetails.cost))
        {
            return;
        }
        Vector2Int originCell = GridManager.Instance.GetMouseCell();
        if (originCell.x < 0)
        {
            return;
        }
            RoundManager.Instance.SpendTime(cardDetails.cost);
        // Evaluate effects
        foreach(CardEffect effect in cardDetails.effects)
        {
            if (effect.affectsAll)
            {
                for (int i = 0; i < 6; i++)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        CardEffectResolver.ResolveCardEffects(effect.effects, GridManager.Instance.GetInfoFromCoords(i, j), effect.values);
                    }
                }
                continue;
            }
            if (effect.melee)
            {
                originCell = new Vector2Int(0, originCell.y);
            }
            if (effect.affectsSelf)
            {
                // Needs a dummy value for the ring info for it to work :/ Something to fix if we keep working on this!
                CardEffectResolver.ResolveCardEffects(effect.effects, GridManager.Instance.GetInfoFromCoords(0, 0), effect.values);
                continue;
            }
            foreach (Vector2Int relativePosition in effect.relativePositionsOfEffect)
            {
                int ring = originCell.x + relativePosition.x;
                if(ring > 5 || ring < 0)
                {
                    continue;
                }
                int wedge = (originCell.y + relativePosition.y) % 6;
                if (wedge < 0)
                {
                    wedge += 6;
                }
                print("Retrieving Coords: (" + ring + "," + wedge + ")");
                CardEffectResolver.ResolveCardEffects(effect.effects, GridManager.Instance.GetInfoFromCoords(ring, wedge), effect.values);
                print("Person at place: " + GridManager.Instance.GetInfoFromCoords(ring, wedge).gridCharacter);
                print("Effecting (" + GridManager.Instance.GetInfoFromCoords(ring, wedge).ring + "," +GridManager.Instance.GetInfoFromCoords(ring, wedge).wedge + "), which contains " + (GridManager.Instance.GetInfoFromCoords(ring, wedge).gridCharacter == null ? "nothing" : "a person"));
            }
        }
        HandZone.Instance.RemoveCardFromHand(this);
        if (cardDetails.returns)
        {
            HandZone.Instance.AddCardToHand(this);
        }
        else
        {
            DeckManager.Instance.DiscardCard(this.cardDetails);
            Destroy(gameObject);
        }
        //Make the player animation happen
        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Animator>().SetTrigger("Attack");
    }

}
