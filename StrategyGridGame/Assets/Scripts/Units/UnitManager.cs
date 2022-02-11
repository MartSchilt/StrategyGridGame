using UnityEngine;

public class UnitManager : MonoBehaviour
{
    [SerializeField] private UnitData[] existingUnits;

    private GameUnit[] gameUnits;
    public GameUnit unitPrefab;

    public GameUnit currentlySelectedUnit;

    [SerializeField] private Transform UnitHolder;

    void Awake()
    {
        InstantiateUnits();

        //Set selected unit as the first for now
        currentlySelectedUnit = gameUnits[0];
    }

    private void InstantiateUnits()
    {
        gameUnits = new GameUnit[existingUnits.Length];
        for (int i = 0; i < existingUnits.Length; i++)
        {
            var gameUnit = Instantiate(unitPrefab, UnitHolder);
            gameUnit.thisUnit = existingUnits[i];
            gameUnits[i] = gameUnit;

            //Idk, place them in the sky for now lol
            gameUnit.transform.position = new Vector3(0, 10000, 0);
        }
    }

    public void moveUnit(GridCell cell, GameUnit unit)
    {
        if (unit.currentGridPos) unit.previousGridPos = unit.currentGridPos;

        unit.currentGridPos = cell;
        cell.ToggleOccupation();
        cell.objectInThisGrid = unit;

        Vector3 gridPos = cell.transform.position;

        //Magic number for now, should be relative to height of terrain mesh
        unit.transform.position = gridPos + new Vector3(0, 10, 0);
        return;
    }
}
