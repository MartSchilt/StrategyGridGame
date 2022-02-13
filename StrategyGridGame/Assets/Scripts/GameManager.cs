using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject gridCellPrefab;
    public GameGrid gameGrid { get; private set; }

    public InputManager inputManager;

    private static GameManager _instance;

    public static GameManager GetInstance()
    {
        if (_instance == null)
            Debug.LogError("GameManager is null");

        return _instance;
    }

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        InstantiateGrid();
    }

    private void InstantiateGrid()
    {
        gameGrid = new GameGrid(10, 10, gridCellPrefab);

        //Assign the current game grid to the inputManager and let the user perform actions
        inputManager.SetGameGrid(gameGrid);
        inputManager.canMove = true; // This is set but never used?
    }
}
