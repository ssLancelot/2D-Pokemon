using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { FreeRoam, Battle , Dialog , CutScene}

public class GameController : MonoBehaviour
{
    [SerializeField] PlayerControll _playerController;
    [SerializeField] BattleSystem _battleSystem;
    [SerializeField] Camera _worldCamera;

    GameState state;
    public static GameController Instance { get; private set; }
    private void Awake()
    {
        Instance=this;
            ConditionDB.Init();
    }
    private void Start()
    {
        _playerController.OnEncountered += StartBattle;
        _battleSystem.OnBattleOver += EndBattle;
        _playerController.OnEnterTrainersView += (Collider2D trainerCollider) =>
        {
         var trainer = trainerCollider.GetComponentInParent<TrainerController>();
            if(trainer != null)
            {
                state = GameState.CutScene;
               StartCoroutine( trainer.TriggerTrainerBattle(_playerController));
            }
        };

        DialogManager.Instance.OnShowDialog += () =>
        {
                state = GameState.Dialog;
        };
        DialogManager.Instance.OnCloseDialog += () =>
        {
            if(state == GameState.Dialog)
            state = GameState.FreeRoam;
        };
    }

    void StartBattle()
    {
        state = GameState.Battle;
        _battleSystem.gameObject.SetActive(true);
        _worldCamera.gameObject.SetActive(false);
         
        var playerParty = _playerController.GetComponent<PokemonParty>();
        var wildPokemon= FindObjectOfType<MapArea>().GetComponent<MapArea>().GetRandomWildPokemon();

        var wildPokemonCopy = new Pokemon(wildPokemon.Base, wildPokemon.Level);
        _battleSystem.StartBattle(playerParty,wildPokemonCopy);
    }
    TrainerController _trainer;
   public void StartTrainerBattle(TrainerController trainer)
    {
        state = GameState.Battle;
        _battleSystem.gameObject.SetActive(true);
        _worldCamera.gameObject.SetActive(false);

        this._trainer = trainer;

        var playerParty = _playerController.GetComponent<PokemonParty>();
        var tranerParty = trainer.GetComponent<PokemonParty>();
        _battleSystem.StartTrainerBattle(playerParty, tranerParty);
    }

    void EndBattle(bool won)
    {
        if(_trainer != null && won)
        {
            _trainer.BattleLost();
            _trainer = null;
        }

        state = GameState.FreeRoam;
        _battleSystem.gameObject.SetActive(false);
        _worldCamera.gameObject.SetActive(true);
    }
    private void Update()
    {
        if (state == GameState.FreeRoam)
        {
            _playerController.HandleUpdate();
        }
        else if (state == GameState.Battle)
        {
            _battleSystem.HandleUpdate();
        }
        else if(state == GameState.Dialog)
        {
            DialogManager.Instance.HandleUpdate();
        }
    }
}
