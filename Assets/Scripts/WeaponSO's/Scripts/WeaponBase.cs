using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class WeaponBase : MonoBehaviour
{
    [SerializeField] private WeaponSO weaponStats;

    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private TextMeshProUGUI jamText;

    protected PlayerController player;
    protected Enemy _enemy;
    private Coroutine currentFireTimer;
    protected bool isOnCoolDown;
    protected WaitUntil coolDownEnforce;
    private WaitForSeconds ReloadTime;
    private float currentChargeTime;
    internal bool isReloading;
    internal bool isJammed;
    internal int ammoLeft;
    private int maxAmmo;
    private bool hasAmmo = true;
    internal float Damage;
    internal float curSpread;
    internal bool _isMelee;

    protected virtual void Awake()
    {
        ReloadTime = new WaitForSeconds(weaponStats.reloadTime);
        ammoLeft = weaponStats.magSize;
        maxAmmo = weaponStats.maxAmmo;
        Damage = weaponStats.damage;
        _isMelee = weaponStats.isMelee;
        
        player ??= GetComponentInParent<PlayerController>();
        _enemy ??= GetComponentInParent<Enemy>();

        if (!_isMelee)
        {
            UpdateAmmo();
            if (jamText != null && player != null)
            {
                jamText.text = "";
            }
            isJammed = false;
        }
        
        if (player != null)
        {
            player.playerAttackAction += UpdateAmmo;
        }
        
       
        
        coolDownEnforce = new WaitUntil(() => !isOnCoolDown);
    }

    public void startAttacking()
    {
        currentFireTimer = StartCoroutine(ReFireTimer());
    }

    public void stopAttacking()
    {
        if (currentFireTimer != null)
        {
            StopCoroutine(currentFireTimer);
            var percent = currentChargeTime / weaponStats.chargeUpTime;
            if (percent != 0) TryAttack(percent);
        }
    }

    private void jamCheck()
    {
        var jamChance = Random.Range(0f, 1);
        var jamRate = weaponStats.jamRate ;

        if (!(jamChance <= jamRate)) return;
        if (player is not null)
        {
            jamText.text = "Jammed, press \"R\" to attempt fix";
        }
        isJammed = true;
    }


    private IEnumerator CooldownTimer()
    {
        isOnCoolDown = true;
        yield return weaponStats.coolDownWait;
        isOnCoolDown = false;
    }

    private void TryAttack(float percent)
    {

        if (!_isMelee)
        {
            if (ammoLeft > 0)
            {
                jamCheck();
            }
            currentChargeTime = 0;
        
            if (!CanAttack(percent) || isJammed) return;

            if (ammoLeft > 0 && isReloading != true && isJammed != true)
            {
                Attack(percent);
                ammoLeft--;
                if (player is not null)
                {
                    ammoText.text = ammoLeft + " / " + maxAmmo;
                }
            }
            else return;

            if (maxAmmo == 0 && ammoLeft != 0)
            {
                hasAmmo = false;
            }
        }
        else
        {
            Attack(percent);
        }
        StartCoroutine(CooldownTimer());
        if (weaponStats.isFullAuto && percent >= 1) currentFireTimer = StartCoroutine(ReFireTimer());
    }

    private IEnumerator TryReload()
    {
        if (ammoLeft == weaponStats.magSize && isJammed == false || _isMelee) yield break;
        if (isJammed)
        {
            if (player is not null) jamText.text = "jam fixed";

            if (ammoLeft != 0)
            {
                ammoLeft--;
                UpdateAmmo();
            }

            isJammed = false;

            yield return new  WaitForSeconds (0.2f);
            if (player is not null) jamText.text = "";
            yield break;
        }

        if (isReloading) yield break;
        isReloading = true;
        
        yield return ReloadTime;
        
        
        if (hasAmmo && maxAmmo >= 0 && !isJammed && isReloading)
        {
            var missing = weaponStats.magSize - ammoLeft;

            if (maxAmmo >= missing)
            {
                // enough reserve to fully reload
                ammoLeft += missing;
                maxAmmo -= missing;
            }
            else
            {
                // not enough reserve to fill mag
                ammoLeft += maxAmmo;
                maxAmmo = 0;
                hasAmmo = false; // no reserve left
            }
            
        }
        UpdateAmmo();
        isReloading = false;

    }
    
    private IEnumerator ReFireTimer()
    {
        print("waiting for cooldown");
        yield return coolDownEnforce;
        print("Post Cooldown");

        while (currentChargeTime < weaponStats.chargeUpTime)
        {
            currentChargeTime += Time.deltaTime;
            yield return null;
        }

        TryAttack(1);
        yield return null;
    }


    private void UpdateAmmo()
    {
        if (player is not null) ammoText.text = ammoLeft + " / " + maxAmmo;
    }

    private bool CanAttack(float percent)
    {
        return !isOnCoolDown && percent >= weaponStats.minChargePercent;
    }

    protected abstract void Attack(float percent);

    public IEnumerator GetTryReload()
    {
        return TryReload();
    }

 
}