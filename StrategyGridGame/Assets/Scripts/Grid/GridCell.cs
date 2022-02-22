using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    private int posX, posZ;
    public GameGrid gameGrid { get; set; }
    // Set this to false if you don't want units to be able to move diagonally
    private bool diagonallyWalkable = false;

    // Saves a reference to the GameObject that gets placed on this cell
    public GridObject objectInThisGrid = null;
    public bool isOccupied = false;
    public int fCost, gCost, hCost;
    public GridCell parentCell;
    public List<GridCell> neighbourList;

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
        if (posX - 1 >= 0)
        {
            // Left Node
            neighbourList.Add(gameGrid.GetGridCell(posX - 1, posZ));
            if (diagonallyWalkable)
            {
                // Lower Left Node
                if (posZ - 1 >= 0) neighbourList.Add(gameGrid.GetGridCell(posX - 1, posZ - 1));
                // Upper Left Node
                if (posZ + 1 < gameGrid.height) neighbourList.Add(gameGrid.GetGridCell(posX - 1, posZ + 1));
            }
        }
        if (posX + 1 < gameGrid.width)
        {
            // Right Node
            neighbourList.Add(gameGrid.GetGridCell(posX + 1, posZ));
            if (diagonallyWalkable)
            {
                // Lower Right Node
                if (posZ - 1 >= 0) neighbourList.Add(gameGrid.GetGridCell(posX + 1, posZ - 1));
                // Upper Right Node
                if (posZ + 1 < gameGrid.height) neighbourList.Add(gameGrid.GetGridCell(posX + 1, posZ + 1));
            }
        }
        // Lower Node
        if (posZ - 1 >= 0) neighbourList.Add(gameGrid.GetGridCell(posX, posZ - 1));
        // Upper Node
        if (posZ + 1 < gameGrid.height) neighbourList.Add(gameGrid.GetGridCell(posX, posZ + 1));
    }

    public void ToggleOccupation()
    {
        isOccupied = !isOccupied;
    }
}
