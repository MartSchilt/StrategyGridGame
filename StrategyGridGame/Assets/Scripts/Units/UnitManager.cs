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

    public void moveUnit(GridCell cell, GameUnit unit)
    {
        // First calculate the path for the given unit
        Vector3 gridPos = cell.transform.position;
        unit.MoveTo(gridPos, () => {/* Empty Action for now */});

        // Then change occupation of the gridcells
        if (unit.currentGridPos)
        {
            unit.previousGridPos = unit.currentGridPos;
            unit.previousGridPos.objectInThisGrid = null;
            unit.previousGridPos.ToggleOccupation();
        }

        unit.currentGridPos = cell;
        cell.ToggleOccupation();
        cell.objectInThisGrid = unit;
    }

    public void moveUnit(Vector3 worldPos, GameUnit unit)
    {
        unit.MoveTo(worldPos, () => {/* Empty Action for now */});
    }
}
