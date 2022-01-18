using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public class LoaderScreen : MonoBehaviour
{
    [SerializeField]
    private float _fadeDuration;

    [SerializeField]
    private UnityEvent _shownEvent = new UnityEvent();

    [SerializeField]
    private UnityEvent _closedEvent = new UnityEvent();

    [SerializeField]
    [HideInInspector]
    private Tweener _tweener;

    [SerializeField]
    [HideInInspector]
    private Image _image;

    public float FadeDuration => _fadeDuration;

    public UnityEvent ShownEvent => _shownEvent;

    public UnityEvent ClosedEvent => _closedEvent;

    private void Start()
    {
        _image = GetComponent<Image>();
    }

    private void Fade(float value, TweenCallback callback)
    {
        _tweener?.Kill();

        _tweener = _image.DOFade(value, _fadeDuration).OnComplete(callback);
    }

    public void Show()
    {
        gameObject.SetActive(true);

        Fade(1f, () => _shownEvent.Invoke());
    }

    public void Close()
    {
        Fade(0f, () =>
        {
            _closedEvent.Invoke();
            gameObject.SetActive(false);
        });
    }
}
