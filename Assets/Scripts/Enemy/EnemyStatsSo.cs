using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStatsSo", menuName = "Scriptable Objects/EnemyStatsSo")]
public class EnemyStatsSo : ScriptableObject
{
   public float health;
   public float SightRange;
   public float AttackRange;
}
