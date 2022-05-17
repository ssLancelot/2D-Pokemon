using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyMemberUI : MonoBehaviour
{

    [SerializeField] Text _nameText;
    [SerializeField] Text _levelText;
    [SerializeField] HPBar _hpBar;
    Pokemon _pokemon;
    [SerializeField] Color _hightLightedColor;
    public void SetData(Pokemon pokemon)
    {
        _pokemon = pokemon;
        _nameText.text = pokemon.Base.name;
        _levelText.text = "Lvl " + pokemon.Level;
        _hpBar.SetHp((float)pokemon.HP / pokemon.MaxHp);
    }
    public void SetSelected(bool selected)
    {
        if (selected)
            _nameText.color = _hightLightedColor;
        else
            _nameText.color = Color.black;
    }
}
