using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private LayerMask gridCellLayer;

    private GameGrid gameGrid;

    public TurnManager turnManager;
    public bool canMove { get; set; }

    private void Start()
    {
        gameGrid = GameManager.GetInstance().gameGrid;
    }

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
                        if (!turnManager.currentTeam.teamUnitManagerInst.unitMoving) MoveUnit(hoveringCell, turnManager.currentTeam.teamUnitManagerInst.currentlySelectedUnit);
                    }
                }
            }
        } else gameGrid = GameManager.GetInstance().gameGrid;
    }

    private void SelectUnit(GameUnit unit)
    {
        turnManager.currentTeam.teamUnitManagerInst.currentlySelectedUnit = unit;
        turnManager.currentTeam.teamUnitManagerInst.UpdateMovementGrid();
    }

    private void MoveUnit(GridCell cell, GameUnit unit)
    {
        if (unit != null) turnManager.currentTeam.teamUnitManagerInst.MoveUnit(cell, unit);
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
