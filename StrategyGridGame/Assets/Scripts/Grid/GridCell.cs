using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    public int x, z;
    private int posX, posZ;
    private GameGrid gameGrid;
    private bool diagonallyWalkable;

    // Saves a reference to the GameObject that gets placed on this cell
    public GridObject objectInThisGrid = null;
    public bool isOccupied;
    public int fCost, gCost, hCost;
    public GridCell parentCell;
    public List<GridCell> neighbourList;

    public GridCell (GameGrid grid, int x, int z)
    {
        gameGrid = grid;
        this.x = x;
        this.z = z;
        isOccupied = false;
        // Set this to false if you don't want units to be able to move diagonally
        diagonallyWalkable = false;
    }

    public void SetPosition(Vector2Int pos)
    {
        posX = pos.x;
        posZ = pos.y;
    }

    public Vector2Int GetPosition()
    {
        return new Vector2Int(posX, posZ);
    }

    /// <summary>
    /// Calculates the cost for traversing this cell in pathfinding
    /// </summary>
    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public void CalculateNeighbours()
    {
        neighbourList = new List<GridCell>();
        if (x - 1 >= 0)
        {
            // Left Node
            neighbourList.Add(gameGrid.GetGridCellFromWorldPos(x - 1, z));
            if (diagonallyWalkable)
            {
                // Lower Left Node
                if (z - 1 >= 0) neighbourList.Add(gameGrid.GetGridCellFromWorldPos(x - 1, z - 1));
                // Upper Left Node
                if (z + 1 < gameGrid.height) neighbourList.Add(gameGrid.GetGridCellFromWorldPos(x - 1, z + 1));
            }
        }
        if (x + 1 < gameGrid.width)
        {
            // Right Node
            neighbourList.Add(gameGrid.GetGridCellFromWorldPos(x + 1, z));
            if (diagonallyWalkable)
            {
                // Lower Right Node
                if (z - 1 >= 0) neighbourList.Add(gameGrid.GetGridCellFromWorldPos(x + 1, z - 1));
                // Upper Right Node
                if (z + 1 < gameGrid.height) neighbourList.Add(gameGrid.GetGridCellFromWorldPos(x + 1, z + 1));
            }
        }
        // Lower Node
        if (z - 1 >= 0) neighbourList.Add(gameGrid.GetGridCellFromWorldPos(x, z - 1));
        // Upper Node
        if (z + 1 < gameGrid.height) neighbourList.Add(gameGrid.GetGridCellFromWorldPos(x, z + 1));
    }

    public void ToggleOccupation()
    {
        isOccupied = !isOccupied;
    }
}
