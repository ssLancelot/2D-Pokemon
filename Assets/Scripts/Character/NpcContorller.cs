using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcContorller : MonoBehaviour, Interacktable
{
    [SerializeField] Dialog _dialog;
    [SerializeField] List<Vector2> _movementPattern;
    [SerializeField] float _timeBetweenPattern;
    Character _character;
    NPCState _state;
    float _idleTimer=0f;
    int _currentPattern=0;
    private void Awake()
    {
        _character = GetComponent<Character>();
    }
    public void Interact(Transform initiator)
    {
        if(_state == NPCState.Idle)
        {
            _state = NPCState.Dialog;
            _character.LookTowards(initiator.position);

            StartCoroutine(DialogManager.Instance.ShowDialog(_dialog, () =>
            {
                _idleTimer = 0f;
                _state=NPCState.Idle;
            }));
        }

      
    
    }
    private void Update()
    {

        if(_state ==NPCState.Idle)
        {
            _idleTimer += Time.deltaTime;
            if(_idleTimer >_timeBetweenPattern)
            {
                _idleTimer = 0f;
                if(_movementPattern.Count > 0)
                StartCoroutine(Walk());
            }
        }
         _character.HandleUpdate();
    }
    IEnumerator Walk()
    {
        _state = NPCState.Walking;
         
        var oldPos = transform.position;

      yield return  _character.Move(_movementPattern[_currentPattern]);
        if(transform.position != oldPos)
        _currentPattern += (_currentPattern + 1) % _movementPattern.Count;
        _state = NPCState.Idle;
    }
}
public enum NPCState { Idle,Walking,Dialog}
