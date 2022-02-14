using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid
{
    private float gridSpaceSize = 10f;

    private GameObject gridCellPrefab;
    private GameObject[,] grid;

    public Pathfinding pathFinding { get; private set; }
    public int height { get; private set; }
    public int width { get; private set; }

    public GameGrid(int height, int width, GameObject gridCellPrefab)
    {
        this.height = height;
        this.width = width;
        this.gridCellPrefab = gridCellPrefab;

        CreateGrid();
        pathFinding = new Pathfinding(this);
    }

    /// <summary>
    /// Creates the grid with predefined gridsize
    /// </summary>
    private void CreateGrid()
    {
        grid = new GameObject[width, height];

        if (gridCellPrefab == null)
        {
            Debug.LogError("Grid Cell Prefab is not assigned");
            // yield return null;
        }
        else
        {
            for (int z = 0; z < height; z++)
            {
                for (int x = 0; x < width; x++)
                {
                    grid[x, z] = GameManager.Instantiate(gridCellPrefab, new Vector3(x * gridSpaceSize, 0, z * gridSpaceSize), Quaternion.identity);
                    grid[x, z].GetComponent<GridCell>().SetPosition(new Vector2Int(x, z));
                    // This was the animation but needs to be done differently now
                    // grid[x, z].transform.parent = transform;
                    grid[x, z].gameObject.name = $"Grid Cell ({x}, {z})";

                    // yield return new WaitForSeconds(0.02f);
                }
            }
        }
    }

    public void SetCellGCosts()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridCell gridCell = GetGridCellFromWorldPos(x, z);
                gridCell.gCost = 99999999;
                gridCell.CalculateFCost();
                gridCell.parentCell = null;
                gridCell.CalculateNeighbours();
            }
        }
    }

    public Vector2Int GetGridPosFromWorld(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt(worldPosition.x / gridSpaceSize);
        int z = Mathf.FloorToInt(worldPosition.y / gridSpaceSize);

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
        return grid[gridPos.x, gridPos.y].GetComponent<GridCell>();
    }

    public GridCell GetGridCellFromWorldPos(int x, int z)
    {
        return GetGridCellFromWorldPos(new Vector3(x, 0, z));
    }
}
