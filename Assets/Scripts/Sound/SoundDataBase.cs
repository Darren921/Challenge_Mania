using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "SoundDataBase", menuName = "Scriptable Objects/SoundDataBase")]
public class SoundDataBase : ScriptableObject
{
    
    public List<SoundData> Sounds = new List<SoundData>();
    public List<string> SceneList = new List<string>();
    public FMODRefs refs;

private void GetSceneList()
    {
        Sounds.Clear();
        SceneList.Clear();
        for (var i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            var sceneIndex = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
            if (!SceneList.Contains(sceneIndex))
            {
                SceneList.Add(sceneIndex);

            }
        }

   

    }

    private void OnValidate()
    {
    }
    
    
}

[Serializable]
public class SoundData
{
    public enum  SoundType
    {
        Music, 
        SFX,
        Interface
    }
    public EventReference SoundEvents;
    public SoundType Type;
    public string SoundName; 
    public SoundData( SoundType type, EventReference soundEvents ,string soundName)
    {
        Type = type;
        SoundName = soundName;
        SoundEvents =  soundEvents;
    }
}
