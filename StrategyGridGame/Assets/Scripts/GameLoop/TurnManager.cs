using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    [SerializeField] private TeamManager[] teams;
    private int totalTurnAmount;
    private int currentTurn;

    public TeamManager currentTeam { get; private set; }

    private void Awake()
    {
        if (teams.Length != 0)
        {
            totalTurnAmount = teams.Length;
            currentTurn = 0;
            currentTeam = teams[currentTurn];

            GameManager.GetInstance().inputManager.turnManager = this;
        }
    }

    public void InitializeTeams()
    {
        foreach (TeamManager team in teams)
        {
            team.SpawnUnits();
        }
    }

    private void PassTurn()
    {
        currentTurn++;
        if (currentTurn > totalTurnAmount) currentTurn = 0;

        currentTeam = teams[currentTurn];
    }

}
