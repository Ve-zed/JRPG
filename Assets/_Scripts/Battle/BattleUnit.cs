using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleUnit : MonoBehaviour
{

    [SerializeField] MonstersBase _base;
    [SerializeField] int _level;
    [SerializeField] bool isPlayerUnit;


    private Image _image;
    Vector3 _originalPos;
    [SerializeField] Color _originalColor;


    public Monster Monster { get; set; }

    private void Awake()
    {
        _image = GetComponent<Image>();
        _originalPos = _image.transform.localPosition;
    }


    public void Setup()
    {
        Monster = new Monster(_base, _level);

        if (isPlayerUnit)
        {
            _image.sprite = Monster.Base.BackSprite;
        }
        else
        {
            _image.sprite = Monster.Base.FrontSprite;
        }
        PlayEnterAnimation();

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
        sequance.Append(_image.DOColor(_originalColor, 0.1f));
    }
    public void PlayFaintAnimation()
    {
        var sequance = DOTween.Sequence();
        sequance.Append(_image.transform.DOLocalMoveY(_originalPos.y - 150f, 0.5f));
        sequance.Join(_image.DOFade(0f, 0.5f));
    }

}
