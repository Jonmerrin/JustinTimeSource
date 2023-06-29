using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCardGameButton : MonoBehaviour
{

    [SerializeField]
    VoidEventSO OnEndTurnEvent;
    // Start is called before the first frame update
    void OnEnable()
    {
        OnEndTurnEvent.Event += OnEndTurn;
    }

    private void OnDisable()
    {
        OnEndTurnEvent.Event -= OnEndTurn;
    }

    void OnEndTurn()
    {
        print("Turn is over!");
    }

    public void EndTurnButton()
    {
        OnEndTurnEvent.RaiseEvent();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
