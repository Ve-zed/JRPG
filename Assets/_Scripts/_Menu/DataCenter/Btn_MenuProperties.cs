using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Btn_MenuProperties : MonoBehaviour
{
    [SerializeField] Btn_MenuManager _btn_MenuManager;
    [SerializeField] int _id;
    [SerializeField] Sprite _btnSpriteUnselected;
    [SerializeField] Sprite _btnSpriteSelected;
    [SerializeField] Image _btnImage;
    [SerializeField] GameObject _objectMove;
    private bool _selected;

    private void Update()
    {
        BtnUpdate(_btn_MenuManager);
        if (_selected)
        {
            _btnImage.sprite = _btnSpriteSelected;
            _objectMove.transform.position = new Vector2(this.gameObject.transform.position.x, _objectMove.transform.position.y);
        }
        else
        {
            _btnImage.sprite = _btnSpriteUnselected;
        }
    }

    public void OnClickBtn()
    {
        _btn_MenuManager.IdManager = _id;
    }

    private void BtnUpdate(Btn_MenuManager _Btn_MenuManager)
    {
        if (_id == _Btn_MenuManager.IdManager)
        {
            _selected = true;
        }
        else
        {
            _selected = false;
        }
    }
}
