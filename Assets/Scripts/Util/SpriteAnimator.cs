using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimator
{

    SpriteRenderer _spriteRenderer;
    List<Sprite> _frames;
    float _frameRate;

    int _currentFrame;
    float _timer;
    
    public SpriteAnimator(List<Sprite> frames , SpriteRenderer spriteRenderer, float frameRate=0.16f)
    {
        this._frames = frames;
        this._spriteRenderer = spriteRenderer;
        this._frameRate = frameRate;

    }
    public void Start()
    {
        _currentFrame = 0;
        _timer = 0f;
        _spriteRenderer.sprite = _frames[0];
    }
    public void HandleUpdate()
    {
        _timer += Time.deltaTime;
        if(_timer >_frameRate)
        {
            _currentFrame =(_currentFrame +1) % _frames.Count;
            _spriteRenderer.sprite = _frames[_currentFrame];
            _timer -= _frameRate;
        }
    }
    public List<Sprite> Frames
    {
        get { return _frames; }
    }
}
