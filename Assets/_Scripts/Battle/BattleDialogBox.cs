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

    public void EnableMoveSelector(bool enabled)
    {
        _moveSelector.SetActive(enabled);
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
