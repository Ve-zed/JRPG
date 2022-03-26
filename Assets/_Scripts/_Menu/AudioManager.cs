using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class AudioManager : MonoBehaviour
{
    public AudioMixer mixer;
    public AudioSource audioSourceMusic;
    public AudioSource audioSourceSFX;

    public AudioClip[] audioClips;

    [HideInInspector] public bool coroutine = false;
    //public float step = 1f;

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

    private void Update()
    {
    }
    public void SetMusicLevel(float sliderValue)
    {
        m_sliderMusicValue = sliderValue;
        mixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
        //Debug.Log(sliderValue);
    }
    public void SetSFXLevel(float sliderValue)
    {
        m_sliderSfxValue = sliderValue;
        mixer.SetFloat("SFXVolume", Mathf.Log10(sliderValue) * 20);

        //Debug.Log(sliderValue);
    }
    public void PlayMusicSound(string name)
    {
        AudioClip clip = GetClip(name);
        audioSourceMusic.PlayOneShot(clip);
        Debug.Log(clip);
    }
    public void PlaySFXSound(string name)
    {
        AudioClip clip = GetClip(name);
        audioSourceSFX.PlayOneShot(clip);
    }

    public IEnumerator IEPlayMusicSound(string name)
    {
        AudioClip clip = GetClip(name);
        audioSourceMusic.PlayOneShot(clip);
        yield return new WaitForSeconds(clip.length);
        StartCoroutine(IEPlayMusicSound(name));
    }
    //public IEnumerator step(string name)
    //{
    //    AudioClip clip = GetClip(name);
    //    audioSourceMusic.PlayOneShot(clip);
    //    yield return new WaitForSeconds(step);
    //}

    AudioClip GetClip(string name)
    {
        foreach (var item in audioClips)
        {
            if (item.name == name)
                return item;
        }
        return null;
    }


}
