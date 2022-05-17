using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainerController : MonoBehaviour,Interacktable
{
    [SerializeField] string _name;
    [SerializeField] Sprite _sprite;
    [SerializeField] Dialog _dialog;
    [SerializeField] Dialog _dialogAfterBattle;
    [SerializeField] GameObject _exclamation;
    [SerializeField] GameObject _fov;

    //state
    bool _battleLost=false;

    Character _character;
    private void Awake()
    {
            _character = GetComponent<Character>();
    }
    private void Start()
    {
        SetFovRotation(_character.Animator.DefaoultDirection);
    }
    private void Update()
    {
        _character.HandleUpdate();
    }
    public void Interact(Transform initiator)
    {
        _character.LookTowards(initiator.position);
        if (!_battleLost)
        {
            StartCoroutine(DialogManager.Instance.ShowDialog(_dialog, () =>
            {
                GameController.Instance.StartTrainerBattle(this);
            }));
        }
        else
        {
            StartCoroutine(DialogManager.Instance.ShowDialog(_dialogAfterBattle));
        }
    }
    
    public IEnumerator TriggerTrainerBattle(PlayerControll player)
    {
        //show exclamation
        _exclamation.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        _exclamation.SetActive(false);


        //walk towards the player
        var diff=player.transform.position -transform.position;
        var moveVec = diff - diff.normalized;
        moveVec = new Vector2(Mathf.Round(moveVec.x), Mathf.Round(moveVec.y));
        yield return _character.Move(moveVec);

        //show dialog
      StartCoroutine(DialogManager.Instance.ShowDialog(_dialog, () =>
         {
             GameController.Instance.StartTrainerBattle(this);
         }));
    }
    public void BattleLost()
    {
        _battleLost = true;
        _fov.gameObject.SetActive(false);
    }
    public void SetFovRotation(FacingDirection dir)
    {
        float angle = 0f;
        if (dir == FacingDirection.Right)
            angle = 90f;
        else if (dir == FacingDirection.Up)
            angle = 180f;
        else if (dir == FacingDirection.Left)
            angle = 270f;

        _fov.transform.eulerAngles =  new Vector3(0,0, angle);

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
