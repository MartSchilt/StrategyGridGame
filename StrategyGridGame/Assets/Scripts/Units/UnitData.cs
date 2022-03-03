using UnityEngine;

[CreateAssetMenu(fileName = "New Unit Data", menuName = "Unit Data")]
public class UnitData : ScriptableObject
{
    [Header("Details")]
    [SerializeField] private string _name;

    [Header("Stats")]
    [SerializeField] private int health;
    private int currentHealth;
    [SerializeField] private int attack;
    [SerializeField] private int speed;
    // TODO: change this to private and use it correctly
    [SerializeField] public int movementRange;

    /*[Header("Equippables")]*/
}
