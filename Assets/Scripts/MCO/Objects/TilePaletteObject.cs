using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class TilePaletteObject
{
    [SerializeField] private TileBase topLeftTile;
    [SerializeField] private TileBase topMiddleTile;
    [SerializeField] private TileBase topRightTile;
    [SerializeField] private TileBase leftTile;
    [SerializeField] private TileBase middleTile;
    [SerializeField] private TileBase rightTile;
    [SerializeField] private TileBase bottomLeftTile;
    [SerializeField] private TileBase bottomMiddleTile;
    [SerializeField] private TileBase bottomRightTile;

    public TileBase TopLeft { get => topLeftTile; set => topLeftTile = value; }
    public TileBase TopMiddle { get => topMiddleTile; set => topMiddleTile = value; }
    public TileBase TopRight { get => topRightTile; set => topRightTile = value; }
    public TileBase Left { get => leftTile; set => leftTile = value; }
    public TileBase Middle { get => middleTile; set => middleTile = value; }
    public TileBase Right { get => rightTile; set => bottomLeftTile = value; }
    public TileBase BottomLeft { get => bottomLeftTile; set => topLeftTile = value; }
    public TileBase BottomMiddle { get => bottomMiddleTile; set => bottomMiddleTile = value; }
    public TileBase BottomRight { get => bottomRightTile; set => bottomRightTile = value; }

    public bool CheckTile(TileBase checkTile)
    {
        List<TileBase> groundTiles = new List<TileBase>
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
        };

        foreach (TileBase tile in groundTiles)
            if (checkTile == tile)
                return true;

        return false;
    }
}
