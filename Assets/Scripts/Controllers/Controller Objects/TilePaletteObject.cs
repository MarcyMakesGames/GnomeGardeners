using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class TilePaletteObject
{
    [SerializeField] private Tile topLeftTile;
    [SerializeField] private Tile topMiddleTile;
    [SerializeField] private Tile topRightTile;
    [SerializeField] private Tile leftTile;
    [SerializeField] private Tile middleTile;
    [SerializeField] private Tile rightTile;
    [SerializeField] private Tile bottomLeftTile;
    [SerializeField] private Tile bottomMiddleTile;
    [SerializeField] private Tile bottomRightTile;

    public Tile TopLeft { get => topLeftTile; set => topLeftTile = value; }
    public Tile TopMiddle { get => topMiddleTile; set => topMiddleTile = value; }
    public Tile TopRight { get => topRightTile; set => topRightTile = value; }
    public Tile Left { get => leftTile; set => leftTile = value; }
    public Tile Middle { get => middleTile; set => middleTile = value; }
    public Tile Right { get => rightTile; set => bottomLeftTile = value; }
    public Tile BottomLeft { get => bottomLeftTile; set => topLeftTile = value; }
    public Tile BottomMiddle { get => bottomMiddleTile; set => bottomMiddleTile = value; }
    public Tile BottomRight { get => bottomRightTile; set => bottomRightTile = value; }
}
