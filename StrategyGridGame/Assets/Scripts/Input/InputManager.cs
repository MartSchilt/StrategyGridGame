using UnityEngine;

public class InputManager : MonoBehaviour
{
    private GameGrid gameGrid;

    //For now, a reference to the unitmanager to get the current unit. should probably be a reference to a TeamManager instead.
    public UnitManager unitManager;

    [SerializeField] private LayerMask gridCellLayer;

    public bool canMove = false;

    void Update()
    {
        if (gameGrid)
        {
            if (Input.GetMouseButtonDown(0))
            {
                GridCell hoveringCell = IsMouseOverAGridSpace();

                if (hoveringCell)
                {
                    MoveUnit(hoveringCell, unitManager.currentlySelectedUnit);
                }
            }
        }
    }

    public void SetGameGrid(GameGrid grid)
    {
        if (!gameGrid)
        {
            gameGrid = grid;
        }
    }

    private void MoveUnit(GridCell cell, GameUnit unit)
    {
        unitManager.moveUnit(cell, unit);
        unit.previousGridPos?.ToggleOccupation();
    }

    private GridCell IsMouseOverAGridSpace()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // Increase the maxDistance if necessary
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 200f, gridCellLayer))
        {
            return hitInfo.transform.GetComponent<GridCell>();
        }
        
        return null;
    }
}
