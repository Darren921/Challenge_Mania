using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class MusicPlayer : MonoBehaviour
{
    private static MusicPlayer instance;
    [SerializeField] private AudioClip mainMusic;
    [SerializeField] private AudioClip BattleMusic;

    private AudioSource audioSource;
    
    private void Awake()
    {
        SceneManager.activeSceneChanged += OnSceneChanged;
        if (instance == null)
        {
            instance = this;
            audioSource = GetComponent<AudioSource>();
            PlayMusic(mainMusic);
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneChanged(Scene lastScene , Scene nextScene)
    {
        switch (nextScene.name)
        {
          case "Main":
              PlayMusic(BattleMusic);
              break;
          case "TitleScreen":
              PlayMusic(mainMusic);
              break;
          case "ChallengeSelection":
              PlayMusic(mainMusic);
              break;
          default:
              break;
        }
    }

    private void PlayMusic(AudioClip musicClip)
    {
        if(audioSource == null) return;
        if (audioSource.clip == musicClip) return;
        audioSource.clip = musicClip;
        audioSource.Play();
    }
}
