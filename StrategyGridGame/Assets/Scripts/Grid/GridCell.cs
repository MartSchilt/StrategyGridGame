using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    private int posX, posY;

    // Saves a reference to the GameObject that gets placed on this cell
    public GridObject objectInThisGrid = null;

    public bool isOccupied = false;

    public void SetPosition(Vector2Int pos)
    {
        posX = pos.x;
        posY = pos.y;
    }

    public Vector2Int GetPosition()
    {
        return new Vector2Int(posX, posY);
    }

    /// <summary>
    /// Logic when clicked on specific cell
    /// </summary>
    /// Could be changed to use children for certain cells
    public void ToggleOccupation()
    {
        isOccupied = !isOccupied;
    }
}
