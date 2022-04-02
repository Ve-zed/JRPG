using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NFT_Count : MonoBehaviour
{
    [SerializeField] Sprite countSpriteUnlock;
    [SerializeField] Sprite countSpriteLock;
    [SerializeField] Image countImage;
    [SerializeField] NFT_Properties _NFT_Properties;

    private void Update()
    {
        if (_NFT_Properties.nFTBuy)
            countImage.sprite = countSpriteUnlock;
        else
            countImage.sprite = countSpriteLock;
    }
}
