using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManger : MonoBehaviour
{
    
    [SerializeField] Slider volumeSlider;
    void Start()
    {
        if(!PlayerPrefs.HasKey("musicVolime"))
        {
            PlayerPrefs.SetFloat("musicVolume",1);
        }
        else
        {
            Load();
        }
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        Save(); 
    }

    private void Load()
    {
         volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }
}
