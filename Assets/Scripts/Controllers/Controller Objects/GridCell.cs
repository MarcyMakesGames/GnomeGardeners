using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell 
{
    private Vector3Int gridPosition;
    private Vector3 worldPosition;
    private GroundType groundType;
    private MapPosition mapPosition;
    private IInteractable cellOccupant;
    
    public Vector3Int GridPosition { get => gridPosition; }
    public Vector3 WorldPosition { get => worldPosition; }
    public GroundType GroundType { get => groundType; set => groundType = value; }
    public MapPosition MapPosition { get => mapPosition; }
    public IInteractable CellOccupant { get => cellOccupant; set => cellOccupant = value; }

    public GridCell(Vector3Int positionOnGrid, Vector3 positionInWorld, GroundType typeOfGround, MapPosition positionOnMap, IInteractable objectInCell)
    {
        gridPosition = positionOnGrid;
        worldPosition = positionInWorld;
        groundType = typeOfGround;
        mapPosition = positionOnMap;
        cellOccupant = objectInCell;
    }
}

public enum GroundType
{
    Arable,
    Grass,
    Dirt,
    Sand,

    Count
}

public enum MapPosition
{
    TopLeft,
    TopMiddle,
    TopRight,
    Left,
    Middle,
    Right,
    BottomLeft,
    BottomMiddle,
    BottomRight
}
