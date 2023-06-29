using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCardList", menuName = "Cards/Card List")]
public class CardList : ScriptableObject
{
    public List<CardDescriptor> Cards;
}
