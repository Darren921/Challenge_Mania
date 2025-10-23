using System;
using System.Collections.Generic;
using FMOD;
using FMODUnity;
using UnityEngine;


[Serializable]
public class SimpleStringEventRefs
{
    public string name;
    public EventReference eventRef;
}


[CreateAssetMenu(fileName = "FMODRefs", menuName = "Scriptable Objects/FMODRefs")]
public class FMODRefs : ScriptableObject
{
    
    public List<SimpleStringEventRefs> persistEventRefs;
    public  List<SimpleStringEventRefs> sceneEventRefs;
}
