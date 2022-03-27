using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interface : MonoBehaviour
{

    private void OnMouseEnter()
    {
        AudioManager.Instance.PlaySFXSound("snd_interface");
    }

    

}
