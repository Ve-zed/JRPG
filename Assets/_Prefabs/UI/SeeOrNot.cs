using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeOrNot : MonoBehaviour
{
    public GameObject enableOrDisablegameObject;

    public bool seeTrigger = false;

    public IEnumerator enableOrDisableObject()
    {
        yield return new WaitForSeconds(0.5f);
        if (!seeTrigger)
            enableOrDisablegameObject.SetActive(false);
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
