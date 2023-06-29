using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    public Collider gridCollider;
    public GameObject[] ringHighlights;
    public GameObject secondHand;
    public GameObject damageBurst;
    private RingCellInfo[,] RingCells = new RingCellInfo[6,6]; 
    
    float gridRadius = 16.75f/2.0f;
    float gridInnerRad;

    private void Awake()
    {
        if(GridManager.Instance != null)
        {
            return;
        }
        GridManager.Instance = this;
        for (int ring = 0; ring < 6; ring++)
        {
            for (int wedge = 0; wedge < 6; wedge++)
            {
                RingCells[ring, wedge] = new RingCellInfo();
                RingCells[ring, wedge].ring = ring;
                RingCells[ring, wedge].wedge = wedge;
                RingCells[ring, wedge].isHighlighted = false;
                RingCells[ring, wedge].gridCharacter = null;
                RingCells[ring, wedge].gridObject = null;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gridInnerRad = gridRadius / 10;
        
    }

    // Update is called once per frame
    void Update()
    {
        ClearHighlights();
        Vector3 castFrom = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 castDir = Camera.main.transform.forward;
        RaycastHit hit;
        Physics.Raycast(castFrom, castDir, out hit, 100, LayerMask.GetMask("World"));
        if (hit.collider == gridCollider)
        {
            if(HandZone.Instance.pinnedCard == null)
            {
                HighlightCell(GetGridCell(hit.point));
            } else
            {
                Vector2Int originPoint = GetGridCell(hit.point);
                CardDescriptor cardDetails = HandZone.Instance.pinnedCard.cardDetails;
                foreach(CardEffect effect in cardDetails.effects)
                {
                    if(effect.affectsAll)
                    {
                        for(int i = 0; i < 6; i++)
                        {
                            for(int j = 0; j < 6; j++)
                            {
                                HighlightCell(new Vector2Int(i,j));
                            }
                        }
                        continue;
                    }
                    if (effect.melee)
                    {
                        originPoint = new Vector2Int(0, originPoint.y);
                    }
                    List<Vector2Int> relativePositions = effect.relativePositionsOfEffect;
                    foreach(Vector2Int relativePosition in relativePositions)
                    {
                        HighlightCell(originPoint + relativePosition);
                    }
                }
            }
        }
        //Update second hand position
        float angle = -360 * RoundManager.Instance.timer.fillAmount;
        secondHand.transform.rotation = Quaternion.Euler(new Vector3(0,0,angle));
    }

    public void AddCharacterToCell(int ring, int wedge, GridCharacterController character)
    {
        if(RingCells[ring, wedge].gridCharacter == null)
        {
            print("Does this even happen");
            RingCells[ring, wedge].gridCharacter = character;
        }
    }


    public void RemoveCharacterFromCell(int ring, int wedge, GridCharacterController character)
    {
        RingCells[ring, wedge].gridCharacter = null;
    }

    public bool isMouseOnGrid()
    {
        Vector3 castFrom = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 castDir = Camera.main.transform.forward;
        RaycastHit hit;
        Physics.Raycast(castFrom, castDir, out hit, 100, LayerMask.GetMask("World"));
        Vector2 gridCell = GetGridCell(hit.point);
        return gridCell.x >= 0 && gridCell.y >= 0;
    }

    public Vector2Int GetGridCell(Vector3 point)
    {
        Vector2Int gridCell = new Vector2Int();
        Vector3 pointOnGrid = point - transform.position;
        pointOnGrid.z = 0;
        float angle = Mathf.Atan2(pointOnGrid.y, pointOnGrid.x)*180/Mathf.PI - 90;
        if( angle < 0)
        {
            angle += 360;
        }
        float distance = Vector3.Magnitude(pointOnGrid);
        if(distance > gridRadius || distance < gridInnerRad)
        {
            gridCell.x = -1;
        }
        gridCell.x = Mathf.FloorToInt((distance - gridInnerRad) / (gridRadius-gridInnerRad)*6);
        gridCell.y = 5 - Mathf.FloorToInt(angle / 60);
        return gridCell;
    }

    public Vector2Int GetMouseCell()
    {
        Vector3 castFrom = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 castDir = Camera.main.transform.forward;
        RaycastHit hitInfo;
        bool hit = Physics.Raycast(castFrom, castDir, out hitInfo, 100, LayerMask.GetMask("World"));
        if(hit)
        {
            return GetGridCell(hitInfo.point);
        }
        return new Vector2Int(-1, 0);
    }

    public Vector3 GridPosFromCell(int ring, int wedge)
    {
        float theta = -60 * wedge + 15;
        float singleCellSize = (gridRadius - gridInnerRad) / 6;
        float r = (singleCellSize * ring + singleCellSize * 0.5f + gridInnerRad) / transform.localScale.x;
        print("Single cell size: " + singleCellSize);
        print("R: " + r);
        print("theta: " + theta);
        return new Vector3(r * Mathf.Cos(theta * Mathf.PI/180), r * Mathf.Sin(theta * Mathf.PI / 180), 0);
    }

    public RingCellInfo GetInfoFromCoords(int ring, int wedge)
    {
        return RingCells[ring, wedge];
    }

    public void HighlightCell(Vector2Int cell)
    {
        if (cell.x < 0 || cell.x > 5)
        {
            return;
        }
        int x = (cell.x);
        int y = (cell.y % 6);
        if(y < 0)
        {
            y += 6;
        }
        int ind = (int)(x * 6 + y);
        ringHighlights[ind].SetActive(true);
    }

    private void ClearHighlights()
    {
        foreach (GameObject cell in ringHighlights)
        {
            cell.SetActive(false);
        }
    }

    public void ShowDamageAt(RingCellInfo info)
    {
        GameObject burst = Instantiate(damageBurst, transform.position, Quaternion.identity);
        burst.transform.localScale = Vector3.one;
        burst.transform.parent = gameObject.transform;
        burst.transform.localPosition = GridPosFromCell(info.ring, info.wedge);
    }
}


public struct RingCellInfo
{
    public int ring;
    public int wedge;
    public bool isHighlighted;

    public GridCharacterController gridCharacter;
    public GridObject gridObject;
}