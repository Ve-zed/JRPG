using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public GameObject _dialogBox;
    [SerializeField] Text _dialogText;
    [SerializeField] int _lettersPerSecond;

    Dialog _dialog;
    Action _onDialogueFinished;

    int _currentLine = 0;

    bool _isTyping;
    public event Action OnShowDialog;
    public event Action OnCloseDialog;

    public static DialogManager Instance { get; private set; }


    private void Awake()
    {
        Instance = this;
    }


    public IEnumerator ShowDialog(Dialog dialog, Action onFinished = null)
    {
        yield return new WaitForEndOfFrame();
        OnShowDialog?.Invoke();

        _dialog = dialog;

        _onDialogueFinished = onFinished;

        _dialogBox.SetActive(true);
        StartCoroutine(TypeDialog(dialog.Lines[0]));
    }
    public void HandleUpdate()
    {
        if (Input.GetKeyDown(KeyCode.E) && !_isTyping)
        {
            _currentLine++;
            if (_currentLine < _dialog.Lines.Count)
            {
                StartCoroutine(TypeDialog(_dialog.Lines[_currentLine]));
            }
            else
            {
                _currentLine = 0;
                _dialogBox.SetActive(false);
                _onDialogueFinished?.Invoke();
                OnCloseDialog?.Invoke();
            }

        }
    }
    public IEnumerator TypeDialog(string line)
    {
        _isTyping = true;
        _dialogText.text = "";
        foreach (var letter in line.ToCharArray())
        {
            _dialogText.text += letter;
            yield return new WaitForSeconds(1f / _lettersPerSecond);
        }
        _isTyping = false;
    }
}
