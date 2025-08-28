using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMelee : Enemy
{
    protected override void Pursuit()
    {
        target = player.transform.position;
        agent.SetDestination(target);
    }

    protected override void Attack()
    {
        target = player.transform.position;
        curWeapon.startAttacking();
        agent.SetDestination(target);
        
    }

    public override void TakeDamage(float damage)
    {
        _rb2D.linearVelocity = Vector3.zero;
        _health -= damage;
        if (_health <= 0)
        {
            EnemySpawner.OnDeath?.Invoke();
           ObjectPoolManager.ReturnObjectToPool(gameObject);
        }
    }
}
