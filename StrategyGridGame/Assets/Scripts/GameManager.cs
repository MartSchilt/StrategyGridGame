using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public GameGrid gameGrid { get; private set; }
    public GridCell gridCellPrefab;
    public Transform cellHolder;
    public InputManager inputManager;

    public static GameManager GetInstance()
    {
        if (!_instance)
            Debug.LogError("GameManager is null");

        return _instance;
    }

    private void Awake()
    {
        _instance = this;
        InstantiateGrid();
    }

    private void InstantiateGrid()
    {
        gameGrid = new GameGrid(10, 10, 11f, gridCellPrefab, cellHolder);

        // Assign the current game grid to the inputManager and let the user be able to perform actions
        inputManager.SetGameGrid(gameGrid);
        inputManager.canMove = true;
    }
}
