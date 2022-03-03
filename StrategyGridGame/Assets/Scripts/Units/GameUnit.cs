using System;
using UnityEngine;

public class GameUnit : GridObject
{
    private MoveManager moveManager;

    public UnitData thisUnit;
    public GridCell currentGridPos;
    public GridCell previousGridPos;

    private void Awake()
    {
        moveManager = GetComponent<MoveManager>();
        moveManager.enabled = true;
    }

    public void MoveTo(Vector3 targetPos, Action onReachedPosition)
    {
        // state = unitstate.moving
        moveManager.SetMovePosition(targetPos, () =>
        {
            // state = unitstate.idle
            onReachedPosition();
        });
    }
}
