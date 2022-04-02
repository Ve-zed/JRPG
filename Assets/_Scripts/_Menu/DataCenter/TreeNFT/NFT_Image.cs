using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NFT_Image : MonoBehaviour
{
    [SerializeField] Sprite _lock;
    public Sprite _unlock;
    [SerializeField] Image _image;
    [SerializeField] NFT_Properties _NFT_Properties;
    Vector2 ImageNFTPos;

    private void Start()
    {
        ImageNFTPos = new Vector2(_image.transform.position.x, _image.transform.position.y);
    }

    private void Update()
    {
        if (_NFT_Properties.nFTUnlock == true)
        {
            _image.sprite = _unlock;
            _image.transform.position = new Vector2(_NFT_Properties.transform.position.x, _NFT_Properties.transform.position.y);
        }
        else
        {
            _image.sprite = _lock;
            _image.transform.position = ImageNFTPos;
        }
    }
}
