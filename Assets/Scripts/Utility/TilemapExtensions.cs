using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

namespace TilemapExtensions
{
    public static class TilemapExtensions
    {
        public static void PaintTiles(this Tilemap map, IEnumerable<Vector2Int> fillList, Tile fill)
        {
            foreach (Vector2Int location in fillList)
                PaintTile(map, location, fill);
        }

        public static void PaintTile(this Tilemap map, Vector2Int gridPosition, Tile fill)
        {
            map.SetTile((Vector3Int)gridPosition, fill);
        }

        public static void PaintTile(this Tilemap map, Vector2Int gridPosition, TileBase fill)
        {
            map.SetTile((Vector3Int)gridPosition, fill);
        }
    }
}