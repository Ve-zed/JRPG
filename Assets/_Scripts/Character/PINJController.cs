using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PINJController : MonoBehaviour, Interactable
{

    [SerializeField] Dialog _dialog;

    public void Interact()
    {
        StartCoroutine(DialogManager.Instance.ShowDialog(_dialog));

        Debug.Log("no tuto");
    }
}
