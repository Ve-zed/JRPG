using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NFT_HeroLevelText : MonoBehaviour
{
    [SerializeField] Btn_DataCenter _Btn_DataCenter;
    [SerializeField] TextMeshProUGUI _textLevel;

    private void Update()
    {
        _textLevel.text = "- Level "+ _Btn_DataCenter._Monster._level.ToString() + " -";
    }
}
