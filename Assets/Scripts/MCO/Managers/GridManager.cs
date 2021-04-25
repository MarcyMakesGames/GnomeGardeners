using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using TilemapExtensions;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Grid gridMap;
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private int halfMapSize;
    [SerializeField] private List<GroundTileAssociation> groundTiles;
    [SerializeField] private Tilemap interactiveTilemap;
    [SerializeField] private Tile hoverTile;
    [SerializeField] private bool createEmptyTileMap;

    private List<GridCell> gridCells = new List<GridCell>();
    private GridCell targetCell;
    private TilePaletteObject targetTilePalette;

    Vector2Int previousGridPosition = new Vector2Int();

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
    }

    private void Start()
    {
        if (createEmptyTileMap)
            CreateEmptyTileMap(halfMapSize);
        else
            CreateTileMap(halfMapSize);
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
        targetCell.GroundType = groundType;

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

        targetCell.Occupant = occupant;
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

    public void HighlightTile(Vector2Int gridPosition)
    {
        if (!gridPosition.Equals(previousGridPosition))
        {
            interactiveTilemap.PaintTile(previousGridPosition, null); // Remove old hoverTile
            interactiveTilemap.PaintTile(gridPosition, hoverTile);
            previousGridPosition = gridPosition;
        }
    }

    public void FlashHighlightTile(Vector2Int gridPosition)
    {
        if (!gridPosition.Equals(previousGridPosition))
        {
            interactiveTilemap.color = Color.white;
            interactiveTilemap.color = Color.red;
            previousGridPosition = gridPosition;
        }
    }

    #endregion

    #region Private Methods

    private void AssignTargetCell (Vector2Int gridPosition)
    {
        targetCell = null;

        foreach (GridCell cell in gridCells)
        {
            if (cell.GridPosition == gridPosition)
            {
                targetCell = cell;
                break;
            }
        }
    }

    #endregion

    #region MapGeneration
    private void CreateTileMap(int mapSize)
    {
        for (int i = -mapSize; i <= mapSize; i++)
            for (int j = -mapSize; j <= mapSize; j++)
            {
                Vector2Int gridPosition = new Vector2Int(i, j);
                Vector3 worldPosition = gridMap.CellToWorld((Vector3Int)gridPosition);

                GridCell cell = CreateCell(gridPosition, worldPosition, groundTiles[0].groundType, mapSize);
                gridCells.Add(cell);

                PaintTile(cell.GridPosition, cell.MapPosition, groundTiles[0].tilePalette);
            }
    }
    private void CreateEmptyTileMap(int mapSize)
    {
        for (int i = -mapSize; i <= mapSize; i++)
            for (int j = -mapSize; j <= mapSize; j++)
            {
                Vector2Int gridPosition = new Vector2Int(i, j);
                Vector3 worldPosition = gridMap.CellToWorld((Vector3Int)gridPosition);

                GridCell cell = CreateCell(gridPosition, worldPosition, groundTiles[0].groundType, mapSize);
                gridCells.Add(cell);
            }
    }

    private GridCell CreateCell(Vector2Int gridPosition, Vector3 worldPosition, GroundType typeOfGround, int mapSize)
    {
        return new GridCell(gridPosition, worldPosition, typeOfGround, GetMapPosition(gridPosition, mapSize), null);
    }

    private MapPosition GetMapPosition(Vector2Int gridPosition, int mapSize)
    {

        if (gridPosition.x == mapSize && gridPosition.y == mapSize)
            return MapPosition.TopRight;

        else if (gridPosition.x == mapSize && gridPosition.y == -mapSize)
            return MapPosition.BottomRight;

        else if (gridPosition.x == -mapSize && gridPosition.y == mapSize)
            return MapPosition.TopLeft;

        else if (gridPosition.x == -mapSize && gridPosition.y == -mapSize)
            return MapPosition.BottomLeft;

        else if (gridPosition.x == mapSize)
            return MapPosition.Right;

        else if (gridPosition.x == -mapSize)
            return MapPosition.Left;

        else if (gridPosition.y == mapSize)
            return MapPosition.TopMiddle;

        else if (gridPosition.y == -mapSize)
            return MapPosition.BottomMiddle;

        return MapPosition.Middle;
    }

    private void PaintTile(Vector2Int gridPosition, MapPosition mapPosition, TilePaletteObject tilePalette)
    {
        switch(mapPosition)
        {
            case MapPosition.TopLeft:
                groundTilemap.PaintTile(gridPosition, tilePalette.TopLeft);
                break;
            case MapPosition.TopMiddle:
                groundTilemap.PaintTile(gridPosition, tilePalette.TopMiddle);
                break;
            case MapPosition.TopRight:
                groundTilemap.PaintTile(gridPosition, tilePalette.TopRight);
                break;
            case MapPosition.Left:
                groundTilemap.PaintTile(gridPosition, tilePalette.Left);
                break;
            case MapPosition.Middle:
                groundTilemap.PaintTile(gridPosition, tilePalette.Middle);
                break;
            case MapPosition.Right:
                groundTilemap.PaintTile(gridPosition, tilePalette.Right);
                break;
            case MapPosition.BottomLeft:
                groundTilemap.PaintTile(gridPosition, tilePalette.BottomLeft);
                break;
            case MapPosition.BottomMiddle:
                groundTilemap.PaintTile(gridPosition, tilePalette.BottomMiddle);
                break;
            case MapPosition.BottomRight:
                groundTilemap.PaintTile(gridPosition, tilePalette.BottomRight);
                break;
        }
    }
    #endregion
}
