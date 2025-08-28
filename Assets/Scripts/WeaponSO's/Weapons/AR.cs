using System;
using UnityEngine;

public class AR : WeaponBase
{
    [SerializeField] Projectile normalBullet;
    [SerializeField] Transform firingPostition;
    
    protected override void Attack(float percent)
    {
        var rb = ObjectPoolManager.SpawnObject(normalBullet.gameObject, firingPostition.position, Quaternion.identity, ObjectPoolManager.PoolType.Gameobject);
        var projectile = rb.GetComponent<Projectile>();
        projectile.isPlayer = player is not null;
        if (projectile.isPlayer)
        {
            projectile.damage = Damage;
        }
        else
        {
            projectile.damage = Damage * 0.15f;
            
        }
        projectile.Init(percent);
        Destroy(rb.gameObject,100);
    }
}
