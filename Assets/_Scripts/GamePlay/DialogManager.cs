using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public GameObject dialogBoxTuto;
    [SerializeField] Text _dialogTextTuto;
    [SerializeField] GameObject PINJTuto;
    public GameObject dialogBox;
    public event Action onShowDialog;
    public event Action onShowDialogTuto;
    public event Action onCloseDialog;
    public event Action onCloseDialogTuto;

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


    public IEnumerator ShowDialogTuto(Dialog dialog, Action onFinished = null)
    {
        yield return new WaitForEndOfFrame();
        onShowDialogTuto?.Invoke();

        _dialog = dialog;

        _onDialogueFinished = onFinished;

        dialogBoxTuto.SetActive(true);
        StartCoroutine(TypeDialogTuto(dialog.Lines[0]));
    }
    public void HandleUpdateTuto()
    {
        if (Input.GetKeyDown(KeyCode.E) && !_isTyping)
        {
            _currentLine++;
            if (_currentLine < _dialog.Lines.Count)
            {
                StartCoroutine(TypeDialogTuto(_dialog.Lines[_currentLine]));

            }
            else
            {
                _currentLine = 0;
                dialogBox.SetActive(false);
                dialogBoxTuto.SetActive(false);
                _onDialogueFinished?.Invoke();
                PINJTuto.SetActive(false);
                onCloseDialogTuto?.Invoke();
            }
        }
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
                dialogBoxTuto.SetActive(false);
                _onDialogueFinished?.Invoke();
                onCloseDialog?.Invoke();
            }
        }
    }
    public IEnumerator TypeDialog(string line)
    {
        _isTyping = true;
        AudioManager.Instance.PlaySFXSound("snd_dialogue");
        _dialogText.text = "";
        foreach (var letter in line.ToCharArray())
        {
            _dialogText.text += letter;
            yield return new WaitForSeconds(1f / _lettersPerSecond);
        }
        _isTyping = false;
        AudioManager.Instance.audioSourceSFX.Stop();
    }
    public IEnumerator TypeDialogTuto(string line)
    {
        _isTyping = true;
        AudioManager.Instance.PlaySFXSound("snd_dialogue");
        _dialogTextTuto.text = "";
        foreach (var letter in line.ToCharArray())
        {
            _dialogTextTuto.text += letter;
            yield return new WaitForSeconds(1f / _lettersPerSecond);
        }
        _isTyping = false;
        AudioManager.Instance.audioSourceSFX.Stop();
    }
}
