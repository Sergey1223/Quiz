using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "NewCardBundleData", menuName = "Card Bundle Data", order = 2)]
public class CardBundleData : ScriptableObject
{
    [SerializeField]
    private CardData[] _cards;

    public CardData[] Cards => _cards;
}