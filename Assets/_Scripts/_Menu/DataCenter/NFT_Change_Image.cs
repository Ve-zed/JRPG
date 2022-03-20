using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NFT_Change_Image : MonoBehaviour
{
    [SerializeField] Sprite _nFT_Sprite_Lock;
    [SerializeField] Sprite _nFT_Sprite_Unlock;
    [SerializeField] Sprite _nFT_Sprite_Selected;
    
    [SerializeField] Image _nFT_Sprite;

    public void OnClickNFT()
    {
        _nFT_Sprite.sprite = _nFT_Sprite_Selected;
    }
}