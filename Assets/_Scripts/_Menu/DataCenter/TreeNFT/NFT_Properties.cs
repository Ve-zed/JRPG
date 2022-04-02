using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NFT_Properties : MonoBehaviour
{
    public int nFTID;
    public Sprite nFTSpriteLock;
    public Sprite nFTSpriteUnlock;
    public Sprite nFTSpriteSelected;
    public Image  nFTImage;
    public int lVRequired;
    public TextMeshProUGUI nFTTextName;
    public TextMeshProUGUI nFTTextDesc;
    public bool nFTBuy;
    public int nFTPrice;
    public bool nFTUnlock = true;
}