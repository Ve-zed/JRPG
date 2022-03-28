using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interface : MonoBehaviour
{
    private void OnMouseDown()
    {
        
        AudioManager.Instance.PlaySFXSound("snd_interface");
    }

    

}
