using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    public List<Widget> Level0Widgets;
    public List<Widget> Level1Widgets;
    public List<Widget> Level2Widgets;
    public List<Widget> Level3Widgets;
    public List<Widget> Level4Widgets;

    private List<Widget> QueuedWidgets;
    private List<EnemyDescriptor> QueuedEnemies;
    private List<GridCharacterController> characters;
    private int waitTime = 0;

    private void Awake()
    {
        if(EnemyManager.Instance != null)
        {
            return;
        }
        EnemyManager.Instance = this;
        characters = new List<GridCharacterController>();
        QueuedWidgets = new List<Widget>();
        QueuedEnemies = new List<EnemyDescriptor>();
    }


    private void Start()
    {
        //TODO: Check for level
        //Temp test
        //QueuedWidgets.Add(Level1Widgets[0]);
        //QueuedWidgets.Add(Level1Widgets[1]);
        //QueuedWidgets.Add(Level1Widgets[2]);

        int level = GameManager.Instance.level;
        int[] numWidgets = new int[5];
        numWidgets[0] = 1;
        numWidgets[1] = 0;
        numWidgets[2] = 0;
        numWidgets[3] = 0;
        numWidgets[4] = 0;

        List<Widget>[] widgetLists = new List<Widget>[5];
        widgetLists[0] = Level0Widgets;
        widgetLists[1] = Level1Widgets;
        widgetLists[2] = Level2Widgets;
        widgetLists[3] = Level3Widgets;
        widgetLists[4] = Level4Widgets;
        print("LEVEL: " + GameManager.Instance.level);
        for (int i = 0; i < level; i++)
        {
            for(int j = 0; j<i%5; j++)
            {
                numWidgets[j] += 1;
            }
            numWidgets[i % 5] += 1;
        }

        for(int i = 0; i < 5; i++)
        {
            for(int j = 0; j < numWidgets[i]; j++)
            {
                //print("WidgetCount = " + widgetLists[i].Count);
                QueuedWidgets.Add(widgetLists[i][Random.Range(0, widgetLists[i].Count)]);
            }
        }

        for(int i = 0; i < 5; i++)
        {
            print("Number of level " + i + " widgets(tm): " + widgetLists[i].Count);
        }

        shuffleList<Widget>(QueuedWidgets);

        AdvanceWidgets();
    }

    private void shuffleList<T>(List<T> list)
    {
        System.Random rng = new System.Random();
        for (int i = 0; i < list.Count; i++)
        {
            // NextDouble returns a random number between 0 and 1.
            // ... It is equivalent to Math.random() in Java.
            int r = i + (int)(rng.NextDouble() * (list.Count - i));
            T temp = list[r];
            list[r] = list[i];
            list[i] = temp;
        }
    }

    public void TakeEnemyTurns()
    {
        characters.Sort((char1, char2) => char1.controller.gridPos.x < char2.controller.gridPos.x ? -1 : 1);
        foreach(GridCharacterController character in characters)
        {
            //print(character.controller.gridPos);
            character.TakeTurn();
        }

        AdvanceWidgets();

        for (int i = 0; i < QueuedEnemies.Count; i++)
        {
            QueuedEnemies[i].turnsToSpawn -= 1;
            if (QueuedEnemies[i].turnsToSpawn <= 0)
            {
                
                GridCharacterController newEnemy = CreateEnemyFromDescriptor(QueuedEnemies[i]);
                QueuedEnemies.RemoveAt(i);
            }
        }
    }

    public GridCharacterController CreateEnemyFromDescriptor(EnemyDescriptor enemyDesc)
    {
        //Instantiate from prefab

        GridCharacterController newEnemy = Instantiate(enemyDesc.prefab);
        newEnemy.Initialize(enemyDesc);
        return newEnemy;
    }

    public int EnemiesRemaining()
    {
        int total = characters.Count + QueuedEnemies.Count;
        foreach(Widget desc in QueuedWidgets)
        {
            total += desc.enemies.Count;
        }
        return total;
    }

    public void AdvanceWidgets()
    {
        //print("Advancing Widgets");
        if((waitTime == 0 || (QueuedEnemies.Count == 0 && characters.Count == 0)) && QueuedWidgets.Count != 0)
        {
            //print("Adding enemies to Enemy Queue");
            int offset = Random.Range(0, 6);
            foreach(EnemyDescriptor info in QueuedWidgets[0].enemies)
            {
                EnemyDescriptor desc = new EnemyDescriptor(info);
                desc.wedge = (desc.wedge + offset) % 6;
                if(desc.turnsToSpawn == 0)
                {
                    CreateEnemyFromDescriptor(desc);
                } else
                {
                    QueuedEnemies.Add(desc);
                }
            }
            waitTime = QueuedWidgets[0].WaitTime;
            QueuedWidgets.RemoveAt(0);
            AdvanceWidgets();
        } else
        {
            waitTime -= 1;
        }
    }

    public void AddEnemy(GridCharacterController character)
    {
        characters.Add(character);
    }

    public void RemoveEnemy(GridCharacterController character)
    {
        print("Why is this called so many times");
        characters.Remove(character);
        if(EnemyManager.Instance.EnemiesRemaining() == 0) {
            GameManager.Instance.LevelComplete();
        }
    }
}
