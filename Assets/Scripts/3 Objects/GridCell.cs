using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell 
{
    private Vector2Int gridPosition;
    private Vector3 worldPosition;
    private GroundType groundType;
    private TilePosition mapPosition;
    private IOccupant occupant;
    
    public Vector2Int GridPosition { get => gridPosition; }
    public Vector3 WorldPosition { get => worldPosition; }
    public GroundType GroundType { get => groundType; set => groundType = value; }
    public TilePosition MapPosition { get => mapPosition; }
    public IOccupant Occupant { get => occupant; set => occupant = value; }

    public GridCell(Vector2Int positionOnGrid, Vector3 positionInWorld, GroundType typeOfGround, TilePosition positionOnMap, IOccupant objectInCell)
    {
        gridPosition = positionOnGrid;
        worldPosition = positionInWorld;
        groundType = typeOfGround;
        mapPosition = positionOnMap;
        occupant = objectInCell;
    }

    public void RemoveCellOccupant()
    {
        occupant = null;
    }

    public void AddCellOccupant(IOccupant occupant)
    {
        this.occupant = occupant;
    }
}