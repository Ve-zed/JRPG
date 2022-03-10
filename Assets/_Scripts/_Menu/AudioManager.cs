using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class AudioManager : MonoBehaviour
{
    public AudioMixer mixer;
    public AudioSource audioSource;

    public AudioClip[] audioClips;
    public AudioClip[] audioMusics;

    public float m_sliderMusicValue = 0.3f;
    public float m_sliderSfxValue = 0.3f;

    public static AudioManager Instance;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        SetMusicLevel(m_sliderMusicValue);
        SetSFXLevel(m_sliderSfxValue);
    }


    public void SetMusicLevel(float sliderValue)
    {
        m_sliderMusicValue = sliderValue;
        mixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);

        Debug.Log(sliderValue);
    }
    public void SetSFXLevel(float sliderValue)
    {
        m_sliderSfxValue = sliderValue;
        mixer.SetFloat("SFXVolume", Mathf.Log10(sliderValue) * 20);

        Debug.Log(sliderValue);
    }
    public void PlaySound(string name)
    {
        AudioClip clip = GetClip(name);
        audioSource.PlayOneShot(clip);
    }

    AudioClip GetClip( string name)
    {
        foreach (var item in audioClips)
        {
            if (item.name == name)
                return item;
        }
        return null;
    }

}
