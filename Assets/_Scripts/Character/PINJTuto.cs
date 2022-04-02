using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PINJTuto : MonoBehaviour, Interactable
{

    [SerializeField] Dialog _dialog;

    private void Start()
    {
        Interact();
    }
    public void Interact()
    {
        StartCoroutine(DialogManager.Instance.ShowDialogTuto(_dialog));

        
    }


}
