using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private LayerMask gridCellLayer;

    private GameGrid gameGrid;

    //For now, a reference to the unitmanager to get the current unit. should probably be a reference to a TeamManager instead.
    public UnitManager unitManager;
    public bool canMove { get; set; }

    void Update()
    {
        if (gameGrid != null)
        {
            // Left mouse click
            if (Input.GetMouseButtonDown(0))
            {
                GridCell hoveringCell = IsMouseOverAGridSpace();

                if (hoveringCell)
                {
                    // Selecting units to move
                    if (hoveringCell.objectInThisGrid is GameUnit) SelectUnit((GameUnit)hoveringCell.objectInThisGrid);
                }
            }

            // Right mouse click
            if (Input.GetMouseButtonDown(1))
            {
                GridCell hoveringCell = IsMouseOverAGridSpace();
                if (hoveringCell)
                {
                    if (!hoveringCell.isOccupied)
                    {
                        if (!unitManager.unitMoving) MoveUnit(hoveringCell, unitManager.currentlySelectedUnit);
                    }
                }
            }
        }
    }

    public void SetGameGrid(GameGrid grid)
    {
        if (gameGrid == null) gameGrid = grid;
    }

    private void SelectUnit(GameUnit unit)
    {
        unitManager.currentlySelectedUnit = unit;
        unitManager.UpdateMovementGrid();
    }

    private void MoveUnit(GridCell cell, GameUnit unit)
    {
        if (unit != null) unitManager.MoveUnit(cell, unit);
    }

    private GridCell IsMouseOverAGridSpace()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // Increase the maxDistance if necessary
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 200f, gridCellLayer)) 
            return hitInfo.transform.GetComponent<GridCell>();
        
        return null;
    }
}
