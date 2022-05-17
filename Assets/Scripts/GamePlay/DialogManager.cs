using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DialogManager : MonoBehaviour
{

    [SerializeField] GameObject _dialogBox;
    [SerializeField] Text _dialogText;
    [SerializeField] int _lettersPerSecond;
    public event Action OnShowDialog;
    public event Action OnCloseDialog;

    public static DialogManager Instance { get; private set; }
    private void Awake()
    {
          Instance = this;
    }
    Dialog _dialog;
    Action onDialogFinished;
    int _currentLine = 0;
    bool _isTyping;
    public bool IsShowing { get; private set; }
    public IEnumerator ShowDialog(Dialog dialog , Action onFinished =null)
    {
        yield return new WaitForEndOfFrame();
        OnShowDialog?.Invoke();

        IsShowing = true;
        this._dialog = dialog;
        onDialogFinished = onFinished;

      _dialogBox.SetActive(true);
        StartCoroutine(TypeDialog(dialog.Lines[0]));
    }

    public void HandleUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Z) && !_isTyping)
        {
            ++_currentLine;
            if(_currentLine < _dialog.Lines.Count)
            {
                StartCoroutine(TypeDialog(_dialog.Lines[_currentLine]));
            }
            else
            {
                _currentLine = 0;
                IsShowing = false;
                _dialogBox.SetActive(false);
                onDialogFinished?.Invoke();
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
