using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHud : MonoBehaviour
{
    [SerializeField] Text _nameText;
    [SerializeField] Text _levelText;
    [SerializeField] Text _statusText;
    [SerializeField] HPBar _hpBar;
    [SerializeField] GameObject _expBar;
    [SerializeField] Color _psnColor;
    [SerializeField] Color _brnColor;
    [SerializeField] Color _slpColor;
    [SerializeField] Color _parColor;
    [SerializeField] Color _frzColor;
    Pokemon _pokemon;

    Dictionary<ConditionID, Color> _statusColors;

    public void SetData(Pokemon pokemon)    
    {
        _pokemon = pokemon;
        _nameText.text = pokemon.Base.name;
        SetLevel();
        _hpBar.SetHp((float)pokemon.HP/pokemon.MaxHp);
        SetExp();
        _statusColors = new Dictionary<ConditionID, Color>()
        {
            {ConditionID.psn , _psnColor},{ConditionID.brn, _brnColor},
          {ConditionID.slp, _slpColor},  {ConditionID.par, _parColor},
            {ConditionID.frz, _frzColor},

        };


        SetStatusText();
        _pokemon.OnStatusChanged += SetStatusText;
    }
    void SetStatusText()
    {
        if(_pokemon.Status == null)
        {
            _statusText.text = "";
        }
        else
        {
            _statusText.text = _pokemon.Status.ID.ToString().ToUpper();
           _statusText.color = _statusColors[_pokemon.Status.ID];
        }
    }
    public void SetLevel()
    {
        _levelText.text = "Lvl " + _pokemon.Level;   
    }
    public void SetExp()
    {
        if (_expBar == null) return;
       float normalizedExp =  GetNormalizedExp();
        _expBar.transform.localScale = new Vector3(normalizedExp, 1, 1);
    }
    public IEnumerator SetExpSmooth(bool reset=false)
    {
        if (_expBar == null) yield break;

        if(reset)
            _expBar.transform.localScale = new Vector3(0,1,1);
        float normalizedExp = GetNormalizedExp();
      yield return  _expBar.transform.DOScaleX(normalizedExp, 1.5f).WaitForCompletion();
    }
    float GetNormalizedExp()
    {
        int currentLevelexp = _pokemon.Base.GetExpForLevel(_pokemon.Level);
        int nextLevelExp = _pokemon.Base.GetExpForLevel(_pokemon.Level + 1 );

       float normalizedExp =(float) (_pokemon.Exp -currentLevelexp) / (nextLevelExp - currentLevelexp);
        return Mathf.Clamp01(normalizedExp);
    }
    public IEnumerator UpdateHp()
    {
        if (_pokemon.HpChanged)
        {
            yield return _hpBar.SetHpSmooth((float)_pokemon.HP / _pokemon.MaxHp);
            _pokemon.HpChanged = false;
        }

    }
}
