using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] List<Sprite> _walkDownSprite;
    [SerializeField] List<Sprite> _walkUpSprite;
    [SerializeField] List<Sprite> _walkRightSprite;
    [SerializeField] List<Sprite> _walkLeftSprite;
    [SerializeField] FacingDirection _defaultDirection=FacingDirection.Down;
  //Parameters
  public float MoveX { get; set; }
  public float MoveY { get; set; }
  public bool IsMoving { get; set; }


    //States
    SpriteAnimator _walkDownAnim;
    SpriteAnimator _walkUpAnim;
    SpriteAnimator _walkRightAnim;
    SpriteAnimator _walkLeftAnim;


    SpriteAnimator _currentAnim;
    bool _wasPreviouslyMoving;
    //Referances
    SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _walkDownAnim = new SpriteAnimator(_walkDownSprite,_spriteRenderer);
        _walkUpAnim = new SpriteAnimator(_walkUpSprite,_spriteRenderer);
        _walkRightAnim = new SpriteAnimator(_walkRightSprite,_spriteRenderer);
        _walkLeftAnim = new SpriteAnimator(_walkLeftSprite,_spriteRenderer);
        SetFacingDirection(_defaultDirection);

        _currentAnim = _walkDownAnim;
    }
    private void Update()
    {
        var prevAnim  =_currentAnim;

        if (MoveX == 1)
            _currentAnim = _walkRightAnim;
        else if (MoveX == -1)
            _currentAnim = _walkLeftAnim;
        else if (MoveY == 1)
            _currentAnim = _walkUpAnim;
        else if (MoveY == -1)
            _currentAnim = _walkDownAnim;

        if (_currentAnim != prevAnim || IsMoving != _wasPreviouslyMoving)
            _currentAnim.Start();


        if (IsMoving)
            _currentAnim.HandleUpdate();
        else
            _spriteRenderer.sprite = _currentAnim.Frames[0];

        _wasPreviouslyMoving = IsMoving;
    }
    public void SetFacingDirection(FacingDirection dir)
    {
        if (dir == FacingDirection.Right)
            MoveX = 1;
        else if(dir == FacingDirection.Left)
            MoveX = -1;
        else if (dir == FacingDirection.Up)
            MoveY = 1;
        else if(dir == FacingDirection.Down)
            MoveY = -1;
    }
    public FacingDirection DefaoultDirection
    {
        get => _defaultDirection;
    }
}
public enum FacingDirection
{
    Up,Down,Left,Right
}
