using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ResetWindow : MonoBehaviour
{
    private const string ResetButtonComponentName = "ResetButton";

    [SerializeField]
    [Range(0f, 1f)]
    private float _shadingValue;

    [SerializeField]
    private float _duration;

    [SerializeField]
    [HideInInspector]
    private Transform _resetButton;

    [SerializeField]
    [HideInInspector]
    private Image _image;

    [SerializeField]
    [HideInInspector]
    private Tweener _tweener;

    [SerializeField]
    [HideInInspector]
    private Color _imageColor;

    public float ShadingValue => _shadingValue;

    public float Duration => _duration;

    private void Start()
    {
        _resetButton = transform.Find(ResetButtonComponentName);
        _image = GetComponent<Image>();
        _imageColor = _image.color;
    }

    public void Show()
    {
        _tweener?.Kill();

        _tweener = _image.DOFade(_shadingValue, _duration).OnComplete(() => _resetButton.gameObject.SetActive(true));
    }

    public void Close()
    {
        _resetButton.gameObject.SetActive(false);

        _image.color = _imageColor;
    }
}