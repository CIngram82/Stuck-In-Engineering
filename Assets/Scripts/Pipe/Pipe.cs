using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum PipeLevel
{
    Empty,
    One,
    LShape,
    IShape,
    TShape,
    XShape

}
public class Pipe : MonoBehaviour, IPointerClickHandler
{
    public int X { get; set; }
    public int Y { get; set; }

    // Set exits in inspector 1 for exit 0 for no exit.
    [Header("N,E,S,W; 1 open 0 close")]
    public int[] exits;
    [SerializeField] private float rotationSpeed = 0.1f;
    [SerializeField] private float rotationDirection = 0;
    [SerializeField] private bool canTurn = true;
    [SerializeField] PipeLevel pipeLevel;
    public Sprite pipeSprite;
    private bool isTurning = false;
    public bool isPowered = false;
    public bool providesPower = false;
    [HideInInspector]
    public Pipe north, south, east, west;
    public bool isDamaged = false;
    
    #region pathfinding
    [HideInInspector] public int gCost, hCost, fCost;
    [HideInInspector] public Pipe previousNode;

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }
    #endregion

    private void Start()
    {
       //GetComponent<Image>().sprite = pipeSprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.rotation.eulerAngles.z != rotationDirection)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.x, transform.rotation.y, rotationDirection), rotationSpeed);
            isTurning = false;
        }
    }
    public void OnMouseDown()
    {
        RotatePipe();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        RotatePipe();
    }
    public void RotatePipe()
    {
        if (!canTurn || isTurning) return;
        // set the new direction the image will turn to.
        rotationDirection += 90;
        if (rotationDirection == 360)
        {
            rotationDirection = 0;
        }
        // update the exit values to match the image. 
        var temp = exits[0];
        for (int i = 0; i < exits.Length - 1; i++)
        {
            exits[i] = exits[i + 1];
        }
        exits[exits.Length - 1] = temp;
        isTurning = true;

        GameManager.Instance.CheckConnections();
    }
    public static void MakeEastWestNeighbors(Pipe east, Pipe west)
    {
        west.east = east;
        east.west = west;
    }
    public static void MakeNorthSouthNeighbors(Pipe north, Pipe south)
    {
        south.north = north;
        north.south = south;
    }

}
