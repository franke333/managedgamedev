using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartController : MonoBehaviour, IDamagable
{
    public PartSO PartSO;


    int _maxHealth;
    int _health;

    public const int maxWidth = 3, maxHeight = 3;

    public Vector2Int shipPosition;


    public int Health
    {
        get { return _health; }
        private set
        {
            _health = value;
            if(_health > _maxHealth)
            {
                _health = _maxHealth;
            }
            if (_health <= 0)
            {
                //TODO destroy part
            }
        }
    }

    public bool TakeDamage(int damage)
    {
        if(damage <= 0)
            return false;

        TakeDamageVFX();
        Health -= damage;
        return Health <= 0;
    }

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>() ?? gameObject.AddComponent<SpriteRenderer>();

        if (PartSO == null)
        {
            Debug.LogError("PartSO is null");
            return;
        }

        _maxHealth = PartSO._maxHealth;
        Health = _maxHealth;
        _spriteRenderer.sprite = PartSO.sprite;
       _shape = PartSO.shape;
    }


    void Update()
    {
    }


    // HitBox script
    // Shape script
    public bool[] _shape;
    public bool IsPart(Vector2Int position)
    {
        return _shape[position.y * maxWidth + position.x];
    }

    // Renderer script

    SpriteRenderer _spriteRenderer;

    float _damageVFXTime = 0.3f;
    Sequence tw;
    public void TakeDamageVFX()
    {
        if (tw != null)
        {
            tw.Kill();
        }
        tw = DOTween.Sequence();
        tw.Append(_spriteRenderer.DOColor(Color.red, _damageVFXTime).SetEase(Ease.OutCubic).OnComplete(() => _spriteRenderer.DOColor(Color.white, _damageVFXTime)));
        tw.Join(transform.DOShakePosition(_damageVFXTime, 0.1f, 25, 90, false));
        Vector3 caputredPos = transform.position;
        tw.OnKill(() => { tw = null; transform.position = caputredPos; _spriteRenderer.color = Color.white; });
    }

    // ...

}
