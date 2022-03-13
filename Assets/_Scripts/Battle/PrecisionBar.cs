using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrecisionBar : MonoBehaviour
{
    public enum fillAmount { Zero, Uno, Default }
    
    private const int _rapportDistMove = 10;
    private const float _bottomOfTheScreen = 4.2f;
    [SerializeField] float _moveSpeed;

    public Image progress;
    public SpriteRenderer barre;

    private fillAmount _state;
    private Vector2 _targetPos;
    private Vector2 _input;

    private void Start()
    {
        progress.fillAmount = 0;
        Uno();
        _targetPos = barre.transform.position;
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
            StartCoroutine(Move(_targetPos));
        }
    }
    IEnumerator Move(Vector3 targetPos)
    {

        barre.transform.position = Vector3.MoveTowards(barre.transform.position, targetPos, _moveSpeed * Time.deltaTime);
        yield return null;

        barre.transform.position = targetPos;
    }



    public void OnClick()
    {
        _state = fillAmount.Default;
        Debug.Log(progress.fillAmount);
        if (progress.fillAmount <= 0.2f || progress.fillAmount >= 0.8f)
        {
            barre.color = Color.red;
            Debug.Log("entre 0 et 0.2 ou entre 0.8 et 1");
        }
        else if (progress.fillAmount > 0.2f && progress.fillAmount <= 0.45f || progress.fillAmount > 0.55f && progress.fillAmount <= 0.8f)
        {
            barre.color = Color.yellow;
            Debug.Log("entre 0.2 et 0.45 ou entre 0.55 et 0.8");
        }
        else if (progress.fillAmount > 0.45f && progress.fillAmount <= 0.55f)
        {
            barre.color = Color.green;
            Debug.Log("entre 0.45 et 0.55");
        }
    }



}
