using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleDialogBox : MonoBehaviour
{

    [SerializeField] int _lettersPerSecond;
    [SerializeField] Color _highlightedColor;



    [SerializeField] Text _dialogText;
    
    [SerializeField] GameObject _actionSelector;
    [SerializeField] GameObject _moveSelector;
    [SerializeField] GameObject _moveDetails;

    [SerializeField] List<Text> _actionTexts;
    [SerializeField] List<Text> _moveTexts;


    [SerializeField] Text _ppText;
    [SerializeField] Text _typeText;



    public void SetDialog(string dialog)
    {
        _dialogText.text = dialog;
    }

    public IEnumerator TypeDialog(string dialog)
    {
        _dialogText.text = "";
        foreach (var letter in dialog.ToCharArray())
        {
            _dialogText.text += letter;
            yield return new WaitForSeconds(1f / _lettersPerSecond);
        }

        yield return new WaitForSeconds(1f);
    }

    public void EnableDialogText(bool enabled)
    {
        _dialogText.enabled = enabled;
    }
    public void EnableActionSelector(bool enabled)
    {
        _actionSelector.SetActive(enabled);
    }
    public void EnableMoveSelector(bool enabled)
    {
        _moveSelector.SetActive(enabled);
        _moveDetails.SetActive(enabled);
    }

    public void UpdateActionSelection(int selectedAction)
    {
        for (int i = 0; i < _actionTexts.Count; i++)
        {
            if (i == selectedAction)
            {
                _actionTexts[i].color = _highlightedColor;
            }
            else
            {
                _actionTexts[i].color = Color.black;
            }
        }
    }
    public void UpdateMoveSelection(int selectedMove, Move move)
    {
        for (int i = 0; i < _moveTexts.Count; i++)
        {
            if (i == selectedMove)
            {
                _moveTexts[i].color = _highlightedColor;
            }
            else
            {
                _moveTexts[i].color = Color.black;
            }
        }

        _ppText.text = $"PP {move.PP}/{move.Base.PP}";
        _typeText.text = move.Base.Type.ToString();

    }

    public void SetMoveNames(List<Move> moves)
    {
        for (int i = 0; i < _moveTexts.Count; i++)
        {
            if (i < moves.Count)
                _moveTexts[i].text = moves[i].Base.Name;
            else
                _moveTexts[i].text = "-";

        }
    }























}
