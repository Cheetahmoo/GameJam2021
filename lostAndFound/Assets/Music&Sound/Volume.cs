using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Volume : MonoBehaviour
{
    public AudioMixer musicMix;
    public AudioMixer soundMix;
    private static float master = 0.3981f;
    private static float music = 0.3981f;
    private static float sound = 0.3981f;
    public void SetMaster(float volume)
    {
        master = volume / 100 + 0.0001f;
        SetVolume();

    }
    public void SetMusic(float volume)
    {
        music = volume / 100 + 0.0001f;
        SetVolume();
    }
    public void SetSound(float volume)
    {
        sound = volume / 100 + 0.0001f;
        SetVolume();
    }
    private void SetVolume()
    {
        Debug.Log(master + " " + music + " " + (Mathf.Log10(master * music) * 25 + 20));
        musicMix.SetFloat("MusicVolume", Mathf.Log10(master * music) * 25 + 20);
        soundMix.SetFloat("SoundVolume", Mathf.Log10(master * sound) * 25 + 20);
    }
}
