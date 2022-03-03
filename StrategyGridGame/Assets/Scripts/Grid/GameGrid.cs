using UnityEngine;

public class GameGrid
{
    private GridCell gridCellPrefab;
    private GridCell[,] grid;
    private Transform cellHolder;

    public float gridSpaceSize { get; private set; }
    public Pathfinding pathFinding { get; private set; }
    public int height { get; private set; }
    public int width { get; private set; }

    public GameGrid(int height, int width, float gridSpaceSize, GridCell gridCellPrefab, Transform gridCellParent)
    {
        this.height = height;
        this.width = width;
        this.gridSpaceSize = gridSpaceSize;
        this.gridCellPrefab = gridCellPrefab;
        this.cellHolder = gridCellParent;

        CreateGrid();
        pathFinding = new Pathfinding(this);
    }

    /// <summary>
    /// Creates the grid with predefined gridsize
    /// </summary>
    private void CreateGrid()
    {
        grid = new GridCell[width, height];

        if (gridCellPrefab == null) Debug.LogError("Grid Cell Prefab is not assigned");
        else
            for (int z = 0; z < height; z++)
                for (int x = 0; x < width; x++)
                {
                    grid[x, z] = GameManager.Instantiate(gridCellPrefab, new Vector3(x * gridSpaceSize, 0, z * gridSpaceSize), Quaternion.identity, cellHolder);
                    grid[x, z].SetPosition(new Vector2Int(x, z));
                    grid[x, z].SetGameGrid(this);

                    // This was the animation but needs to be done differently now
                    // grid[x, z].transform.parent = transform;
                    grid[x, z].gameObject.name = $"Grid Cell ({x}, {z})";
                }
    }

    public void SetCellGCosts()
    {
        for (int x = 0; x < width; x++)
            for (int z = 0; z < height; z++)
            {
                GridCell gridCell = GetGridCell(x, z);
                gridCell.gCost = 99999999;
                gridCell.CalculateFCost();
                gridCell.parentCell = null;
                gridCell.CalculateNeighbours();
            }
    }

    public Vector2Int GetGridPosFromWorld(Vector3 worldPosition)
    {
        // Have to first Ceil and then Floor it because floats just don't like to be consistent
        int x = Mathf.FloorToInt(Mathf.CeilToInt(worldPosition.x) / gridSpaceSize);
        int z = Mathf.FloorToInt(Mathf.CeilToInt(worldPosition.z) / gridSpaceSize);

        x = Mathf.Clamp(x, 0, width);
        z = Mathf.Clamp(z, 0, height);

        return new Vector2Int(x, z);
    }

    public Vector3 GetWorldPosFromGridPos(Vector2Int gridPos)
    {
        float x = gridPos.x * gridSpaceSize;
        float z = gridPos.y * gridSpaceSize;

        return new Vector3(x, 0, z);
    }

    public GridCell GetGridCellFromWorldPos(Vector3 worldPosition)
    {
        Vector2Int gridPos = GetGridPosFromWorld(worldPosition);
        return GetGridCell(gridPos.x, gridPos.y);
    }

    public GridCell GetGridCellFromWorldPos(int x, int z)
    {
        return GetGridCellFromWorldPos(new Vector3(x, z));
    }

    public GridCell GetGridCell(int x, int z)
    {
        return grid[x, z].GetComponent<GridCell>();
    }

    public bool CellIsOccupied(int x, int z)
    {
        GridCell cell = GetGridCell(x, z);
        return cell.isOccupied;
    }

    internal bool CellExists(int x, int z)
    {
        return (x < width && x >= 0 && z < height && z >= 0);
    }
}
