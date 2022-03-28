using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeOrNot : MonoBehaviour
{
    public GameObject enableOrDisableGameObject;

    public bool seeTrigger = false;

    public IEnumerator enableOrDisableObject()
    {
        yield return new WaitForSeconds(0.1f);
        if (!seeTrigger)
            enableOrDisableGameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(enableOrDisableObject());
            seeTrigger = false;
        }
    }

    private void OnMouseOver()
    {
        seeTrigger = true;

    }
    private void OnMouseExit()
    {
        seeTrigger = false;
            StartCoroutine(enableOrDisableObject());
    }
}
