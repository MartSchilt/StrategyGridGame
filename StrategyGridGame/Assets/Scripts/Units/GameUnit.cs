using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUnit : GridObject
{
    public UnitData thisUnit;

    public GridCell currentGridPos;
    public GridCell previousGridPos;

    private MoveManager moveManager;

    private void Awake()
    {
        moveManager = GetComponent<MoveManager>();
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
