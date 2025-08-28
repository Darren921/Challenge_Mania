using System;
using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Knife : WeaponBase
{
   [SerializeField] private BoxCollider2D hitbox;
   private bool isAttacking;

   protected override void Awake()
    {
        base.Awake();
        hitbox.enabled = false;
    }
    protected override void Attack(float percent)
    {
        if (isAttacking) return;
        isAttacking = true;
        StartCoroutine(Stab());

    }

    private IEnumerator Stab()
    {
        hitbox.enabled = true;
        yield return new WaitForSeconds(1.5f);
        hitbox.enabled = false;    
        isAttacking = false;

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        print(other.gameObject.name);
        if (other.gameObject.CompareTag("Player"))
        {  
            _enemy._rb2D.linearVelocity = Vector2.zero;
            _enemy._rb2D.angularVelocity = 0;
            player = other.gameObject.GetComponent<PlayerController>();
            player.TakeDamage(Damage);
        }    }


}
