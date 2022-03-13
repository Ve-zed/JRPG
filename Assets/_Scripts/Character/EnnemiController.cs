using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiController : MonoBehaviour, Interactable
{

    [SerializeField] string _name;
    [SerializeField] Sprite _sprite;
    [SerializeField] SpriteRenderer _ennemi;
    [SerializeField] Dialog _dialog;
    [SerializeField] Dialog _dialogAfterBattle;
    [SerializeField] GameObject _exclamation;
    [SerializeField] GameObject _fov;
    [SerializeField] DialogManager _dialogManager;

    bool _battleLost = false;

    public void Interact()
    {
        if (!_battleLost)
        {
            StartCoroutine(DialogManager.Instance.ShowDialog(_dialog, () =>

            StartCoroutine(StartBattle())

            ));
        }
        else
            StartCoroutine(DialogManager.Instance.ShowDialog(_dialogAfterBattle)); ;
    }

    IEnumerator StartBattle()
    {
        yield return new WaitForSeconds(0f);
        _dialogManager.dialogBox.SetActive(false);
        GameController.Instance.StartEnnemiBattle(this);
    }

    public void BattleLost()
    {
        _battleLost = true;
        _fov.SetActive(false);
    }

    public IEnumerator TriggerEnnemiBattle(PlayerController player)
    {
        _exclamation.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        _exclamation.SetActive(false);

        var pos = player.transform.position;
        pos.y = player.transform.position.y + 1;
        transform.position = pos;
        _ennemi.enabled = true;

        StartCoroutine(DialogManager.Instance.ShowDialog(_dialog, () =>
              {
                  StartCoroutine(StartBattle());
              }));
    }

    public Sprite Sprite { get => _sprite; }
    public string Name { get => _name; }
}
