using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveVelocity
{
    void SetVelocity(Vector3 velocityVector);
    void Disable();
    void Enable();
}

public class MoveTransformVelocity : MonoBehaviour, IMoveVelocity
{
    private float moveSpeed;
    private GameUnit unit;
    private Vector3 velocityVector;

    // Start is called before the first frame update
    void Awake()
    {
        unit = GetComponent<GameUnit>();
        moveSpeed = 20f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += velocityVector * moveSpeed * Time.deltaTime;
        // TODO: Add unit movement animation, should be called here in the unit
    }

    public void Disable()
    {
        this.enabled = false;
    }

    public void Enable()
    {
        this.enabled = true;
    }

    public void SetVelocity(Vector3 velocityVector)
    {
        this.velocityVector = velocityVector;
    }
}
