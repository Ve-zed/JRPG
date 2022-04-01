using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextBTC: MonoBehaviour
{
    [SerializeField] GameController _GameController;
    [SerializeField] TextMeshProUGUI _textNumberBTC;

    private void Update()
    {
        _textNumberBTC.text = (_GameController.mHR*1000).ToString();
        _textNumberBTC.text += " BTC";
    }
}