using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    PlayerController player;

    [SerializeField] Slider healthSlider;
    [SerializeField] Slider SprintSlider;

    private void Start()
    {
        player = GetComponent<PlayerController>();
        player.playerHitAction += UpdateHealth;
        healthSlider.maxValue = player._playerStatsSO.health + GameModifiers.playerHealthModifer * 10;
        healthSlider.value = player.Health;
        SprintSlider.maxValue = player.MaxSprintTimer;
    }

    private void Update()
    {
        SprintSlider.value = player.SprintTimer;
    }


    private void UpdateHealth()
    {
        healthSlider.value = player.Health;
    }
}