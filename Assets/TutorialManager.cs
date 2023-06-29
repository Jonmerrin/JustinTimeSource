using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    
    private void Update()
    {
        if (Input.anyKey)
        {
            LevelLoader.Instance.LoadNextLevel();
        }

    }
}
