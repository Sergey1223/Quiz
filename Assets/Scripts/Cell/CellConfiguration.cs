using UnityEngine.Events;

public class CellConfiguration : Configuration
{
    private CardData _cardData;

    private UnityAction<Cell> _touchedEventListener;

    private UnityAction _lockedEventListener;

    private UnityAction _unlockedEventListener;

    private bool _spawnAnimationEnabled;

    public CardData CardData { get => _cardData; set => _cardData = value; }

    public UnityAction<Cell> TouchedEventListener { get => _touchedEventListener; set => _touchedEventListener = value; }

    public UnityAction LockedEventListener { get => _lockedEventListener; set => _lockedEventListener = value; }

    public UnityAction UnlockedEventListener { get => _unlockedEventListener; set => _unlockedEventListener = value; }

    public bool SpawnAnimationEnabled { get => _spawnAnimationEnabled; set => _spawnAnimationEnabled = value; }
}

