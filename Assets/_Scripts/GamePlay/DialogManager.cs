using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public GameObject dialogBox;
    public event Action onShowDialog;
    public event Action onCloseDialog;

    [SerializeField] Text _dialogText;
    [SerializeField] int _lettersPerSecond;

    private Dialog _dialog;
    private Action _onDialogueFinished;
    private int _currentLine = 0;
    private bool _isTyping;

    public static DialogManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }


    public IEnumerator ShowDialog(Dialog dialog, Action onFinished = null)
    {
        yield return new WaitForEndOfFrame();
        onShowDialog?.Invoke();

        _dialog = dialog;

        _onDialogueFinished = onFinished;

        dialogBox.SetActive(true);
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
                dialogBox.SetActive(false);
                _onDialogueFinished?.Invoke();
                onCloseDialog?.Invoke();
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
