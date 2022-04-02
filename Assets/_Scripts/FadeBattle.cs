using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeBattle : MonoBehaviour
{
    public static FadeBattle Instance;

    private void Awake()
    {
        Instance = this;
    }

    public Image imageFadeBattle;

}
