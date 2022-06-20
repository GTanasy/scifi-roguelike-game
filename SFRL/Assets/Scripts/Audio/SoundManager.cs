using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public Sound[] sounds;

    //public static SoundManager instance;

    public Slider _volumeSlider;

    void Awake()
    {
        //if (instance == null)
        //{
        //    instance = this;
        //}
        //else
        //{
        //    Destroy(gameObject);
        //}
        //DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
        _volumeSlider = GameObject.Find("UI/PauseMenu/VolumeSlider").GetComponent<Slider>();
    }

    void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            Load();
        }

        else
        {
            Load();
        }
    }

    void Update()
    {
        if (_volumeSlider == null)
        {
            if (GameObject.Find("UI/PauseMenu/VolumeSlider") != null)
            {
                _volumeSlider = GameObject.Find("UI/PauseMenu/VolumeSlider").GetComponent<Slider>();
            }
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }

    public void changeVolume()
    {
        AudioListener.volume = _volumeSlider.value;
        Save();
    }

    private void Load()
    {
        _volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", _volumeSlider.value);
    }
}
