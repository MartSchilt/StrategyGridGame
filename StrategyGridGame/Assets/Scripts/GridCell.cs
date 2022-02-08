using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    private int posX, posY;

    // Saves a reference to the GameObject that gets placed on this cell
    public GameObject objectInThisGrid = null;

    public bool isOccupied = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
    public void Click()
    {
        // Works only for sprites
        // GetComponentInChildren<SpriteRenderer>().material.color = Color.green;
        GetComponentInChildren<MeshRenderer>().material.color = Color.green;
        Debug.Log(isOccupied);
    }
}
