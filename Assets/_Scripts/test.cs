using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class test : MonoBehaviour
{
    private const int _rapportDistMove = 15;
    private const float _bottomOfTheScreen = 4.2f;

    public Image progress;
    public SpriteRenderer gameObject;

    [SerializeField] private float _moveSpeed;

    public enum fillAmount { Zero, Uno }
    private fillAmount _state;

    private Vector2 targetPos;
    private Vector2 _input;

    private void Start()
    {
        progress.fillAmount = 0;
        Uno();
        targetPos = gameObject.transform.position;
    }
    void Update()
    {

        targetPos.y = this.transform.position.y - _bottomOfTheScreen;
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

    public void Zero()
    {
        _state = fillAmount.Zero;
        progress.fillAmount -= Time.deltaTime * _moveSpeed;
        gameObject.transform.position = progress.transform.position;
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
            targetPos.x += _input.x;
            StartCoroutine(Move(targetPos));
        }
    }
    IEnumerator Move(Vector3 targetPos)
    {

        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPos, _moveSpeed * Time.deltaTime);
        yield return null;

        gameObject.transform.position = targetPos;
    }
}
