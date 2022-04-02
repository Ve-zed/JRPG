using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NFT_Update : MonoBehaviour
{
    [SerializeField] NFT_Properties _NFT_Properties;
    [SerializeField] NFT_Description _NFT_Trading;
    [SerializeField] NFT_Buy _NFT_Buy;
    [SerializeField] NFT_Sell _NFT_Sell;
    public GameObject NFT;
    [SerializeField] Btn_DataCenter _Btn_DataCenter;
    [SerializeField] NFT_Image _NFT_Image;

    private void Update()
    {
        if (_Btn_DataCenter._Monster._level >= _NFT_Properties.lVRequired)
            _NFT_Properties.nFTUnlock = true;
        else
            _NFT_Properties.nFTUnlock = false;

        NFTUpdate(_NFT_Properties, _NFT_Trading);
    }

    public void OnClickNFT()
    {
        if (_NFT_Properties.nFTUnlock == true)
        {
            NFTPropertiesUpdate(_NFT_Properties, _NFT_Trading, _NFT_Image);
        }
        else
            Debug.Log("Niveau trop bas.");
    }

    public void NFTAction()
    {

    }

    private void NFTUpdate(NFT_Properties _NFT_Properties, NFT_Description _NFT_Trading)
    {
        if (_NFT_Trading.nFTID == _NFT_Properties.nFTID)
        {
            _NFT_Properties.nFTImage.sprite = _NFT_Properties.nFTSpriteSelected;
            _NFT_Buy.nFTSelected = NFT;
            _NFT_Sell.nFTSelected = NFT;
        }
        else
        {
            if (_NFT_Properties.nFTUnlock == true)
                _NFT_Properties.nFTImage.sprite = _NFT_Properties.nFTSpriteUnlock;
            else
                _NFT_Properties.nFTImage.sprite = _NFT_Properties.nFTSpriteLock;
        }
    }

    public void NFTPropertiesUpdate(NFT_Properties _NFT_Properties, NFT_Description _NFT_Trading, NFT_Image _NFT_Image)
    {
        _NFT_Trading.nFTID = _NFT_Properties.nFTID;
        _NFT_Trading.nFTTextName.text = _NFT_Properties.nFTTextName.text;
        _NFT_Trading.nFTTextDesc.text = _NFT_Properties.nFTTextDesc.text;
        _NFT_Trading.nFTSelected.sprite = _NFT_Properties.nFTSpriteUnlock;
        _NFT_Trading.nFTImage.sprite = _NFT_Image._unlock;
        _NFT_Trading.nFTPriceBuy.SetText(_NFT_Properties.nFTPrice.ToString());
        _NFT_Trading.nFTPriceSell.SetText(_NFT_Properties.nFTPrice.ToString());
    }
}
