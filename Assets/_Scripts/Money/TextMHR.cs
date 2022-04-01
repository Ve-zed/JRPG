using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextMHR : MonoBehaviour
{
    [SerializeField] GameController _GameController;
    [SerializeField] TextMeshProUGUI _textNumberMHR;

    private void Update()
    {
        _textNumberMHR.text = _GameController.mHR.ToString();
        _textNumberMHR.text += " MHR";
    }
}