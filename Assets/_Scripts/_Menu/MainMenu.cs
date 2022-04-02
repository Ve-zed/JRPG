using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MainMenu : MonoBehaviour
{
    public Image imageFade;
    public GameObject accueil;
    public GameObject parameter;
    public GameObject controls;
    public GameObject credits;

    public List<Interface> buttons;

    private void Awake()
    {
        StartCoroutine(AudioManager.Instance.IEPlayMusicSound("snd_music_menu"));

        accueil.SetActive(true);
        parameter.SetActive(false);
        controls.SetActive(false);
        credits.SetActive(false);
    }
    private void Update()
    {
       
        
    }
    public void OnClickPlay()
    {
        
        imageFade.DOFade(1, 0.8f).OnComplete(FadePlayComplete);
        for (int i = 1; i < buttons.Count; i++)
        {
            buttons[i].Hide(0.1f);
        }
        AudioManager.Instance.audioSourceMusic.Stop();
        StartCoroutine(AudioManager.Instance.IEPlayMusicSound("snd_music_exploration"));
        StartCoroutine(AudioManager.Instance.IEPlayMusicSound("snd_ambiance_exploration"));

    }

    void FadePlayComplete()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

    public void OnClickParameter()
    {   
        accueil.SetActive(false);
        parameter.SetActive(true);
    }
    public void OnClickCredits()
    {
        accueil.SetActive(false);
        credits.SetActive(true);
    }
    public void OnClickControls()
    {
        controls.SetActive(true);
        parameter.SetActive(false);
    }
    public void OnClickReturn()
    {
        if (controls.activeInHierarchy == true)
        {
            controls.SetActive(!controls);
            parameter.SetActive(true);
        }
        else
        {
            accueil.SetActive(true);
            credits.SetActive(false);
            parameter.SetActive(false);
        }
    }
    public void OnClickLeave()
    {
        //If we are running in a standalone build of the game
#if UNITY_STANDALONE
        //Quit the application
        Application.Quit();
#endif

        //If we are running in the editor
#if UNITY_EDITOR
        //Stop playing the scene
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

}
