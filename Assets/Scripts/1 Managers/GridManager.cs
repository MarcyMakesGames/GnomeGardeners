using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using TilemapExtensions;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Grid gridMap;
    [SerializeField] private int halfMapSize;
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private Tilemap interactiveTilemap;
    [SerializeField] private Tile hoverTile;
    [SerializeField] private List<GroundTileAssociation> groundTiles;
    [SerializeField] private GameObject cellPrefab;

    private List<GridCell> gridCells = new List<GridCell>();
    private GridCell targetCell;
    private TilePaletteObject targetTilePalette;


    public List<GridCell> GridCells { get => gridCells; }

    public VoidEventChannelSO OnTileChanged;

    #region Unity Methods

    private void Awake()
    {
        if (gridMap == null)
            gridMap = GetComponent<Grid>();
        if (groundTilemap == null)
            throw new System.NotImplementedException("Did not assign the ground tilemap.");
        if (interactiveTilemap == null)
            throw new System.NotImplementedException("Did not assign the interactive tilemap.");

        GameManager.Instance.GridManager = this;
        LogTileMap(halfMapSize);
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Returns Vector3Ints on the grid map that are -1 and +1 x and y from the origin.
    /// </summary>
    /// <param name="origin"></param>
    /// <returns></returns>
    public List<GridCell> GetNeighborCells(Vector2Int origin, int checkDistance = 1)
    {
        List<GridCell> neighbors = new List<GridCell>();

        for (int i = origin.x - checkDistance; i <= origin.x + checkDistance; i++)
            for(int j = origin.y - checkDistance; j <= origin.y + checkDistance; j++)
            {
                Vector2Int checkGrid = new Vector2Int(i, j);

                if (checkGrid == origin)
                    continue;

                foreach (GridCell cell in gridCells)
                    if (checkGrid == cell.GridPosition)
                    {
                        neighbors.Add(cell);
                        break;
                    }
            }
        
        return neighbors;
    }

    public void ChangeTile(Vector2Int gridPosition, GroundType groundType)
    {
        AssignTargetCell(gridPosition);

        if(targetCell == null)
        {
            Debug.Log("Could not find target position in cell list.");
            return;
        }

        targetTilePalette = null;

        foreach(GroundTileAssociation groundTileAssociation in groundTiles)
            if(groundType == groundTileAssociation.groundType)
            {
                targetTilePalette = groundTileAssociation.tilePalette;
                break;
            }
        
        if(targetTilePalette == null)
        {
            Debug.Log("Could not find target tile palette associated with Ground Type.");
            return;
        }


        PaintTile(gridPosition, targetCell.MapPosition, targetTilePalette);

        OnTileChanged.RaiseEvent();
    }

    public void ChangeTileOccupant(Vector2Int gridPosition, IOccupant occupant)
    {
        AssignTargetCell(gridPosition);

        if (targetCell == null)
        {
            Debug.Log("Could not find target position in cell list.");
            return;
        }

        targetCell.RemoveCellOccupant();
        targetCell.AddCellOccupant(occupant);
    }

    public Vector2Int GetClosestGrid(Vector3 origin)
    {
        targetCell = null;

        foreach(GridCell cell in gridCells)
        {
            if (targetCell == null || Vector3.Distance(cell.WorldPosition, origin) <= Vector3.Distance(targetCell.WorldPosition, origin))
                targetCell = cell;
        }

        return targetCell.GridPosition;
    }

    public GridCell GetClosestCell(Vector3 origin)
    {
        targetCell = null;

        foreach (GridCell cell in gridCells)
        {
            if (targetCell == null || Vector3.Distance(cell.WorldPosition, origin) <= Vector3.Distance(targetCell.WorldPosition, origin))
                targetCell = cell;
        }

        return targetCell;
    }

    public GridCell GetGridCell(Vector2Int gridPosition)
    {
        foreach (GridCell cell in gridCells)
            if (cell.GridPosition == gridPosition)
                return cell;

        return null;
    }

    public void HighlightTile(Vector2Int gridPosition, Vector2Int previousGridPosition)
    {
        if (!gridPosition.Equals(previousGridPosition))
        {
            interactiveTilemap.PaintTile(previousGridPosition, null); // Remove old hoverTile
            interactiveTilemap.PaintTile(gridPosition, hoverTile);
        }
    }

    #endregion

    #region Private Methods

    private void AssignTargetCell (Vector2Int gridPosition)
    {
        targetCell = null;

        foreach (GridCell cell in gridCells)
            if (cell.GridPosition == gridPosition)
            {
                targetCell = cell;
                break;
            }
    }

    #endregion

    #region MapGeneration
    private void LogTileMap(int mapSize)
    {
        for (int i = -mapSize; i <= mapSize; i++)
            for (int j = -mapSize; j <= mapSize; j++)
            {
                Vector2Int gridPosition = new Vector2Int(i, j);
                Vector3 worldPosition = gridMap.CellToWorld((Vector3Int)gridPosition);

                TileBase tile = groundTilemap.GetTile((Vector3Int)gridPosition);
                GroundType groundType = GetTileGroundType(tile);
                TilePosition tilePosition = GetTilePosition(tile);
                Sprite spriteMask = GetTilePaletteObject(groundType).SpriteMask;

                gridCells.Add(CreateCell(gridPosition, worldPosition, groundType, tilePosition, spriteMask));
            }
    }

    private GridCell CreateCell(Vector2Int gridPosition, Vector3 worldPosition, GroundType typeOfGround, TilePosition tilePosition, Sprite spriteMask)
    {
        GameObject newCell = Instantiate(cellPrefab, worldPosition, Quaternion.identity, transform);
        GridCell cell = newCell.GetComponent<GridCell>();
        cell.InitGridCell(gridPosition, worldPosition, typeOfGround, tilePosition, null, spriteMask);

        return cell;
    }

    private void PaintTile(Vector2Int gridPosition, TilePosition mapPosition, TilePaletteObject tilePalette)
    {
        switch(mapPosition)
        {
            case TilePosition.TopLeft:
                groundTilemap.PaintTile(gridPosition, tilePalette.TopLeft);
                break;
            case TilePosition.TopMiddle:
                groundTilemap.PaintTile(gridPosition, tilePalette.TopMiddle);
                break;
            case TilePosition.TopRight:
                groundTilemap.PaintTile(gridPosition, tilePalette.TopRight);
                break;
            case TilePosition.Left:
                groundTilemap.PaintTile(gridPosition, tilePalette.Left);
                break;
            case TilePosition.Middle:
                groundTilemap.PaintTile(gridPosition, tilePalette.Middle);
                break;
            case TilePosition.Right:
                groundTilemap.PaintTile(gridPosition, tilePalette.Right);
                break;
            case TilePosition.BottomLeft:
                groundTilemap.PaintTile(gridPosition, tilePalette.BottomLeft);
                break;
            case TilePosition.BottomMiddle:
                groundTilemap.PaintTile(gridPosition, tilePalette.BottomMiddle);
                break;
            case TilePosition.BottomRight:
                groundTilemap.PaintTile(gridPosition, tilePalette.BottomRight);
                break;
            case TilePosition.ColumnTop:
                groundTilemap.PaintTile(gridPosition, tilePalette.ColumnTop);
                break;
            case TilePosition.ColumnMiddle:
                groundTilemap.PaintTile(gridPosition, tilePalette.ColumnMiddle);
                break;
            case TilePosition.ColumnBottom:
                groundTilemap.PaintTile(gridPosition, tilePalette.ColumnBottom);
                break;
            case TilePosition.RowLeft:
                groundTilemap.PaintTile(gridPosition, tilePalette.RowLeft);
                break;
            case TilePosition.RowMiddle:
                groundTilemap.PaintTile(gridPosition, tilePalette.RowMiddle);
                break;
            case TilePosition.RowRight:
                groundTilemap.PaintTile(gridPosition, tilePalette.RowRight);
                break;
            case TilePosition.Single:
                groundTilemap.PaintTile(gridPosition, tilePalette.Single);
                break;
        }
    }

    private GroundType GetTileGroundType(TileBase checkTile)
    {
        foreach (GroundTileAssociation tileset in groundTiles)
            if (tileset.tilePalette.CheckContainsTile(checkTile))
                return tileset.groundType;

        return GroundType.None;
    }

    private TilePosition GetTilePosition(TileBase checkTile)
    {
        foreach (GroundTileAssociation tileset in groundTiles)
            if (tileset.tilePalette.CheckContainsTile(checkTile))
                return tileset.tilePalette.GetMapPosition(checkTile);

        return TilePosition.NotSwappable;
    }

    private TilePaletteObject GetTilePaletteObject(GroundType groundType)
    {
        foreach (GroundTileAssociation groundTileAssociation in groundTiles)
            if (groundType == groundTileAssociation.groundType)
                return groundTileAssociation.tilePalette;

        return null;
    }
    #endregion
}
