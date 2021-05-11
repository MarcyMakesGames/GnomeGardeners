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


        [SerializeField] private TileBase roundedTopLeft;
        [SerializeField] private TileBase roundedTopLeftOutline;
        [SerializeField] private Sprite roundedTopLeftSprite;

        [SerializeField] private TileBase roundedTopMiddle;
        [SerializeField] private TileBase roundedTopMiddleOutline;
        [SerializeField] private Sprite roundedTopMiddleSprite;

        [SerializeField] private TileBase roundedTopRight;
        [SerializeField] private TileBase roundedTopRightOutline;
        [SerializeField] private Sprite roundedTopRightSprite;

        [SerializeField] private TileBase roundedLeft;
        [SerializeField] private TileBase roundedLeftOutline;
        [SerializeField] private Sprite roundedLeftSprite;

        [SerializeField] private TileBase roundedMiddle;
        [SerializeField] private TileBase roundedMiddleOutline;
        [SerializeField] private Sprite roundedMiddleSprite;

        [SerializeField] private TileBase roundedRight;
        [SerializeField] private TileBase roundedRightOutline;
        [SerializeField] private Sprite roundedRightSprite;

        [SerializeField] private TileBase roundedBottomLeft;
        [SerializeField] private TileBase roundedBottomLeftOutline;
        [SerializeField] private Sprite roundedBottomLeftSprite;

        [SerializeField] private TileBase roundedBottomMiddle;
        [SerializeField] private TileBase roundedBottomMiddleOutline;
        [SerializeField] private Sprite roundedBottomMiddleSprite;

        [SerializeField] private TileBase roundedBottomRight;
        [SerializeField] private TileBase roundedBottomRightOutline;
        [SerializeField] private Sprite roundedBottomRightSprite;

        [SerializeField] private TileBase juncture1;
        [SerializeField] private TileBase juncture1Outline;
        [SerializeField] private Sprite juncture1Sprite;

        [SerializeField] private TileBase juncture2;
        [SerializeField] private TileBase juncture2Outline;
        [SerializeField] private Sprite juncture2Sprite;

        [SerializeField] private TileBase juncture3;
        [SerializeField] private TileBase juncture3Outline;
        [SerializeField] private Sprite juncture3Sprite;

        [SerializeField] private TileBase juncture4;
        [SerializeField] private TileBase juncture4Outline;
        [SerializeField] private Sprite juncture4Sprite;

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

            if (roundedBottomLeft != null)
                TilePositionDict.Add(roundedBottomLeft, TilePosition.BottomLeft);
            if (roundedBottomMiddle != null)
                TilePositionDict.Add(roundedBottomMiddle, TilePosition.BottomMiddle);
            if (roundedBottomRight != null)
                TilePositionDict.Add(roundedBottomRight, TilePosition.BottomRight);
            if (roundedLeft != null)
                TilePositionDict.Add(roundedLeft, TilePosition.RoundedLeft);
            if (roundedMiddle != null)
                TilePositionDict.Add(roundedMiddle, TilePosition.RoundedMiddle);
            if (roundedRight != null)
                TilePositionDict.Add(roundedRight, TilePosition.RoundedRight);
            if (roundedTopLeft != null)
                TilePositionDict.Add(roundedTopLeft, TilePosition.RoundedTopLeft);
            if (roundedTopMiddle != null)
                TilePositionDict.Add(roundedTopMiddle, TilePosition.RoundedTopMiddle);
            if (roundedTopRight != null)
                TilePositionDict.Add(roundedTopRight, TilePosition.RoundedTopRight);
            if (juncture1 != null)
                TilePositionDict.Add(juncture1, TilePosition.Juncture1);
            if (juncture2 != null)
                TilePositionDict.Add(juncture2, TilePosition.Juncture2);
            if (juncture3 != null)
                TilePositionDict.Add(juncture3, TilePosition.Juncture3);
            if (juncture4 != null)
                TilePositionDict.Add(juncture4, TilePosition.Juncture4);

            OutlinePositionDict.Add(topLeftOutline, TilePosition.TopLeft);
            OutlinePositionDict.Add(topMiddleOutline, TilePosition.TopMiddle);
            OutlinePositionDict.Add(topRightOutline, TilePosition.TopRight);
            OutlinePositionDict.Add(leftOutline, TilePosition.Left);
            OutlinePositionDict.Add(middleOutline, TilePosition.Middle);
            OutlinePositionDict.Add(rightOutline, TilePosition.Right);
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

            if (roundedBottomLeftOutline != null)
                OutlinePositionDict.Add(roundedBottomLeftOutline, TilePosition.RoundedBottomLeft);
            if (roundedBottomMiddleOutline != null)
                OutlinePositionDict.Add(roundedBottomMiddleOutline, TilePosition.RoundedBottomMiddle);
            if (roundedBottomRight != null)
                OutlinePositionDict.Add(roundedBottomRightOutline, TilePosition.RoundedBottomRight);
            if (roundedLeftOutline != null)
                OutlinePositionDict.Add(roundedLeftOutline, TilePosition.RoundedLeft);
            if (roundedMiddleOutline != null)
                OutlinePositionDict.Add(roundedMiddleOutline, TilePosition.RoundedMiddle);
            if (roundedRightOutline != null)
                OutlinePositionDict.Add(roundedRightOutline, TilePosition.RoundedRight);
            if (roundedTopLeftOutline != null)
                OutlinePositionDict.Add(roundedTopLeftOutline, TilePosition.RoundedTopLeft);
            if (roundedTopMiddleOutline != null)
                OutlinePositionDict.Add(roundedTopMiddleOutline, TilePosition.RoundedTopMiddle);
            if (roundedTopRightOutline != null)
                OutlinePositionDict.Add(roundedTopRightOutline, TilePosition.RoundedTopRight);
            if (juncture1Outline != null)
                OutlinePositionDict.Add(juncture1Outline, TilePosition.Juncture1);
            if (juncture2Outline != null)
                OutlinePositionDict.Add(juncture2Outline, TilePosition.Juncture2);
            if (juncture3Outline != null)
                OutlinePositionDict.Add(juncture3Outline, TilePosition.Juncture3);
            if (juncture4Outline != null)
                OutlinePositionDict.Add(juncture4Outline, TilePosition.Juncture4);

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

            if (roundedBottomLeftSprite != null)
                SpritePositionDict.Add(roundedBottomLeftSprite, TilePosition.BottomLeft);
            if (roundedBottomMiddleSprite != null)
                SpritePositionDict.Add(roundedBottomMiddleSprite, TilePosition.BottomMiddle);
            if (roundedBottomRightSprite != null)
                SpritePositionDict.Add(roundedBottomRightSprite, TilePosition.BottomRight);
            if (roundedLeft != null)
                SpritePositionDict.Add(roundedLeftSprite, TilePosition.RoundedLeft);
            if (roundedMiddleSprite != null)
                SpritePositionDict.Add(roundedMiddleSprite, TilePosition.RoundedMiddle);
            if (roundedRightSprite != null)
                SpritePositionDict.Add(roundedRightSprite, TilePosition.RoundedRight);
            if (roundedTopLeftSprite != null)
                SpritePositionDict.Add(roundedTopLeftSprite, TilePosition.RoundedTopLeft);
            if (roundedTopMiddleSprite != null)
                SpritePositionDict.Add(roundedTopMiddleSprite, TilePosition.RoundedTopMiddle);
            if (roundedTopRightSprite != null)
                SpritePositionDict.Add(roundedTopRightSprite, TilePosition.RoundedTopRight);
            if (juncture1Sprite != null)
                SpritePositionDict.Add(juncture1Sprite, TilePosition.Juncture1);
            if (juncture2Sprite != null)
                SpritePositionDict.Add(juncture2Sprite, TilePosition.Juncture2);
            if (juncture3Sprite != null)
                SpritePositionDict.Add(juncture3Sprite, TilePosition.Juncture3);
            if (juncture4Sprite != null)
                SpritePositionDict.Add(juncture4Sprite, TilePosition.Juncture4);
        }
    }
}
