using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

namespace TilemapExtensions
{
    public static class TilemapExtensions
    {
        public static void PaintTiles(this Tilemap map, IEnumerable<Vector3Int> fillList, Tile fill)
        {
            foreach (Vector3Int location in fillList)
                PaintTile(map, location, fill);
        }

        public static void PaintTile(this Tilemap map, Vector3Int gridPosition, Tile fill)
        {
            map.SetTile(gridPosition, fill);
        }
    }
}