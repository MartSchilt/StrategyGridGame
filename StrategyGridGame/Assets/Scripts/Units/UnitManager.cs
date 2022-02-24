using System;
using System.Collections;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    [SerializeField] private UnitData[] existingUnits;

    private GameUnit[] gameUnits;
    public GameUnit unitPrefab;
    private GameGrid gameGrid;

    public GameUnit currentlySelectedUnit { get; set; }

    [SerializeField] private Transform UnitHolder;

    void Start()
    {
        StartCoroutine(InstantiateUnits());
    }

    private IEnumerator InstantiateUnits()
    {
        // This should not be coded like this
        yield return new WaitForSeconds(1f); // Need to wait for the grid to spawn in

        gameGrid = GameManager.GetInstance().gameGrid;

        gameUnits = new GameUnit[existingUnits.Length];
        for (int i = 0; i < existingUnits.Length; i++)
        {
            var gameUnit = Instantiate(unitPrefab, UnitHolder);
            gameUnit.thisUnit = existingUnits[i];
            gameUnits[i] = gameUnit;

            //Place them next to each other, for now
            Vector3 position = new Vector3(i * gameGrid.gridSpaceSize, 0, 0);
            // Manually adding 10 to the y so the unit is above the grid and visible
            // Should be changed
            gameUnit.transform.position = position; //+ new Vector3(0, 10, 0); 

            gameUnit.currentGridPos = gameGrid.GetGridCellFromWorldPos(position);
            gameUnit.currentGridPos.ToggleOccupation();
            gameUnit.currentGridPos.objectInThisGrid = gameUnit;
        }

        //Set selected unit as the first for now
        currentlySelectedUnit = gameUnits[0];
        yield return new WaitForSeconds(0.5f); // Need to wait for the grid to spawn in
    }

    public void MoveUnit(GridCell cell, GameUnit unit)
    {
        // First calculate the path for the given unit
        Vector3 gridPos = cell.transform.position;
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
        });
    }

    // Should be called at the end of units turn
    private void UpdateMovementGrid()
    {
        // Ensure the grid is not null
        gameGrid = GameManager.GetInstance().gameGrid;

        GridCell cell = currentlySelectedUnit.currentGridPos;
        Vector3 unitPosition = gameGrid.GetWorldPosFromGridPos(cell.GetPosition());
        int unitX = Mathf.RoundToInt(unitPosition.x);
        int unitZ = Mathf.RoundToInt(unitPosition.z);

        for (int x = 0; x < gameGrid.width; x++)
        {
            for (int z = 0; z < gameGrid.height; z++)
            {
                // TODO: remove sprite 
                cell.validMovePosition = false;
            }
        }

        int maxMoveDistance = currentlySelectedUnit.thisUnit.movementRange;
        for (int x = unitX - maxMoveDistance; x <= unitX + maxMoveDistance; x++)
        {
            for (int z = unitZ - maxMoveDistance; z <= unitZ + maxMoveDistance; z++)
            {
                if (!gameGrid.CellIsOccupied(x, z))
                {
                    // Check if gridCell is in range of the unit
                    if (gameGrid.pathFinding.FindPath(unitX, unitZ, x, z).Count <= maxMoveDistance)
                    {
                        // TODO: add sprite
                        gameGrid.GetGridCell(x, z).validMovePosition = true;
                    } 
                }
            }
        }

    }
}
