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

    private void Awake()
    {
        this.rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
   //     mousePos = mousePos = cam.ScreenToWorldPoint(InputManager.GetMousePos());
        Vector3 direction = mousePos - transform.position;
        Vector3 rotation = transform.position - mousePos;
        float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot);
        FireDirection = new Vector2(direction.x, direction.y).normalized;

    }
    public void Init(float chargePercent)
    {
        rb.AddForce(shootForce * chargePercent * FireDirection, ForceMode2D.Impulse);
    }

}


