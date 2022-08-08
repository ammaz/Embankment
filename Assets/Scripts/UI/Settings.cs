using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using BitBenderGames;

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

    public void QuitGame()
    {
        Application.Quit();
    }

    public void EnableOrDisableCamera(bool check)
    {
        GameObject.Find("Main Camera").GetComponent<MobileTouchCamera>().enabled = check;
    }
}
