using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Scriptable Objects/PlayerStats")]
public class PlayerStatsSO : ScriptableObject
{
    public float walkSpeed;
    public float runSpeed;
    public float health;
}
