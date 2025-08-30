using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyRanged : Enemy
{
    private bool isAttacking;


    private void OnDisable()
    {
        isAttacking = false;
    }

    protected override void Pursuit()
    {
        target = player.transform.position;
        agent.SetDestination(target);
    }

    protected override void Attack()
    {
        target = playerInLOS ? transform.position : player.transform.position;

        agent.SetDestination(target);

        if (curWeapon.ammoLeft == 0 || curWeapon.isJammed)
        {
            StartCoroutine(curWeapon.GetTryReload());
            isAttacking = false;
        }

        if (!isAttacking && playerInLOS)
        {
            StartCoroutine(Fire());
            curWeapon.startAttacking();
        }

        if (!isAttacking || !playerInSightRange || !playerInLOS)
        {
            isAttacking = false;
            curWeapon.stopAttacking();
        }
    }

    private IEnumerator Fire()
    {
        isAttacking = true;
        yield return new WaitForSeconds(AttackRate);
        isAttacking = false;
    }

    protected override void SetUpType()
    {
        _type = EnemyType.Ranged;
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