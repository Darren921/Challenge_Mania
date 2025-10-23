using System;
using System.Collections.Generic;
using System.IO;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UIElements;


public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer instance { get; private set; }
    private FMODEvents  _fmEvents;
    private EventInstance musicEventRef;



    private Dictionary<string, float> MusicIndex = new Dictionary<string, float>()
    {
        {"ChallengeSelection",0},
        {"TitleScreen", 0},
        {"Main", 1},
    };

    private void Awake()
    {
      
    }
   
    private List<EventInstance> _tempEventInstances = new List<EventInstance>();
    private List<EventInstance> _permEventInstances = new List<EventInstance>();

    private void Start()
    {
        _fmEvents = GetComponent<FMODEvents>();
        SceneManager.activeSceneChanged += OnSceneChanged;
        if (instance == null)
        {
            instance = this;
            PlayMusic(_fmEvents._Music);
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void OnSceneChanged(Scene lastScene , Scene nextScene)
    {
        musicEventRef.setParameterByName("MusicChosen",MusicIndex[nextScene.name] );
    }

    private void PlayMusic(EventReference musicEventReference)
    {
        musicEventRef = CreatePermInstance(musicEventReference) ;
        musicEventRef.start();
    }

    public EventInstance CreateTempInstance(EventReference eventRef)
    {
        var eventInstance = RuntimeManager.CreateInstance(eventRef);
        _tempEventInstances.Add(eventInstance);
        return eventInstance;
    }
    public EventInstance CreatePermInstance(EventReference eventRef)
    {
        var eventInstance = RuntimeManager.CreateInstance(eventRef);
        _permEventInstances.Add(eventInstance);
        return eventInstance;
    }

    public void PlayOneShot(EventReference sound, Vector3 position )
    {
        RuntimeManager.PlayOneShot(sound, position);
    }

    private void CleanUp()
    {
        foreach (var eventInstance in _tempEventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
    }

    private void OnDestroy()
    {
        CleanUp();
    }
}
