using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class Cell : LockableMonoBehaviour, IInitializable
{
    private const string BackgroundComponentName = "Background";
    private const string FrameComponentName = "Frame";
    private const string ContentComponentName = "Content";

    [SerializeField]
    [HideInInspector]
    private CardData _cardData;

    [SerializeField]
    private CellTouchedEvent _touchedEvent = new CellTouchedEvent();

    [SerializeField]
    private bool _spawnAnimationEnabled;

    [SerializeField]
    private Tweener _tweener;

    [SerializeField]
    [HideInInspector]
    private Transform _contentTransform;

    [SerializeField]
    [HideInInspector]
    private Transform _frameTransform;

    [SerializeField]
    [HideInInspector]
    private Transform _backgroundTransform;

    [HideInInspector]
    public CardData CardData => _cardData;

    public CellTouchedEvent TouchedEvent => _touchedEvent;

    private void Start()
    {
        if (_spawnAnimationEnabled) {
            PlaySpawnAnimation();
        }
    }

    private void OnMouseDown()
    {
        TouchedEvent.Invoke(this);
    }

    private void OnDestroy()
    {
        _touchedEvent.RemoveAllListeners();
        _lockedEvent.RemoveAllListeners();
        _unlockedEvent.RemoveAllListeners();
    }

    private void SetBackgroundColor(Color color)
    {
        if (_backgroundTransform.TryGetComponent(out SpriteRenderer renderer))
        {
            renderer.color = color;
        }
    }

    private void SetContentSprite(Sprite sprite)
    {
        if (_contentTransform.TryGetComponent(out SpriteRenderer spriteRenderer))
        {
            spriteRenderer.sprite = sprite;
        }
    }

    private void RunBounceEffet(Transform target)
    {
        Vector3 sourceScale = target.localScale;
        target.localScale = Vector2.zero;

        _tweener?.Kill();

        _tweener = target.DOScale(sourceScale, 1f).SetEase(Ease.InOutBounce).OnComplete(Unlock);
    }

    private void PlaySpawnAnimation()
    {
        Lock();

        RunBounceEffet(transform);
    }

    private IEnumerator PlayRightAnswerAnimation()
    {
        Lock();

        RunBounceEffet(_contentTransform);

        if (TryGetComponent(out ParticleSystem particleSystem))
        {
            if (!particleSystem.isPlaying)
            {
                particleSystem.Play();

                yield return new WaitForSeconds(particleSystem.main.duration);
            }
        }

        Unlock();
    }
    
    private void PlayWrongAnswerAnimation()
    {
        _tweener?.Kill();

        _tweener = _contentTransform.DOShakePosition(1f, Vector2.left / 2, 20).OnComplete(() => _unlockedEvent.Invoke());
    }

    public void Initialize(Configuration configuration)
    {
        _backgroundTransform = transform.Find(BackgroundComponentName);
        _contentTransform = transform.Find(ContentComponentName);

        if (configuration is CellConfiguration cellConfiguration)
        {
            _cardData = cellConfiguration.CardData;
            _spawnAnimationEnabled = cellConfiguration.SpawnAnimationEnabled;

            SetBackgroundColor(cellConfiguration.CardData.BackgroundColor);
            SetContentSprite(cellConfiguration.CardData.Sprite);

            _touchedEvent.AddListener(cellConfiguration.TouchedEventListener);
            _lockedEvent.AddListener(cellConfiguration.LockedEventListener);
            _unlockedEvent.AddListener(cellConfiguration.UnlockedEventListener);
        }
    }

    public Vector2 GetSize()
    {
        if (_frameTransform == null)
        {
            _frameTransform = transform.Find(FrameComponentName);
        }

        if (_frameTransform.TryGetComponent(out SpriteRenderer renderer))
        {
            return renderer.bounds.size;
        }

        return Vector2.zero;
    }

    public void RevealCheckResult(bool result)
    {
        if (result)
        {
            StartCoroutine(PlayRightAnswerAnimation());
        }
        else
        {
            PlayWrongAnswerAnimation();
        }
    }

    public void SetContent(CardData cardData)
    {
        _cardData = cardData;

        SetContentSprite(cardData.Sprite);
    }
}

