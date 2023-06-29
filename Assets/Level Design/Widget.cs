using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewWidget", menuName = "Level Design/Widget")]
public class Widget : ScriptableObject
{
    public List<EnemyDescriptor> enemies;
    public int WaitTime;
}


[System.Serializable]
public class EnemyDescriptor
{
    public EnemyDescriptor(EnemyDescriptor desc)
    {
        prefab = desc.prefab;
        turnsToSpawn = desc.turnsToSpawn;
        wedge = desc.wedge;
        ring = desc.ring;
    }

    public GridCharacterController prefab;
    public int turnsToSpawn;
    public int wedge;
    public int ring = 5;
}