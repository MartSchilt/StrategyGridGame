using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    [SerializeField] private UnitData[] teamUnitData;
    private GameUnit[] teamUnits;
    private GameGrid gameGrid;

    public GameUnit unitPrefab;

    [SerializeField] private UnitManager teamUnitManagerPrefab;
    public UnitManager teamUnitManagerInst { get; private set; }

    public void SpawnUnits()
    {
        gameGrid = GameManager.GetInstance().gameGrid;
        teamUnits = new GameUnit[teamUnitData.Length];

        teamUnitManagerInst = Instantiate(teamUnitManagerPrefab);

        for (int i = 0; i < teamUnitData.Length; i++)
        {
            var gameUnit = Instantiate(unitPrefab, teamUnitManagerInst.transform);
            gameUnit.thisUnit = teamUnitData[i];
            teamUnits[i] = gameUnit;

            //Place them next to each other, for now
            Vector3 position = new Vector3(i * gameGrid.gridSpaceSize, 0, 0);
            gameUnit.transform.position = position;

            gameUnit.currentGridPos = gameGrid.GetGridCellFromWorldPos(position);
            gameUnit.currentGridPos.ToggleOccupation();
            gameUnit.currentGridPos.objectInThisGrid = gameUnit;
        }

        teamUnitManagerInst.SetUnits(teamUnits);
    }
}
