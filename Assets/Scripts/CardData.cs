using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "NewCardData", menuName = "Card Data", order = 1)]
public class CardData : ScriptableObject
{
    [SerializeField]
    private string _identifier;

    [SerializeField]
    private string _name;

    [SerializeField]
    private Sprite _sprite;

    [SerializeField]
    private Color _backgroundColor;

    public string Identifier => _identifier;

    public string Name => _name;

    public Sprite Sprite => _sprite;

    public Color BackgroundColor => _backgroundColor;
}