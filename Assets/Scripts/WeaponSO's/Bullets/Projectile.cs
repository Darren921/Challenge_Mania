using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float shootForce;
    private Rigidbody2D rb;
    private Camera cam;
    private Vector3 mousePos;
    private Vector2 FireDirection;
    internal float damage;
    internal bool isPlayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }

    public void Init(float chargePercent)
    {
        if (isPlayer)
        {
            mousePos = cam.ScreenToWorldPoint(FindAnyObjectByType<PlayerController>().mousePos);
            var direction = mousePos - transform.position;
            var rotation = transform.position - mousePos;
            var rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rot);
            FireDirection = new Vector2(direction.x, direction.y).normalized;
        }
        else
        {
            var player = FindFirstObjectByType<PlayerController>();
            var direction = player.transform.position - transform.position;
            var rotation = transform.position - player.transform.position;
            var rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rot);
            FireDirection = new Vector2(direction.x, direction.y).normalized;
        }

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.AddForce(shootForce * chargePercent * FireDirection, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        rb.linearVelocity = Vector2.zero;
        print(collision.gameObject.name);
        if (collision.gameObject.CompareTag("Wall"))
        {
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            if (!isPlayer)
            {
                collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
            }

            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }
    }
}