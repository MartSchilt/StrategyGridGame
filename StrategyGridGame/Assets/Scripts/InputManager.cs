using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public GameGrid gameGrid;
    [SerializeField] private LayerMask gridCellLayer;

    // Start is called before the first frame update
    void Start()
    {
        gameGrid = FindObjectOfType<GameGrid>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GridCell cellMouseIsOver = IsMouseOverAGridSpace();
            Debug.Log(cellMouseIsOver);
            if (cellMouseIsOver != null)
            {
                cellMouseIsOver.Click();
            }
        }
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
