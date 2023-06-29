using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupController : MonoBehaviour
{
    public SpriteRenderer sprite;
    public GameObject helperText;
    public string[] powerList;
    public string[] helperTextList;
    public Sprite[] spriteList;
    public int powerType;
    public Vector3 mouseOverScale = new Vector3(1.1f, 1.1f, 1.1f);
    public bool wasBought = false;

    // Start is called before the first frame update
    void Start()
    {
        sprite.sprite = spriteList[powerType];
        helperText.GetComponent<TMPro.TextMeshPro>().text = helperTextList[powerType];
        helperText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        sprite.sprite = spriteList[powerType];
        helperText.GetComponent<TMPro.TextMeshPro>().text = helperTextList[powerType];
    }

    private void OnMouseOver()
    {
        if(!ShopManager.Instance.hasBoughtPowerup)
        {
            transform.localScale = mouseOverScale;
        }
        helperText.SetActive(true);
    }

    private void OnMouseExit()
    {
        transform.localScale = new Vector3(1, 1, 1);
        helperText.SetActive(false);
    }

    private void OnMouseDown()
    {
        if(!ShopManager.Instance.hasBoughtPowerup)
        {
            wasBought = true;
            switch(powerList[powerType])
            {
                case "Remove Card":
                    ShopManager.Instance.RemoveCard();
                    break;
                case "Bigger Hand":
                    GameManager.Instance.startingHandSize += 1;
                    break;
                case "More Time":
                    GameManager.Instance.timePerRound += 1;
                    break;
                case "Heal":
                    GameManager.Instance.health += 1;
                    break;
                case "More Options":
                    GameManager.Instance.optionsInStore += 1;
                    break;
            }
            ShopManager.Instance.hasBoughtPowerup = true;
        }
    }
    public void GreyOut()
    {
        if (!wasBought)
        {
            sprite.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        }
    }
}
