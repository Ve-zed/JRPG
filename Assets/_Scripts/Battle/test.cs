using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class test : MonoBehaviour
{
    private const int _rapportDistMove = 10;
    private const float _bottomOfTheScreen = 4.2f;
    [SerializeField] private float _moveSpeed;

    public Image Progress;
    public SpriteRenderer Barre;


    public enum fillAmount { Zero, Uno, Default }
    private fillAmount _state;
    private Vector2 _targetPos;
    private Vector2 _input;

    private void Start()
    {
        Progress.fillAmount = 0;
        Uno();
        _targetPos = Barre.transform.position;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            OnClick();

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
        if (_state == fillAmount.Default)
        {
            //_input.x = _input.x;
            //Debug.Log("Default");

        }

    }

    public void Zero()
    {
        _state = fillAmount.Zero;
        Progress.fillAmount -= Time.deltaTime * _moveSpeed;
        Barre.transform.position = Progress.transform.position;
        if (Progress.fillAmount <= 0)
        {
            Uno();
        }
    }

    public void Uno()
    {
        _state = fillAmount.Uno;
        Progress.fillAmount += Time.deltaTime * _moveSpeed;
        if (Progress.fillAmount >= 1)
        {
            Zero();
        }
    }

    public void Move()
    {
        if (_input != Vector2.zero)
        {
            _targetPos.x += _input.x;
            StartCoroutine(Move(_targetPos));
        }
    }
    IEnumerator Move(Vector3 targetPos)
    {

        Barre.transform.position = Vector3.MoveTowards(Barre.transform.position, targetPos, _moveSpeed * Time.deltaTime);
        yield return null;

        Barre.transform.position = targetPos;
    }



    public void OnClick()
    {
        _state = fillAmount.Default;
        Debug.Log(Progress.fillAmount);
        if (Progress.fillAmount <= 0.2f || Progress.fillAmount >= 0.8f)
        {
            Barre.color = Color.red;
            Debug.Log("entre 0 et 0.2 ou entre 0.8 et 1");
        }
        else if (Progress.fillAmount > 0.2f && Progress.fillAmount <= 0.45f || Progress.fillAmount > 0.55f && Progress.fillAmount <= 0.8f)
        {
            Barre.color = Color.yellow;
            Debug.Log("entre 0.2 et 0.45 ou entre 0.55 et 0.8");
        }
        else if (Progress.fillAmount > 0.45f && Progress.fillAmount <= 0.55f)
        {
            Barre.color = Color.green;
            Debug.Log("entre 0.45 et 0.55");
        }
    }



}
