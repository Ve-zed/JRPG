using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NFT_Sell : MonoBehaviour
{
    [SerializeField] GameController _GameController;
    public GameObject nFTSelected;
    private NFT_Properties _NFT_Properties;

    public void OnClickSell()
    {
        if (nFTSelected != null)
        {
            _NFT_Properties = nFTSelected.GetComponent<NFT_Properties>();
            if (_NFT_Properties.nFTBuy == true)
            {
                _NFT_Properties.nFTBuy = false;
                _GameController.mHR += _NFT_Properties.nFTPrice;
                Debug.Log("Selling");
            }
            else
                Debug.Log("Non activé.");
        }
    }
}
