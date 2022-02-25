using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 15;

    private static Pathfinding _instance;

    public GameGrid grid { get; private set; }
    private List<GridCell> openList;
    private HashSet<GridCell> closedList; //Changed to HashSet for better optimization

    public Pathfinding(GameGrid gameGrid)
    {
        _instance = this;
        grid = gameGrid;
    }

    public static Pathfinding GetInstance()
    {
        if (_instance == null)
            Debug.LogError("Pathfinding is null");
        return _instance;
    }

    public List<GridCell> FindPath(int startX, int startZ, int endX, int endZ)
    {
        GridCell startCell = grid.GetGridCell(startX, startZ);
        GridCell endCell = grid.GetGridCell(endX, endZ);
        return FindPath(startCell, endCell);
    }

    public List<GridCell> FindPath(GridCell startCell, GridCell endCell)
    {
        if (startCell == null || endCell == null)
        {
            Debug.LogError("StartCell or EndCell is null for pathfinding");
            return null;
        }

        openList = new List<GridCell> { startCell };
        closedList = new HashSet<GridCell>();

        grid.SetCellGCosts();

        startCell.gCost = 0;
        startCell.hCost = CalculateDistanceCost(startCell, endCell);
        startCell.CalculateFCost();

        while (openList.Count > 0)
        {
            GridCell currentCell = GetLowestFCostNode(openList);
            if (currentCell == endCell) return CalculatePath(endCell);

            openList.Remove(currentCell);
            closedList.Add(currentCell);

            foreach (GridCell neighbourCell in currentCell.neighbourList)
            {
                // Skip a cell if it is occupied
                if (closedList.Contains(neighbourCell)) continue;
                if (neighbourCell.isOccupied)
                {
                    closedList.Add(neighbourCell);
                    continue;
                }

                int tentativeGCost = currentCell.gCost + CalculateDistanceCost(currentCell, neighbourCell);
                if (tentativeGCost < neighbourCell.gCost)
                {
                    neighbourCell.parentCell = currentCell;
                    neighbourCell.gCost = tentativeGCost;
                    neighbourCell.hCost = CalculateDistanceCost(neighbourCell, endCell);
                    neighbourCell.CalculateFCost();

                    if (!openList.Contains(neighbourCell)) openList.Add(neighbourCell);
                }
            }
        }

        Debug.Log("No path found to the given endCell");
        return null;
    }

    // Added this override function for ease of use with world coords instead of cells
    public List<Vector3> FindPath(Vector3 startPos, Vector3 endPos)
    {
        GridCell startCell = grid.GetGridCellFromWorldPos(startPos);
        GridCell endCell = grid.GetGridCellFromWorldPos(endPos);

        List<GridCell> path = FindPath(startCell, endCell);

        if (path == null) return null;

        List<Vector3> vectorPath = new List<Vector3>();
        foreach (GridCell cell in path)
        {
            // Just do some magical conversion
            vectorPath.Add(new Vector3(cell.GetPosition().x, 0, cell.GetPosition().y) * grid.gridSpaceSize);
        }
        return vectorPath;
    }

    private int CalculateDistanceCost(GridCell a, GridCell b)
    {
        int xDistance = Mathf.Abs(a.GetPosition().x - b.GetPosition().x);
        int zDistance = Mathf.Abs(a.GetPosition().y - b.GetPosition().y);
        int remaining = Mathf.Abs(xDistance - zDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private GridCell GetLowestFCostNode(List<GridCell> gridCellList)
    {
        GridCell lowestFCostCell = gridCellList[0];
        foreach (GridCell cell in gridCellList) if (cell.fCost <= lowestFCostCell.fCost) lowestFCostCell = cell;

        return lowestFCostCell;
    }

    private List<GridCell> CalculatePath(GridCell endCell)
    {
        List<GridCell> path = new List<GridCell> { endCell };
        GridCell currentCell = endCell;
        while (currentCell.parentCell != null)
        {
            path.Add(currentCell.parentCell);
            currentCell = currentCell.parentCell;
        }
        path.Reverse();
        return path;
    }

    public bool PathInRange(Vector3 startPos, Vector3 endPos, int movementRange)
    {
        List<Vector3> path = FindPath(startPos, endPos);
        return (path.Count <= movementRange);
    }
}
