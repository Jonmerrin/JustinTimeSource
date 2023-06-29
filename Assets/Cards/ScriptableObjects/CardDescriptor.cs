using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NewCard", menuName = "Cards/Generic Card")]
public class CardDescriptor : ScriptableObject
{
    public string Name;
    public string Description;
    public int cost;
    public bool returns;

    public Sprite image;
    public AudioClip soundEffect;
    public List<CardEffect> effects;
}

[System.Serializable]
public struct CardEffect
{
    public bool affectsAll;
    public bool affectsSelf;
    public bool melee;
    public List<Vector2Int> relativePositionsOfEffect;
    public List<CardEffectList> effects;
    public List<int> values;
}


public class CardEffectResolver {

    /*********************************************
     * 
     *             Effect Functions
     * 
     *********************************************
     */

    public static void DoDamage(RingCellInfo info, int amount)
    {
        //Debug.Log(info.gridCharacter == null);
        if(info.gridCharacter != null)
        {
            //Debug.Log(info.gridCharacter);
            // If we have buffs or something, we can take a look at some Instance
            // player thing or the round manager or something. Yay Singletons!!!!
            info.gridCharacter.TakeDamage(amount);
        }
        GridManager.Instance.ShowDamageAt(info);
    }


    public static void MoveSpaces(RingCellInfo info, int ringOffset, int wedgeOffset)
    {
        if(info.gridCharacter != null)
        {
            info.gridCharacter.Move(ringOffset, wedgeOffset);
        }
    }

    public static void DoTimeDamage(RingCellInfo info)
    {
        int damage = (int)Mathf.Ceil(RoundManager.Instance.GetTimeRemaining());
        if (info.gridCharacter != null)
        {
            info.gridCharacter.TakeDamage(damage);
        }
    }

    public static void EndTurn()
    {
        //Debug.Log("endturn");
        RoundManager.Instance.timeSpent = GameManager.Instance.timePerRound + RoundManager.Instance.timeOffset;
    }

    public static void NextTurnBonusTime(int amount)
    {
        if(amount == 0)
        {
            amount = (int)Mathf.Ceil(RoundManager.Instance.GetTimeRemaining());
        }
        RoundManager.Instance.nextRoundTimeOffset += amount;
    }

    public static void Lifesteal(RingCellInfo info, int amount)
    {
        if (info.gridCharacter != null)
        {
            info.gridCharacter.TakeDamage(amount);
            GameManager.Instance.health += 1;
        }
    }

    /*********************************************
     * !!!!!!!!!!!!!!!Important!!!!!!!!!!!!!!!!!!
     *             Switch Case Evaluation
     * !!!!!!!!!!!!!DO NOT FORGET!!!!!!!!!!!!!!!!
     *********************************************
     */

    public static void ResolveCardEffect(CardEffectList effectName, RingCellInfo ringInfo, List<int> values)
    {
        switch (effectName)
        {
            case CardEffectList.DoDamage:
                DoDamage(ringInfo, values[0]);
                break;
            case CardEffectList.MoveSpaces:
                MoveSpaces(ringInfo, values[0], values[1]);
                break;
            case CardEffectList.DoTimeDamage:
                DoTimeDamage(ringInfo);
                break;
            case CardEffectList.EndTurn:
                EndTurn();
                break;
            case CardEffectList.NextTurnBonusTime:
                NextTurnBonusTime(values[0]);
                break;
            case CardEffectList.Lifesteal:
                Lifesteal(ringInfo, values[0]);
                break;

        }
    }

    public static void ResolveCardEffects(List<CardEffectList> effectNames, RingCellInfo ringInfo, List<int> values)
    {
        //Debug.Log("Assigning Resolutions to Event");
        foreach(CardEffectList effectName in effectNames)
        {
            ResolveCardEffect(effectName, ringInfo, values);
        }
    }
}



/*********************************************
 * 
 *             Function Enum
 * 
 *********************************************
 */
public enum CardEffectList
{
    DoDamage,
    MoveSpaces,
    DoTimeDamage,
    EndTurn,
    NextTurnBonusTime,
    Lifesteal
}