using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCharacterController : MonoBehaviour
{
    [SerializeField]
    private int ring;
    [SerializeField]
    private int wedge;
    //[SerializeField]
    //private int turnsToSpawn = 0;

    public MikeController controller;

    public void TakeTurn()
    {
        /*if(turnsToSpawn > 0)
        {
            turnsToSpawn -= 1;
        } else if(turnsToSpawn == 0)
        {
            turnsToSpawn -= 1;
            
        }*/
        //Move forward or attack?
        controller.TakeTurn();
    }


    public bool canMoveTo(int dRing, int dWedge, GridCharacterController origin)
    {
        if (origin == this)
        {
            return true;
        }
        int newRing = ring + dRing;
        int newWedge = wedge + dWedge + (wedge + dWedge < 0 ? 6 : 0);
        if (newRing > 5 || newRing < 0)
        {
            return false;
        }
        if (GridManager.Instance.GetInfoFromCoords(newRing, newWedge).gridCharacter == null)
        {
            return true;
        }
        else
        {
            return GridManager.Instance.GetInfoFromCoords(newRing, newWedge).gridCharacter.canMoveTo(dRing, dWedge, origin);
        }
    }
    public bool canMoveTo(int dRing, int dWedge)
    {
        int newRing = ring + dRing;
        int newWedge = wedge + dWedge + (wedge + dWedge < 0 ? 6 : 0);
        if(newRing > 5 || newRing < 0)
        {
            return false;
        }
        if (GridManager.Instance.GetInfoFromCoords(newRing, newWedge).gridCharacter == null)
        {
            return true;
        } else
        {
            return GridManager.Instance.GetInfoFromCoords(newRing, newWedge).gridCharacter.canMoveTo(dRing, dWedge, this);
        }
    }

    public void Move(int dRing, int dWedge)
    {
        if(!canMoveTo(dRing, dWedge))
        {
            return;
        }
        GridManager.Instance.RemoveCharacterFromCell(ring, wedge, this);
        ring = Mathf.Clamp(dRing + ring, 0, 6);
        wedge = (dWedge + wedge) % 6;
        if (wedge < 0)
        {
            wedge += 6;
        }
        
        if (GridManager.Instance.GetInfoFromCoords(ring, wedge).gridCharacter != null)
        {
            GridManager.Instance.GetInfoFromCoords(ring, wedge).gridCharacter.Move(dRing, dWedge);
        }
        controller.gridPos = new Vector2(ring, wedge);
        GridManager.Instance.AddCharacterToCell(ring, wedge, this);
    }

    private void Start()
    {
        EnemyManager.Instance.AddEnemy(this);
    }

    public void TakeDamage(int amount)
    {
        controller.TakeDamage(amount);
    }

    private void Update()
    {
        MoveToSpace(GridManager.Instance.GridPosFromCell(ring, wedge));
    }

    public void MoveToSpace(Vector3 gridPos)
    {
        transform.position = Vector3.Lerp(transform.position, GridManager.Instance.transform.TransformPoint(gridPos), 0.1f);
    }

    public void Initialize(EnemyDescriptor info)
    {
        wedge = info.wedge;
        ring = info.ring;

        GridManager.Instance.AddCharacterToCell(ring, wedge, this);
        controller.gridPos = new Vector2(ring, wedge);
    }

}
