using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public Slider volumeSlider;

    void Start()
    {
        if (!PlayerPrefs.HasKey("soundVolume"))
        {
            PlayerPrefs.SetFloat("soundVolume", 1);
        }

        Load();
    }

    public void ChangeVolume()
    {
        //AudioListener.volume = volumeSlider.value;
        Save();
    }
    
    private void Save()
    {
        PlayerPrefs.SetFloat("soundVolume", volumeSlider.value);
    }
    
    private void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("soundVolume");
    }
}