using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public SpriteRenderer sprite;
    public Vector3 mouseOverScale = new Vector3(1.1f, 1.1f, 1.1f);

    private void OnMouseOver()
    {
        transform.localScale = mouseOverScale;
    }

    private void OnMouseExit()
    {
        transform.localScale = new Vector3(1, 1, 1);
    }

    private void OnMouseDown()
    {
        ShopManager.Instance.hasBoughtCard = true;
        ShopManager.Instance.hasBoughtPowerup = true;
    }
}
