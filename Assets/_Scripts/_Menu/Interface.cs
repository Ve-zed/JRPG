using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class Interface : MonoBehaviour
{

    public Image image;
    public TextMeshProUGUI text;
    bool _hidden = false;
    private void OnMouseDown()
    {
        
        AudioManager.Instance.PlaySFXSound("snd_interface");
    }
    public void Hide(float time)
    {
        image.DOKill();
        image.DOFade(0, time);

        text.DOKill();
        text.DOFade(0, time);

        _hidden = true;
    }

}
