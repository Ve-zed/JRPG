using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleUnit : MonoBehaviour
{

    public bool isPlayerUnit;
    [SerializeField] BattleHud _hud;
    [SerializeField] HPBar _hpBar;

    [SerializeField] SeeOrNot _seeOrNot;
    [SerializeField] BattleSystem _battleSystem;

    [SerializeField] BattleDialogBox _dialogBox;


    //[SerializeField] BattleUnit player;

    public bool isAttacking;

    public Image _image;
    Vector3 _originalPos;
    public Color originalColor;

    public bool IsPlayerUnit { get { return isPlayerUnit; } }
    public BattleHud Hud { get { return _hud; } }

    public Monster Monster { get; set; }

    private void Awake()
    {
        _image = GetComponent<Image>();
        _originalPos = _image.transform.localPosition;
    }


    public void Setup(Monster monster)
    {
        Monster = monster;

        if (isPlayerUnit)
        {
            _image.sprite = Monster.Base.BackSprite;
        }
        else
        {
            _image.sprite = Monster.Base.FrontSprite;
        }

        Monster.HP = Monster.MaxHp;

        //Hud.UpdateHP();
        _hpBar.SetHP((float)Monster.HP / Monster.MaxHp);

        _hud.SetData(Monster);


        _image.color = originalColor;
        PlayEnterAnimation();

    }


    public void Clear()
    {
        _hud.gameObject.SetActive(false);
    }
    public void Show()
    {
        _hud.gameObject.SetActive(true);
    }
    public void PlayEnterAnimation()
    {


        if (isPlayerUnit)
        {
            _image.transform.localPosition = new Vector3(-500f, _originalPos.y);
        }
        else
            _image.transform.localPosition = new Vector3(500f, _originalPos.y);

        _image.transform.DOLocalMoveX(_originalPos.x, 1f);
    }

    public void PlayAttackAnimation()
    {
        var sequance = DOTween.Sequence();
        if (isPlayerUnit)
            sequance.Append(_image.transform.DOLocalMoveX(_originalPos.x + 50f, 0.25f));
        else
            sequance.Append(_image.transform.DOLocalMoveX(_originalPos.x - 50f, 0.25f));

        sequance.Append(_image.transform.DOLocalMoveX(_originalPos.x, 0.25f));


    }
    public void PlayHitAnimation()
    {
        var sequance = DOTween.Sequence();

        sequance.Append(_image.DOColor(Color.red, 0.1f));
        sequance.Append(_image.DOColor(originalColor, 0.1f));
    }
    public void PlayFaintAnimation()
    {
        var sequance = DOTween.Sequence();
        sequance.Append(_image.transform.DOLocalMoveY(_originalPos.y - 150f, 0.5f));
        sequance.Join(_image.DOFade(0f, 0.5f));
    }

    public void OnMouseOver()
    {
        if (!isAttacking)
        {
            _battleSystem._playerUnit = this;
            //            _seeOrNot.transform.position = _battleSystem._playerUnit.transform.position;
            var pos = _battleSystem._playerUnit.transform.position;
            pos.x += 4.5f;
            _seeOrNot.transform.position = pos;
            _battleSystem.MoveSelection();
            _dialogBox.EnableMoveSelector(true);
        }
    }
    public void OnMouseExit()
    {
        StartCoroutine(_seeOrNot.enableOrDisableObject());
    }



}
