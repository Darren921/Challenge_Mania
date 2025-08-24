using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Stats", menuName = "Scriptable Objects/WeaponStats", order = 2)]

public class WeaponSO : ScriptableObject
{
    public WaitForSeconds coolDownWait { get; private set; }
    [SerializeField] private float timeBetweenAttacks;
    [field: SerializeField] public float chargeUpTime { get; private set; }
    [field: SerializeField,Range(0,1)] public float minChargePercent;
    [field: SerializeField] public int maxAmmo { get; private set; }

    [field: SerializeField] public float jamRate  { get; private set; }

    [field: SerializeField] public int magSize { get; private set; }
    [field: SerializeField] public bool isFullAuto { get; private set; }
    [field: SerializeField] public float reloadTime { get; private set; }

    [field: SerializeField] public float attackRange { get; private set; }

    [field: SerializeField] public float damage { get; private set; }


    private void OnEnable()
    {
        coolDownWait = new WaitForSeconds(timeBetweenAttacks);
    }
}

