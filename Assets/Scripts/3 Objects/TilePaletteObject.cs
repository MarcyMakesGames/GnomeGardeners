using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class TilePaletteObject
{
    [SerializeField] private Sprite spriteMask;
    [SerializeField] private TileBase topLeft;
    [SerializeField] private TileBase topMiddle;
    [SerializeField] private TileBase topRight;
    [SerializeField] private TileBase left;
    [SerializeField] private TileBase middle;
    [SerializeField] private TileBase right;
    [SerializeField] private TileBase bottomLeft;
    [SerializeField] private TileBase bottomMiddle;
    [SerializeField] private TileBase bottomRight;
    [SerializeField] private TileBase columnTop;
    [SerializeField] private TileBase columnMiddle;
    [SerializeField] private TileBase columnBottom;
    [SerializeField] private TileBase rowLeft;
    [SerializeField] private TileBase rowMiddle;
    [SerializeField] private TileBase rowRight;
    [SerializeField] private TileBase single;

    [SerializeField] private List<TileBase> extraTiles;
    private List<TileBase> checkTiles;
    private bool compiledTileList = false;

    private Dictionary<TileBase, TilePosition> TilePositionDict = new Dictionary<TileBase, TilePosition>();

    public Sprite SpriteMask { get => SpriteMask; }
    public TileBase TopLeft { get => topLeft; }
    public TileBase TopMiddle { get => topMiddle; }
    public TileBase TopRight { get => topRight; }
    public TileBase Left { get => left; }
    public TileBase Middle { get => middle; }
    public TileBase Right { get => right; }
    public TileBase BottomLeft { get => bottomLeft; }
    public TileBase BottomMiddle { get => bottomMiddle; }
    public TileBase BottomRight { get => bottomRight; }
    public TileBase ColumnTop { get => columnTop; }
    public TileBase ColumnMiddle { get => columnMiddle; }
    public TileBase ColumnBottom { get => columnBottom; }
    public TileBase RowLeft { get => rowLeft; }
    public TileBase RowMiddle { get => rowMiddle; }
    public TileBase RowRight { get => rowRight; }
    public TileBase Single { get => single; }


    public bool CheckContainsTile(TileBase checkTile)
    {
        if (!compiledTileList)
            InitTilePaletteObject();

        foreach (TileBase tile in checkTiles)
            if (checkTile == tile)
                return true;

        return false;
    }

    public TilePosition GetMapPosition(TileBase checkTile)
    {
        foreach (KeyValuePair<TileBase, TilePosition> pair in TilePositionDict)
            if (checkTile == pair.Key)
                return pair.Value;
        return TilePosition.NotSwappable;
    }

    private void InitTilePaletteObject()
    {
        checkTiles = new List<TileBase>
        {
            TopLeft,
            TopMiddle,
            TopRight,
            Left,
            Middle,
            Right,
            BottomLeft,
            BottomMiddle,
            BottomRight,
            ColumnBottom,
            ColumnMiddle,
            ColumnTop,
            RowMiddle,
            RowRight,
            RowLeft,
            Single
        };

        foreach (TileBase tile in extraTiles)
            checkTiles.Add(tile);

        compiledTileList = true;

        TilePositionDict.Add(TopLeft, TilePosition.TopLeft);
        TilePositionDict.Add(TopMiddle, TilePosition.TopMiddle);
        TilePositionDict.Add(TopRight, TilePosition.TopRight);
        TilePositionDict.Add(Left, TilePosition.Left);
        TilePositionDict.Add(Middle, TilePosition.Middle);
        TilePositionDict.Add(Right, TilePosition.Right);
        TilePositionDict.Add(BottomLeft, TilePosition.BottomLeft);
        TilePositionDict.Add(BottomMiddle, TilePosition.BottomMiddle);
        TilePositionDict.Add(BottomRight, TilePosition.BottomRight);
        TilePositionDict.Add(ColumnTop, TilePosition.ColumnTop);
        TilePositionDict.Add(ColumnMiddle, TilePosition.ColumnMiddle);
        TilePositionDict.Add(ColumnBottom, TilePosition.ColumnBottom);
        TilePositionDict.Add(RowLeft, TilePosition.RowLeft);
        TilePositionDict.Add(RowMiddle, TilePosition.RowMiddle);
        TilePositionDict.Add(RowRight, TilePosition.RowRight);
        TilePositionDict.Add(Single, TilePosition.Single);
    }
}
