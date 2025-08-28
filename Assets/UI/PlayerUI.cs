using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
   PlayerController player;
   
   [SerializeField]Slider healthSlider;
   [SerializeField]Slider SprintSlider;
   
   private void Start()
   {
      player = GetComponent<PlayerController>();
      player.playerHitAction += UpdateHealth;
      healthSlider.maxValue = player._playerStatsSO.health;
      healthSlider.value = player._playerStatsSO.health;
      
      
   }

   private void UpdateHealth()
   {
      healthSlider.value = player.Health;
   }
}
