using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Settings : MonoBehaviour
{
    //For Volume
    public AudioSource Volume;
    public Slider volumeSlider;
    //For Music
    public AudioSource Music;
    public Slider musicSlider;

    public void SetVolume()
    {
        Volume.volume = volumeSlider.value;
    }

    public void SetMusic()
    {
        Music.volume = musicSlider.value;
    }
}
