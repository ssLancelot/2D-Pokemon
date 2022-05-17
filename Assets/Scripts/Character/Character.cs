using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Character : MonoBehaviour
{
    [SerializeField] float _moveSpeed;
    public bool IsMoving { get; private set; }
    CharacterAnimator _animator;
    private void Awake()
    {
        _animator = GetComponent<CharacterAnimator>();
    }
    public IEnumerator Move(Vector2 moveVec, Action OnMoveOver = null)
    {
        _animator.MoveX = Mathf.Clamp(moveVec.x, -1f, 1f);
        _animator.MoveY = Mathf.Clamp(moveVec.y, -1f, 1f);
   
        var targetPos = transform.position;
        targetPos.x += moveVec.x;
        targetPos.y += moveVec.y;

        if (!IsPathClear(targetPos))
            yield break;


      IsMoving = true;
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, _moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;
        IsMoving = false;
        OnMoveOver?.Invoke();
    }
    public void HandleUpdate()
    {
       _animator.IsMoving = IsMoving;
    }
    bool IsPathClear(Vector3 targetPos)
    {
        var diff = targetPos - transform.position;
        var dir = diff.normalized;
        if (Physics2D.BoxCast(transform.position + dir, new Vector2(0.2f, 0.2f), 0f, dir, diff.magnitude - 1, GameLayers.i.SolidObcejtLayer | GameLayers.i.InteractableLayer | GameLayers.i.PlayerLayer) == true)
            return false;

        return true;


    }
    private bool IsWalkable(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.15f, GameLayers.i.SolidObcejtLayer | GameLayers.i.InteractableLayer) != null)
        {
            return false;
        }
        return true;
    }
    public void LookTowards(Vector3 targetPos)
    {
        var xDiff = Mathf.Floor(targetPos.x) - Mathf.Floor(transform.position.x);
        var yDiff = Mathf.Floor(targetPos.y) - Mathf.Floor(transform.position.y);

        if(xDiff ==0 || yDiff ==0)
        {
            _animator.MoveX = Mathf.Clamp(xDiff, -1f, 1f);
            _animator.MoveY = Mathf.Clamp(yDiff, -1f, 1f);
        }
        else
        {
            Debug.Log("Error in look towards : You can't ask the charakter to look diagonally");
        }

    }
    public CharacterAnimator Animator
    {
        get => _animator;
    }
}
