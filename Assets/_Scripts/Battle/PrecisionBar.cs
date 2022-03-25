using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrecisionBar : MonoBehaviour
{



    public enum fillAmount { Zero, Uno, Default, Idle }

    private const int _rapportDistMove = 10;
    private const float _bottomOfTheScreen = 4.2f;
    [SerializeField] float _moveSpeed;

    [SerializeField] BattleSystem _battleSystem;

    public Image progress;
    public SpriteRenderer barre;


    public fillAmount _state;
    private Vector2 _targetPos = Vector2.zero;
    private Vector2 _input = Vector2.zero;

    public static PrecisionBar Instance;

    public int barreSplit = 0;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        progress.fillAmount = 0;
        _targetPos = barre.transform.position;
        //Uno();
    }
    void Update()
    {
        if (_battleSystem.powerUsed && _state == fillAmount.Idle)
            _state = fillAmount.Uno;
        /*if (Input.GetKeyDown(KeyCode.E))
        if (Input.GetKeyDown(KeyCode.A))
            ResetFillAmount();
        */
        if (Input.GetKeyDown(KeyCode.Space))
            OnClick();
        if (_state != fillAmount.Idle)
        {
            _targetPos.y = this.transform.position.y - _bottomOfTheScreen;
            if (_state == fillAmount.Zero)
            {
                Zero();
                _input.x = -Time.deltaTime * _moveSpeed * _rapportDistMove;
                Move();
            }
            if (_state == fillAmount.Uno)
            {
                Uno();
                _input.x = Time.deltaTime * _moveSpeed * _rapportDistMove;
                Move();
            }
        }
    }

    public void ResetFillAmount()
    {
        _state = fillAmount.Idle;
        progress.fillAmount = 0;
        _targetPos.x = -32;
        barre.transform.position = _targetPos;
        barre.color = Color.white;
        barreSplit = 0;
    }


    public void Zero()
    {
        _state = fillAmount.Zero;
        progress.fillAmount -= Time.deltaTime * _moveSpeed;
        barre.transform.position = progress.transform.position;
        if (progress.fillAmount <= 0)
        {
            Uno();
        }
    }

    public void Uno()
    {
        _state = fillAmount.Uno;
        progress.fillAmount += Time.deltaTime * _moveSpeed;
        if (progress.fillAmount >= 1)
        {
            Zero();
        }
    }

    public void Move()
    {
        if (_input != Vector2.zero)
        {
            _targetPos.x += _input.x;
            StartCoroutine(TargetMove(_targetPos));
        }
    }
    IEnumerator TargetMove(Vector3 targetPos)
    {

        barre.transform.position = Vector3.MoveTowards(barre.transform.position, targetPos, _moveSpeed * Time.deltaTime);
        yield return null;

        barre.transform.position = targetPos;
    }


    public void OnClick()
    {
        _state = fillAmount.Default;
        if (progress.fillAmount <= 0.2f || progress.fillAmount >= 0.8f)
        {
            barre.color = Color.red;
            barreSplit = 1;
        }
        else if (progress.fillAmount > 0.2f && progress.fillAmount <= 0.45f || progress.fillAmount > 0.55f && progress.fillAmount <= 0.8f)
        {
            barre.color = Color.yellow;
            barreSplit = 2;
        }
        else if (progress.fillAmount > 0.45f && progress.fillAmount <= 0.55f)
        {
            barre.color = Color.green;
            barreSplit = 3;
        }
        Debug.Log(progress.fillAmount);
        StartCoroutine(_battleSystem.PlayerMove());
    }



}
