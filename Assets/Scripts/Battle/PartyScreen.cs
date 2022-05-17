using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyScreen : MonoBehaviour
{
    [SerializeField] Text _messageText;

    PartyMemberUI[] _memberSlots;
    List<Pokemon> _pokemons;

    public void Init()
    {
        _memberSlots = GetComponentsInChildren<PartyMemberUI>(true);
    }

    public void SetPartyData(List<Pokemon> pokemons)
    {
        this._pokemons = pokemons; 
        for (int i = 0; i < _memberSlots.Length; i++)
        {
            if(i<pokemons.Count)
            {
                _memberSlots[i].gameObject.SetActive(true);
                _memberSlots[i].SetData(pokemons[i]);
            }
               
            else
                _memberSlots[i].gameObject.SetActive(false);
        }
        _messageText.text = "Choose a Pokemon"; 
    }
    public void UpdateMemberSelection(int selectedMember)
    {
        for (int i = 0; i < _pokemons.Count; i++)
        {
            if (i == selectedMember)
                _memberSlots[i].SetSelected(true);
            else
                _memberSlots[i].SetSelected(false);
        }
    }
    public void SetMessageText(string message)
    {
        _messageText.text=message;
    }
}
