using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public Slider volumeSlider;

    public FMOD.Studio.Bus MasterBus;

    void Start()
    {
        if (!PlayerPrefs.HasKey("soundVolume"))
        {
            PlayerPrefs.SetFloat("soundVolume", 1);
        }

        MasterBus = FMODUnity.RuntimeManager.GetBus("bus:/");

        Load();
    }

    public void ChangeVolume()
    {
        var result = MasterBus.setVolume(volumeSlider.value);
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