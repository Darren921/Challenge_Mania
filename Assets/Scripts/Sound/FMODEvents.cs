using System;
using FMODUnity;
using UnityEngine;
using UnityEngine.LightTransport;

public class FMODEvents : MonoBehaviour
{
   [SerializeField] private FMODRefs _refs;

   [field: Header("Music")] 
   [field: SerializeField] internal EventReference _Music;

   [field: Header("Weapons")] 
   [field: SerializeField] internal EventReference Gunshots;

   public static FMODEvents Instance { get; private set; }


   private void Awake()
   {
       
   }
   private void Start()
   {
       if (Instance == null)
       {
           Instance = this;
       }
       else
       {
           Destroy(gameObject);
       }
   }
}
