using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NFT_Buy : MonoBehaviour
{
    [SerializeField] GameController _GameController;
    public GameObject nFTSelected;
    private NFT_Properties _NFT_Properties;

    public void OnClickBuy()
    {
        if (nFTSelected != null)
        {
            _NFT_Properties = nFTSelected.GetComponent<NFT_Properties>();
            if (_GameController.mHR >= _NFT_Properties.nFTPrice && _NFT_Properties.nFTBuy == false)
            {
                _NFT_Properties.nFTBuy = true;
                _GameController.mHR -= _NFT_Properties.nFTPrice;
                //nFTSelected
                Debug.Log("Buying");
            }
            else
                Debug.Log("Pas assez de sous ou NFT déjà activé.");
        }
    }
}