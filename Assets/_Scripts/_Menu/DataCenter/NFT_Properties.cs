using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NFT_Properties : MonoBehaviour
{
    //Apparence
    [SerializeField] Sprite _nFTSpriteLock;
    [SerializeField] Sprite _nFTSpriteUnlock;
    [SerializeField] Sprite _nFTSpriteSelected;
    [SerializeField] Image _nFTSprite;

    //Texte propre au NFT
    [SerializeField] TextMeshProUGUI _nFTTextName;
    [SerializeField] TextMeshProUGUI _nFTTextDesc;

    //Texte modifié
    [SerializeField] TextMeshProUGUI _nFTModifTextName;
    [SerializeField] TextMeshProUGUI _nFTModifTextDesc;

    //NFT count
    [SerializeField] GameObject _nFTCountTrading;

    //Nombre
    public List<GameObject> NFTCount;

    private bool Selected = false;

    private void Update()
    {
        if (Selected == true)
        {
            _nFTSprite.sprite = _nFTSpriteSelected;
            _nFTModifTextName.text = _nFTTextName.text;
            _nFTModifTextDesc.text = _nFTTextDesc.text;
            _nFTCountTrading.SetActive(true);
        }
    }

    public void OnClickNFT()
    {
        Selected = true;
    }
}