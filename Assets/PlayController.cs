using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayController : MonoBehaviour
{
    private void OnMouseOver()
    {
        transform.localScale = Vector3.one * 1.1f;
    }

    private void OnMouseExit()
    {
        transform.localScale = Vector3.one;
    }

    private void OnMouseUpAsButton()
    {
        LevelLoader.Instance.LoadLevelWithIndex(2);// TO DO CHANGE THIS BACK FOR TUTORIAL
    }
}
