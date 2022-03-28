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

    //NFT count and trading
    public List<GameObject> NFTCount;
    public List<GameObject> NFTTrading;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            print("clic");
            Vector3 world = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D Click = Physics2D.Raycast(world, Vector2.zero);
            
            if (Click.collider != null)
            {
                print("collider touché");
                _nFTSprite.sprite = _nFTSpriteSelected;
                _nFTModifTextName.text = _nFTTextName.text;
                _nFTModifTextDesc.text = _nFTTextDesc.text;
                for (int i = 0; i <= NFTTrading.Count - 1; i++)
                {
                    NFTTrading[i].SetActive(true);
                }
            }
            /*else
            {
                _nFTSprite.sprite = _nFTSpriteUnlock;
                for (int i = 0; i < NFTTrading.Count; i++)
                {
                    NFTTrading[i].SetActive(false);
                }
            }*/
        }
    }
}