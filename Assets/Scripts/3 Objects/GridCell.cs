using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners 
{
    public class GridCell : MonoBehaviour
    {
        private Vector2Int gridPosition;
        private Vector3 worldPosition;
        private GroundType groundType;
        private TilePosition mapPosition;
        private Occupant occupant;


        public Vector2Int GridPosition { get => gridPosition; }
        public Vector3 WorldPosition { get => worldPosition; }
        public GroundType GroundType { get => groundType; set => groundType = value; }
        public TilePosition MapPosition { get => mapPosition; }
        public Occupant Occupant { get => occupant; set => occupant = value; }

        public void InitGridCell(Vector2Int positionOnGrid, Vector3 positionInWorld, GroundType typeOfGround, TilePosition positionOnMap, Occupant objectInCell, Sprite spriteMask)
        {
            gridPosition = positionOnGrid;
            worldPosition = positionInWorld;
            transform.position = worldPosition;
            groundType = typeOfGround;
            mapPosition = positionOnMap;
            occupant = objectInCell;
            ChangeSpriteTarget(spriteMask);
        }


        public void ChangeSpriteTarget(Sprite spriteMask)
        {
            SpriteMask mask = GetComponent<SpriteMask>();

            if (spriteMask != null)
                mask.sprite = spriteMask;
        }

        public void ChangeGroundType(GroundType groundType)
        {
            this.groundType = groundType;
        }
    }
}
