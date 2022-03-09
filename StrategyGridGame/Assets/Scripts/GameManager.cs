using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public GameGrid gameGrid { get; private set; }
    public InputManager inputManager;
    public GridCell gridCellPrefab;
    public Transform cellHolder;

    [SerializeField] private TurnManager gameTurnManagerPrefab;
    private TurnManager turnManagerInst;

    public static GameManager GetInstance()
    {
        if (!_instance)
            Debug.LogError("GameManager is null");

        return _instance;
    }

    private void Awake()
    {
        _instance = this;
        InitializeGame();
    }

    private void InitializeGame()
    {
        gameGrid = new GameGrid(10, 10, 11f, gridCellPrefab, cellHolder);

        turnManagerInst = Instantiate(gameTurnManagerPrefab);
        turnManagerInst.InitializeTeams();

        // Let the user be able to perform actions
        inputManager.canMove = true;
    }
}
