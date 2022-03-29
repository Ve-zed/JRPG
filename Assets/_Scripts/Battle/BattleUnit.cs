using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleUnit : MonoBehaviour
{

    public bool isPlayerUnit;
    public bool isVirus;
    [SerializeField] BattleHud _hud;
    [SerializeField] HPBar _hpBar;

    private BattleState _state;

    [SerializeField] SeeOrNot _seeOrNot;
    [SerializeField] BattleSystem _battleSystem;

    [SerializeField] BattleDialogBox _dialogBox;

    [SerializeField] GameObject _pouvoirBarre;

    public BoxCollider2D boxCollider;
    public bool isAttacking;
    public bool isSelected;
    public bool isPowerUsed = false;

    public Image _image;
    public Material originalMaterial;
    public Material outLineMaterial;
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
            _image.sprite = Monster.Base.PlayerSprite;
        else
            _image.sprite = Monster.Base.EnnemiSprite;
        Monster.HP = Monster.MaxHp;

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
            _image.transform.localPosition = new Vector3(-500f, _originalPos.y);
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
    public IEnumerator PlayHitAnimation()
    {
        var sequance = DOTween.Sequence();

        if (_image.color.a < 1)
        {
            sequance.Append(_image.DOColor(Color.red, 0.1f));
            sequance.Append(_image.DOColor(originalColor, 0.1f));
            yield return new WaitForSeconds(1f);
            PlayFadeAnimation();
        }
        else
        {
            sequance.Append(_image.DOColor(Color.red, 0.1f));
            sequance.Append(_image.DOColor(originalColor, 0.1f));
        }
    }
    public void PlayFadeAnimation()
    {

        var sequance = DOTween.Sequence();

        sequance.Append(_image.DOFade(0.5f, 0.5f));
    }
    public void PlayNormalAnimation()
    {
        var sequance = DOTween.Sequence();

        _image.DOFade(1f, 0.5f);
    }
    public IEnumerator PlayHealAnimation()
    {
        var sequance = DOTween.Sequence();
        if (_image.color.a < 1)
        {
            _image.DOColor(Color.green, 0.1f);
            yield return new WaitForSeconds(0.5f);
            _image.DOColor(originalColor, 0.1f);
            yield return new WaitForSeconds(1f);
            PlayFadeAnimation();

        }
        else
        {
            _image.DOColor(Color.green, 0.1f);
            yield return new WaitForSeconds(0.5f);
            _image.DOColor(originalColor, 0.1f);
        }
    }
    public IEnumerator PlayBoostAnimation()
    {
        if (_image.color.a < 1)
        {
            _image.DOColor(Color.blue, 0.1f);
            yield return new WaitForSeconds(0.5f);
            _image.DOColor(originalColor, 0.1f);
            yield return new WaitForSeconds(1f);
            PlayFadeAnimation();

        }
        else
        {
            _image.DOColor(Color.blue, 0.1f);
            yield return new WaitForSeconds(0.5f);
            _image.DOColor(originalColor, 0.1f);
        }

    }
    public void PlayFaintAnimation()
    {
        var sequance = DOTween.Sequence();
        sequance.Join(_image.DOFade(0f, 0.5f));
        boxCollider.enabled = false;
    }
    public void OnMouseEnter()
    {
        if (!isAttacking && _battleSystem.canSelected && isPlayerUnit)
        {
            _seeOrNot.seeTrigger = true;
            _image.material = outLineMaterial;
        }
        if (_battleSystem.canSelectedEnnemi && !isPlayerUnit)
            _image.material = outLineMaterial;
    }
    public void OnMouseExit()
    {
        if (!isSelected)
            _image.material = originalMaterial;
    }

    private void OnMouseDown()
    {
        if (_battleSystem.canSelectedEnnemi && !isPlayerUnit)
        {
            _battleSystem._targetSelectedUnit = this;
            _battleSystem.canSelectedEnnemi = false;
            _image.material = outLineMaterial;
            if (_battleSystem._targetSelectedUnit.Monster.HP > 0 && !_battleSystem.powerUsed)
                StartCoroutine(_battleSystem.PlayerMove());
            else if (_battleSystem._targetSelectedUnit.Monster.HP > 0 && _battleSystem.powerUsed)
            {
                _pouvoirBarre.SetActive(true);
            }
        }
        else if (!isAttacking && _battleSystem.canSelected && isPlayerUnit)
        {
            if (_battleSystem._playerSelectedUnit != null)
            {
                _battleSystem._playerSelectedUnit._image.material = originalMaterial;
                _battleSystem._playerSelectedUnit.isSelected = false;
            }
            isSelected = true;
            _battleSystem._playerSelectedUnit = this;
            var pos = _battleSystem._playerSelectedUnit.transform.position;
            pos.x += 4f;
            _seeOrNot.transform.position = pos;
            _battleSystem.MoveSelection();
            _dialogBox.EnableMoveSelector(true);
            _image.material = outLineMaterial;
        }

    }



}
