using System;
using UnityEngine;

public class AR : WeaponBase
{
    [SerializeField] Projectile normalBullet;
    [SerializeField] Transform firingPostition;

    protected override void Attack(float percent)
    {
        var rb = ObjectPoolManager.SpawnObject(normalBullet.gameObject, firingPostition.position, Quaternion.identity,
            ObjectPoolManager.PoolType.Gameobject);
        MusicPlayer.instance.PlayOneShot(FMODEvents.Instance.Gunshots,transform.position);
        var projectile = rb.GetComponent<Projectile>();
        projectile.isPlayer = player is not null;
        if (projectile.isPlayer)
        {
            projectile.damage = Damage + GameModifiers.playerDamageModifer;
        }
        else
        {
            projectile.damage = (Damage * 0.15f) + GameModifiers.enemyDamageModifer;
        }

        projectile.Init(percent);
    }
}