using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameGrid gridPrefab;
    public GameGrid gridInst { get; private set; }

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
        gridInst = Instantiate(gridPrefab);
        gridInst.OnAwake();

        //Assign the current game grid to the inputManager and let the user perform actions
        inputManager.SetGameGrid(gridInst);
        inputManager.canMove = true; // This is set but never used?
    }
}
