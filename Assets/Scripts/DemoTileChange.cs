using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoTileChange : MonoBehaviour
{
    GridManager gridManager;
    [SerializeField] Vector3Int changeTilePosition;

    // Start is called before the first frame update
    void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
    }

    [ContextMenu("Change Tile")]
    protected void ChangeTile()
    {
        gridManager.ChangeTile(changeTilePosition, GroundType.Dirt);
    }
}
