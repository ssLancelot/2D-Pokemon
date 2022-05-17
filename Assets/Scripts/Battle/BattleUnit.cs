using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleUnit : MonoBehaviour
{

    [SerializeField] bool _isPlayerUnit;
    [SerializeField] BattleHud _hud;
    public bool IsplayerUnit { get { return _isPlayerUnit; } }

    public BattleHud Hud { get { return _hud; } }

    public Pokemon Pokemon { get; set; }
    Vector3 _orginalPos;
    Image _image;
    Color _orginalColor;
    private void Awake()
    {
        _image = GetComponent<Image>();
        _orginalPos = _image.transform.localPosition;
        _orginalColor = _image.color;
    }
    public void Setup(Pokemon pokemon)
    {
        Pokemon = pokemon;
        if (_isPlayerUnit)
            _image.sprite = Pokemon.Base.BackSprite;
        else
            _image.sprite = Pokemon.Base.FrontSprite;

        _hud.gameObject.SetActive(true);
        _hud.SetData(pokemon);

        transform.localScale = new Vector3(1, 1, 1);
        _image.color = _orginalColor;
        PlayEnterAnimation();
    }
    public void Clear()
    {
        _hud.gameObject.SetActive(false);
    }
    public void PlayEnterAnimation()
    {
        if (_isPlayerUnit)
            _image.transform.localPosition = new Vector3(-1100f, _orginalPos.y);
        else
            _image.transform.localPosition = new Vector3(1100f, _orginalPos.y);
        _image.transform.DOLocalMoveX(_orginalPos.x, 2f);
    }
    public void PlayAttackAnimation()
    {
        var sequence = DOTween.Sequence();
        if (_isPlayerUnit)
            sequence.Append(_image.transform.DOLocalMoveX(_orginalPos.x + 50f, 0.25f));
        else
            sequence.Append(_image.transform.DOLocalMoveX(_orginalPos.x - 50f, 0.25f));
        sequence.Append(_image.transform.DOLocalMoveX(_orginalPos.x, 0.25f));
    }
    public void PlayHitAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(_image.DOColor(Color.gray, 0.1f));
        sequence.Append(_image.DOColor(_orginalColor, 0.1f));
    }
    public void PlayFaintAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(_image.transform.DOLocalMoveY(_orginalPos.y - 150f, 0.5f));
        sequence.Join(_image.DOFade(0f, 0.5f));
    }
    public IEnumerator PlayCaptureAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(_image.DOFade(0, 0.5f));
        sequence.Join(transform.DOLocalMoveY(_orginalPos.y + 50f, 0.5f));
        sequence.Join(transform.DOScale(new Vector3(0.3f, 0.3f, 1f),0.5f));
        yield return sequence.WaitForCompletion();
    }
    public IEnumerator PlayBreakOutAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(_image.DOFade(1, 0.5f));
        sequence.Join(transform.DOLocalMoveY(_orginalPos.y , 0.5f));
        sequence.Join(transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f));
        yield return sequence.WaitForCompletion();
    }
}
