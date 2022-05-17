using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleDialogBox : MonoBehaviour
{
    [SerializeField] int _lettersPerSecond;
    [SerializeField] Color _hightlightedColor;
    [SerializeField] Text _dialogText;
    [SerializeField] GameObject _actionSelector;
    [SerializeField] GameObject _moveSelector;
    [SerializeField] GameObject _moveDetails;
    [SerializeField] GameObject _choiceBox;

    [SerializeField] List<Text> _actionTexts;
    [SerializeField] List<Text> _moveTexts;

    [SerializeField] Text _yesText;
    [SerializeField] Text _noText;

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
            yield return new WaitForSeconds(1f /_lettersPerSecond);
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
    public void EnableChoiceBox(bool enabled)
    {
        _choiceBox.SetActive(enabled);
    }
    public void UpdateActionSelection(int selectedAction)
    {
        for (int i = 0; i < _actionTexts.Count; i++)
        {
            if (i == selectedAction)
                _actionTexts[i].color = _hightlightedColor;
            else
                _actionTexts[i].color = Color.black;
        }
    }
    public void UpdateMoveSelection(int selectedMove, Move move)
    {
        for (int i = 0; i < _moveTexts.Count; i++)
        {
            if (i == selectedMove)
                _moveTexts[i].color = _hightlightedColor;
            else
                _moveTexts[i].color= Color.black;
        }
        _ppText.text= $"PP {move.PP}/{move.Base.PP}";
        _typeText.text=move.Base.Type.ToString();

        if (move.PP == 0)
            _ppText.color = Color.red;
        else
            _ppText.color= Color.green;
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
    public void UpdateChoiceBox(bool yesSelected)
    {
        if (yesSelected)
        {
            _yesText.color = _hightlightedColor;
            _noText.color = Color.black;
        }
        else
        {
            _yesText.color = Color.black;
            _noText.color = _hightlightedColor; 
        }
    }
}
