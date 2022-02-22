using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveManager : MonoBehaviour
{
    private List<Vector3> path;
    private int pathIndex = -1;
    private IMoveVelocity component;

    void Awake()
    {
        component = GetComponent<IMoveVelocity>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pathIndex != -1)
        {
            Vector3 nextPos = path[pathIndex];
            Vector3 moveVelocity = (nextPos - transform.position).normalized;
            component.SetVelocity(moveVelocity);

            float reachedPathPositionDistance = 1f;
            if (Vector3.Distance(transform.position, nextPos) < reachedPathPositionDistance) 
                if (pathIndex < path.Count -1) pathIndex++;
        }
        else
        {
            component.SetVelocity(Vector3.zero);
        }
    }

    public void SetMovePosition(Vector3 movePos)
    {
        path = GameManager.GetInstance().gameGrid.pathFinding.FindPath(transform.position, movePos);
        if (path.Count > 0) path.RemoveAt(0);
        if (path.Count > 0) pathIndex = 0;
        else pathIndex = -1;
    }

    public void SetMovePosition(Vector3 movePos, Action onReachedPosition)
    {
        SetMovePosition(movePos);
        onReachedPosition();
    }
}
