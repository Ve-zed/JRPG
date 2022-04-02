using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NFT_LvRequiredText : MonoBehaviour
{
    [SerializeField] NFT_Properties _NFT_Properties;
    [SerializeField] TextMeshProUGUI _textLevel;

    private void Update()
    {
        if (_NFT_Properties.nFTUnlock == false)
            _textLevel.text = "- Nv " + _NFT_Properties.lVRequired.ToString() + " -";
        else
            _textLevel.text = "";
    }
}
