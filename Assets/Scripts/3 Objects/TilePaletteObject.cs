using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TilePaletteObjects
{
    [System.Serializable]
    public class TilePaletteObject
    {
        [SerializeField] private GroundType groundType;
        [SerializeField] private string spriteLayer;


        [SerializeField] private TileBase topLeft;
        [SerializeField] private TileBase topLeftOutline;
        [SerializeField] private Sprite topLeftSprite;

        [SerializeField] private TileBase topMiddle;
        [SerializeField] private TileBase topMiddleOutline;
        [SerializeField] private Sprite topMiddleSprite;

        [SerializeField] private TileBase topRight;
        [SerializeField] private TileBase topRightOutline;
        [SerializeField] private Sprite topRightSprite;

        [SerializeField] private TileBase left;
        [SerializeField] private TileBase leftOutline;
        [SerializeField] private Sprite leftSprite;

        [SerializeField] private TileBase middle;
        [SerializeField] private TileBase middleOutline;
        [SerializeField] private Sprite middleSprite;

        [SerializeField] private TileBase right;
        [SerializeField] private TileBase rightOutline;
        [SerializeField] private Sprite rightSprite;

        [SerializeField] private TileBase bottomLeft;
        [SerializeField] private TileBase bottomLeftOutline;
        [SerializeField] private Sprite bottomLeftSprite;

        [SerializeField] private TileBase bottomMiddle;
        [SerializeField] private TileBase bottomMiddleOutline;
        [SerializeField] private Sprite bottomMiddleSprite;

        [SerializeField] private TileBase bottomRight;
        [SerializeField] private TileBase bottomRightOutline;
        [SerializeField] private Sprite bottomRightSprite;

        [SerializeField] private TileBase columnTop;
        [SerializeField] private TileBase columnTopOutline;
        [SerializeField] private Sprite columnTopSprite;

        [SerializeField] private TileBase columnMiddle;
        [SerializeField] private TileBase columnMiddleOutline;
        [SerializeField] private Sprite columnMiddleSprite;

        [SerializeField] private TileBase columnBottom;
        [SerializeField] private TileBase columnBottomOutline;
        [SerializeField] private Sprite columnBottomSprite;

        [SerializeField] private TileBase rowLeft;
        [SerializeField] private TileBase rowLeftOutline;
        [SerializeField] private Sprite rowLeftSprite;

        [SerializeField] private TileBase rowMiddle;
        [SerializeField] private TileBase rowMiddleOutline;
        [SerializeField] private Sprite rowMiddleSprite;

        [SerializeField] private TileBase rowRight;
        [SerializeField] private TileBase rowRightOutline;
        [SerializeField] private Sprite rowRightSprite;

        [SerializeField] private TileBase single;
        [SerializeField] private TileBase singleOutline;
        [SerializeField] private Sprite singleSprite;

        [SerializeField] private List<TileBase> extraTiles;
        [SerializeField] private List<TileBase> extraOutlines;
        [SerializeField] private List<Sprite> extraSprites;


        private bool compiledTileList = false;

        private Dictionary<TileBase, TilePosition> TilePositionDict = new Dictionary<TileBase, TilePosition>();
        private Dictionary<TileBase, TilePosition> OutlinePositionDict = new Dictionary<TileBase, TilePosition>();
        private Dictionary<Sprite, TilePosition> SpritePositionDict = new Dictionary<Sprite, TilePosition>();


        public string SpriteLayer { get => spriteLayer; }
        public GroundType GroundType { get => groundType; }


        public bool CheckContainsTile(TileBase checkTile)
        {
            if (!compiledTileList)
                InitTilePaletteObject();

            foreach (KeyValuePair<TileBase, TilePosition> pair in TilePositionDict)
                if (pair.Key == checkTile)
                    return true;

            foreach (KeyValuePair<TileBase, TilePosition> pair in OutlinePositionDict)
                if (pair.Key == checkTile)
                    return true;

            return false;
        }

        public TileBase GetOutline(TilePosition position)
        {
            foreach (KeyValuePair<TileBase, TilePosition> pair in OutlinePositionDict)
                if (pair.Value == position)
                    return pair.Key;

            return null;
        }

        public Sprite GetSpriteMask(TilePosition position)
        {
            foreach (KeyValuePair<Sprite, TilePosition> pair in SpritePositionDict)
                if (pair.Value == position)
                    return pair.Key;

            return null;
        }

        public TilePosition GetMapPosition(TileBase checkTile)
        {
            foreach (KeyValuePair<TileBase, TilePosition> pair in TilePositionDict)
                if (checkTile == pair.Key)
                    return pair.Value;

            foreach (KeyValuePair<TileBase, TilePosition> pair in OutlinePositionDict)
                if (checkTile == pair.Key)
                    return pair.Value;

            return TilePosition.NotSwappable;
        }

        private void InitTilePaletteObject()
        {
            compiledTileList = true;

            TilePositionDict.Add(topLeft, TilePosition.TopLeft);
            TilePositionDict.Add(topMiddle, TilePosition.TopMiddle);
            TilePositionDict.Add(topRight, TilePosition.TopRight);
            TilePositionDict.Add(left, TilePosition.Left);
            TilePositionDict.Add(middle, TilePosition.Middle);
            TilePositionDict.Add(right, TilePosition.Right);
            TilePositionDict.Add(bottomLeft, TilePosition.BottomLeft);
            TilePositionDict.Add(bottomMiddle, TilePosition.BottomMiddle);
            TilePositionDict.Add(bottomRight, TilePosition.BottomRight);
            TilePositionDict.Add(columnTop, TilePosition.ColumnTop);
            TilePositionDict.Add(columnMiddle, TilePosition.ColumnMiddle);
            TilePositionDict.Add(columnBottom, TilePosition.ColumnBottom);
            TilePositionDict.Add(rowLeft, TilePosition.RowLeft);
            TilePositionDict.Add(rowMiddle, TilePosition.RowMiddle);
            TilePositionDict.Add(rowRight, TilePosition.RowRight);
            TilePositionDict.Add(single, TilePosition.Single);

            OutlinePositionDict.Add(topLeftOutline, TilePosition.TopLeft);
            OutlinePositionDict.Add(topMiddleOutline, TilePosition.TopMiddle);
            OutlinePositionDict.Add(topRightOutline, TilePosition.TopRight);
            OutlinePositionDict.Add(leftOutline, TilePosition.Left);
            OutlinePositionDict.Add(middleOutline, TilePosition.Middle);
            OutlinePositionDict.Add(right, TilePosition.Right);
            OutlinePositionDict.Add(bottomLeftOutline, TilePosition.BottomLeft);
            OutlinePositionDict.Add(bottomMiddleOutline, TilePosition.BottomMiddle);
            OutlinePositionDict.Add(bottomRightOutline, TilePosition.BottomRight);
            OutlinePositionDict.Add(columnTopOutline, TilePosition.ColumnTop);
            OutlinePositionDict.Add(columnMiddleOutline, TilePosition.ColumnMiddle);
            OutlinePositionDict.Add(columnBottomOutline, TilePosition.ColumnBottom);
            OutlinePositionDict.Add(rowLeftOutline, TilePosition.RowLeft);
            OutlinePositionDict.Add(rowMiddleOutline, TilePosition.RowMiddle);
            OutlinePositionDict.Add(rowRightOutline, TilePosition.RowRight);
            OutlinePositionDict.Add(singleOutline, TilePosition.Single);

            SpritePositionDict.Add(topLeftSprite, TilePosition.TopLeft);
            SpritePositionDict.Add(topMiddleSprite, TilePosition.TopMiddle);
            SpritePositionDict.Add(topRightSprite, TilePosition.TopRight);
            SpritePositionDict.Add(leftSprite, TilePosition.Left);
            SpritePositionDict.Add(middleSprite, TilePosition.Middle);
            SpritePositionDict.Add(rightSprite, TilePosition.Right);
            SpritePositionDict.Add(bottomLeftSprite, TilePosition.BottomLeft);
            SpritePositionDict.Add(bottomMiddleSprite, TilePosition.BottomMiddle);
            SpritePositionDict.Add(bottomRightSprite, TilePosition.BottomRight);
            SpritePositionDict.Add(columnTopSprite, TilePosition.ColumnTop);
            SpritePositionDict.Add(columnMiddleSprite, TilePosition.ColumnMiddle);
            SpritePositionDict.Add(columnBottomSprite, TilePosition.ColumnBottom);
            SpritePositionDict.Add(rowLeftSprite, TilePosition.RowLeft);
            SpritePositionDict.Add(rowMiddleSprite, TilePosition.RowMiddle);
            SpritePositionDict.Add(rowRightSprite, TilePosition.RowRight);
            SpritePositionDict.Add(singleSprite, TilePosition.Single);
        }
    }
}
