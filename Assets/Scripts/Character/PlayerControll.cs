using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerControll : MonoBehaviour
{

    [SerializeField] string _name;
    [SerializeField] Sprite _sprite;
    public event Action OnEncountered;
    public event Action<Collider2D> OnEnterTrainersView;
    
    Vector2 _input;
    Character _character;
    private void Awake()
    {
        _character = GetComponent<Character>();
    }
    public void HandleUpdate()
    {
        if (!_character.IsMoving)
        {
            _input.x = Input.GetAxisRaw("Horizontal");
            _input.y = Input.GetAxisRaw("Vertical");
            if (_input.x != 0) _input.y = 0;
            if (_input != Vector2.zero)
            {
                StartCoroutine(_character.Move(_input, OnMoveOver));
            }
        }

        _character.HandleUpdate();
        if (Input.GetKeyDown(KeyCode.Z))
            Interac();
    }

    void Interac()
    {
        var facingDir = new Vector3(_character.Animator.MoveX, _character.Animator.MoveY);
        var interactpos = transform.position + facingDir;

        //Debug.DrawLine(transform.position , interactpos,Color.green,0.5f);

        var collider = Physics2D.OverlapCircle(interactpos, 0.3f, GameLayers.i.InteractableLayer);
        if (collider != null)
        {
            collider.GetComponent<Interacktable>()?.Interact(transform);
        }
    }
    void OnMoveOver()
    {
        CheckForEncounters();
        CheckIfInTrainersView();
    }
    void CheckForEncounters()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.2f, GameLayers.i.GrassLayer) != null)
        {
            if (UnityEngine.Random.Range(1, 101) <= 10)
            {
                _character.Animator.IsMoving = false;
                OnEncountered();
            }
        }
    }
    void CheckIfInTrainersView()
    {
        var collider = Physics2D.OverlapCircle(transform.position, 0.2f, GameLayers.i.FovLayer);
        if (collider!= null)
        {
            _character.Animator.IsMoving = false;
            OnEnterTrainersView?.Invoke(collider);
        }

    }
    public string Name
    {
        get { return _name; }
    }
    public Sprite Sprite
    {
        get { return _sprite; }
    }
}
