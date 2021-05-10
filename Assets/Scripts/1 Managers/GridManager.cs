using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using TilemapExtensions;
using TilePaletteObjects;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Grid gridMap;
    [SerializeField] private int halfMapSize;
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private Tilemap interactiveTilemap;
    [SerializeField] private Tile hoverTile;
    [SerializeField] private List<TilePaletteObject> groundTiles;
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private GameObject grassWaterColor;
    [SerializeField] private GameObject arableWaterColor;
    [SerializeField] private GameObject fallowWaterColor;
    [SerializeField] private GameObject pathWaterColor;

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

        foreach(TilePaletteObject tilePalette in groundTiles)
            if(groundType == tilePalette.GroundType)
            {
                targetTilePalette = tilePalette;
                break;
            }
        
        if(targetTilePalette == null)
        {
            Debug.Log("Could not find target tile palette associated with Ground Type.");
            return;
        }

        targetCell.ChangeSpriteTarget(targetTilePalette.GetSpriteMask(targetCell.MapPosition));
        targetCell.ChangeGroundType(groundType);

        targetCell.transform.parent =   targetTilePalette.SpriteLayer == "Arable" ? arableWaterColor.transform :
                                        targetTilePalette.SpriteLayer == "Fallow" ? fallowWaterColor.transform :
                                        targetTilePalette.SpriteLayer == "Path" ? pathWaterColor.transform : grassWaterColor.transform;

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
                TilePaletteObject tilePalette = GetTilePaletteObject(tile);

                if (tilePalette != null)
                {
                    GroundType tileType = tilePalette.GroundType;
                    TilePosition position = tilePalette.GetMapPosition(tile);
                    string spriteLayer = tilePalette.SpriteLayer;
                    Sprite spriteMask = tilePalette.GetSpriteMask(position);

                    PaintTile(gridPosition, position, tilePalette);
                    gridCells.Add(CreateCell(gridPosition, worldPosition, tileType, position, spriteMask, spriteLayer));
                }
                else
                {
                    GroundType tileType = GroundType.Grass;
                    TilePosition position = TilePosition.NotSwappable;
                    string spriteLayer = "Grass";

                    gridCells.Add(CreateCell(gridPosition, worldPosition, tileType, position, null, spriteLayer));
                }
            }
    }

    GameObject newCell;
    private GridCell CreateCell(Vector2Int gridPosition, Vector3 worldPosition, GroundType typeOfGround, TilePosition tilePosition, Sprite spriteMask, string spriteLayer)
    {

        switch(typeOfGround)
        {
            case GroundType.Grass:
                newCell = Instantiate(cellPrefab, worldPosition, Quaternion.identity, grassWaterColor.transform);
                newCell.GetComponent<SpriteMask>().enabled = false;
                break;
            case GroundType.ArableSoil:
                newCell = Instantiate(cellPrefab, worldPosition, Quaternion.identity, arableWaterColor.transform);
                break;
            case GroundType.FallowSoil:
                newCell = Instantiate(cellPrefab, worldPosition, Quaternion.identity, fallowWaterColor.transform);
                break;
            case GroundType.Path:
                newCell = Instantiate(cellPrefab, worldPosition, Quaternion.identity, pathWaterColor.transform);
                break;
        }

        GridCell cell = newCell.GetComponent<GridCell>();

        cell.InitGridCell(gridPosition, worldPosition, typeOfGround, tilePosition, null, spriteMask);

        return cell;
    }

    private void PaintTile(Vector2Int gridPosition, TilePosition mapPosition, TilePaletteObject tilePalette)
    {
        switch(mapPosition)
        {
            case TilePosition.TopLeft:
                groundTilemap.PaintTile(gridPosition, tilePalette.GetOutline(TilePosition.TopLeft));
                break;
            case TilePosition.TopMiddle:
                groundTilemap.PaintTile(gridPosition, tilePalette.GetOutline(TilePosition.TopMiddle));
                break;
            case TilePosition.TopRight:
                groundTilemap.PaintTile(gridPosition, tilePalette.GetOutline(TilePosition.TopRight));
                break;
            case TilePosition.Left:
                groundTilemap.PaintTile(gridPosition, tilePalette.GetOutline(TilePosition.Left));
                break;
            case TilePosition.Middle:
                groundTilemap.PaintTile(gridPosition, tilePalette.GetOutline(TilePosition.Middle));
                break;
            case TilePosition.Right:
                groundTilemap.PaintTile(gridPosition, tilePalette.GetOutline(TilePosition.Right));
                break;
            case TilePosition.BottomLeft:
                groundTilemap.PaintTile(gridPosition, tilePalette.GetOutline(TilePosition.BottomLeft));
                break;
            case TilePosition.BottomMiddle:
                groundTilemap.PaintTile(gridPosition, tilePalette.GetOutline(TilePosition.BottomMiddle));
                break;
            case TilePosition.BottomRight:
                groundTilemap.PaintTile(gridPosition, tilePalette.GetOutline(TilePosition.BottomRight));
                break;
            case TilePosition.ColumnTop:
                groundTilemap.PaintTile(gridPosition, tilePalette.GetOutline(TilePosition.ColumnTop));
                break;
            case TilePosition.ColumnMiddle:
                groundTilemap.PaintTile(gridPosition, tilePalette.GetOutline(TilePosition.ColumnMiddle));
                break;
            case TilePosition.ColumnBottom:
                groundTilemap.PaintTile(gridPosition, tilePalette.GetOutline(TilePosition.ColumnBottom));
                break;
            case TilePosition.RowLeft:
                groundTilemap.PaintTile(gridPosition, tilePalette.GetOutline(TilePosition.RowLeft));
                break;
            case TilePosition.RowMiddle:
                groundTilemap.PaintTile(gridPosition, tilePalette.GetOutline(TilePosition.RowMiddle));
                break;
            case TilePosition.RowRight:
                groundTilemap.PaintTile(gridPosition, tilePalette.GetOutline(TilePosition.RowRight));
                break;
            case TilePosition.Single:
                groundTilemap.PaintTile(gridPosition, tilePalette.GetOutline(TilePosition.Single));
                break;
            case TilePosition.RoundedBottomLeft:
                groundTilemap.PaintTile(gridPosition, tilePalette.GetOutline(TilePosition.RoundedBottomLeft));
                break;
            case TilePosition.RoundedBottomMiddle:
                groundTilemap.PaintTile(gridPosition, tilePalette.GetOutline(TilePosition.RoundedBottomMiddle));
                break;
            case TilePosition.RoundedBottomRight:
                groundTilemap.PaintTile(gridPosition, tilePalette.GetOutline(TilePosition.RoundedBottomRight));
                break;
            case TilePosition.RoundedLeft:
                groundTilemap.PaintTile(gridPosition, tilePalette.GetOutline(TilePosition.RoundedLeft));
                break;
            case TilePosition.RoundedMiddle:
                groundTilemap.PaintTile(gridPosition, tilePalette.GetOutline(TilePosition.RoundedMiddle));
                break;
            case TilePosition.RoundedRight:
                groundTilemap.PaintTile(gridPosition, tilePalette.GetOutline(TilePosition.RoundedRight));
                break;
            case TilePosition.RoundedTopLeft:
                groundTilemap.PaintTile(gridPosition, tilePalette.GetOutline(TilePosition.RoundedTopLeft));
                break;
            case TilePosition.RoundedTopMiddle:
                groundTilemap.PaintTile(gridPosition, tilePalette.GetOutline(TilePosition.RoundedTopMiddle));
                break;
            case TilePosition.RoundedTopRight:
                groundTilemap.PaintTile(gridPosition, tilePalette.GetOutline(TilePosition.RoundedTopRight));
                break;
            case TilePosition.Juncture1:
                groundTilemap.PaintTile(gridPosition, tilePalette.GetOutline(TilePosition.Juncture1));
                break;
            case TilePosition.Juncture2:
                groundTilemap.PaintTile(gridPosition, tilePalette.GetOutline(TilePosition.Juncture2));
                break;
            case TilePosition.Juncture3:
                groundTilemap.PaintTile(gridPosition, tilePalette.GetOutline(TilePosition.Juncture3));
                break;
            case TilePosition.Juncture4:
                groundTilemap.PaintTile(gridPosition, tilePalette.GetOutline(TilePosition.Juncture4));
                break;
        }
    }

    private TilePaletteObject GetTilePaletteObject(TileBase tile)
    {
        foreach (TilePaletteObject tilePalette in groundTiles)
            if (tilePalette.CheckContainsTile(tile))
                return tilePalette;

        return null;
    }
    #endregion
}
