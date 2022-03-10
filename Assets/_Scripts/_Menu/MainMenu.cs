using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject Accueil;
    public GameObject Parameter;
    public GameObject Controls;
    public GameObject Credits;

    private void Awake()
    {
        Accueil.SetActive(true);
        Parameter.SetActive(false);
        Controls.SetActive(false);
        Credits.SetActive(false);
    }
    public void OnClickPlay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void OnClickParameter()
    {
        Accueil.SetActive(false);
        Parameter.SetActive(true);
    }
    public void OnClickCredits()
    {
        Accueil.SetActive(false);
        Credits.SetActive(true);
    }
    public void OnClickControls()
    {
        Controls.SetActive(true);
        Parameter.SetActive(false);
    }
    public void OnClickReturn()
    {
        if (Controls.activeInHierarchy == true)
        {
            Controls.SetActive(!Controls);
            Parameter.SetActive(true);
        }
        else
        {
            Accueil.SetActive(true);
            Credits.SetActive(false);
            Parameter.SetActive(false);
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
