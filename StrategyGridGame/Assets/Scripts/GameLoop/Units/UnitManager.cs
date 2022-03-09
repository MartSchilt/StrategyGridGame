using UnityEngine;

public class UnitManager : MonoBehaviour
{
    private GameUnit[] gameUnits;
    private GameGrid gameGrid;

    public GameUnit currentlySelectedUnit { get; set; }
    public bool unitMoving;

    public void SetUnits(GameUnit[] units)
    {
        gameUnits = units;

        //Set selected unit as the first for now
        currentlySelectedUnit = gameUnits[0];
        UpdateMovementGrid();
    }

    public void MoveUnit(GridCell cell, GameUnit unit)
    {
        // First calculate the path for the given unit
        Vector3 gridPos = cell.transform.position;
        if (gameGrid.pathFinding.PathInRange(gameGrid.GetWorldPosFromGridPos(unit.currentGridPos.GetPosition()), gridPos, unit.thisUnit.movementRange))
        {
            unitMoving = true;

            cell.GetComponentInChildren<MeshRenderer>().material.color = Color.red;

            unit.MoveTo(gridPos, () =>
            {
                // Change occupation of the gridcells after moving the unit
                if (unit.currentGridPos)
                {
                    unit.previousGridPos = unit.currentGridPos;
                    unit.previousGridPos.objectInThisGrid = null;
                    unit.previousGridPos.ToggleOccupation();
                }

                unit.currentGridPos = cell;
                cell.ToggleOccupation();
                cell.objectInThisGrid = unit;

                UpdateMovementGrid();
            });
        }
    }

    // Should be called at the end of units turn
    public void UpdateMovementGrid()
    {
        // Ensure the grid is not null
        gameGrid = GameManager.GetInstance().gameGrid;

        GridCell unitCell = currentlySelectedUnit.currentGridPos;
        Vector3 unitPosition = gameGrid.GetWorldPosFromGridPos(unitCell.GetPosition());
        Vector2Int unitPos = gameGrid.GetGridPosFromWorld(unitPosition);

        GridCell cell;

        for (int x = 0; x < gameGrid.width; x++)
            for (int z = 0; z < gameGrid.height; z++)
            {
                cell = gameGrid.GetGridCell(x, z);
                cell.GetComponentInChildren<MeshRenderer>().material.color = Color.white;
                cell.validMovePosition = false;
            }

        int maxMoveDistance = currentlySelectedUnit.thisUnit.movementRange;
        for (int x = unitPos.x - maxMoveDistance; x <= unitPos.x + maxMoveDistance; x++)
            for (int z = unitPos.y - maxMoveDistance; z <= unitPos.y + maxMoveDistance; z++)
                if (gameGrid.CellExists(x, z))
                {
                    cell = gameGrid.GetGridCell(x, z);
                    if (!cell.isOccupied)
                    {
                        // Check if gridCell is in range of the unit
                        if (gameGrid.pathFinding.FindPath(unitCell, cell).Count <= maxMoveDistance)
                        {
                            cell.GetComponentInChildren<MeshRenderer>().material.color = Color.blue;
                            cell.validMovePosition = true;
                        }
                    }
                }

        // Force change the colour of the tile under the moving unit
        unitCell.validMovePosition = false;
        unitCell.GetComponentInChildren<MeshRenderer>().material.color = Color.green;

        unitMoving = false;
    }
}
