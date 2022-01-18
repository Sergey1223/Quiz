using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class GridObject : LockableMonoBehaviour
{
    [SerializeField]
    private float _maxWidth;

    [SerializeField]
    private float _maxHeight;

    [SerializeField]
    private float _cellGap;

    [SerializeField]
    private int _columnsCount;

    [SerializeField]
    private GameObject _cellPrefab;

    [SerializeField]
    [HideInInspector]
    private UnityEvent _initializedEvent = new UnityEvent();

    [SerializeField]
    [HideInInspector]
    private CellTouchedEvent _cellTouchedEvent = new CellTouchedEvent();

    [SerializeField]
    private Vector2 _actualSize;

    [SerializeField]
    private Vector2 _cellSize;

    [SerializeField]
    private int _rowsCount;

    public float MaximumWidth => _maxWidth;

    public float MaximumHeight => _maxHeight;

    public float CellGap => _cellGap;

    public int ColumnsCount => _columnsCount;

    public GameObject CellPrefab => _cellPrefab;

    [HideInInspector]
    public UnityEvent InitializedEvent => _initializedEvent;

    [HideInInspector]
    public CellTouchedEvent CellTouchedEvent => _cellTouchedEvent;

    private void Start()
    {
        _cellSize = CellPrefab.transform.GetComponent<Cell>().GetSize();

        _initializedEvent.Invoke();
    }

    private Vector2 CalculateActualSize()
    {
        return new Vector2
        {
            x = (_cellSize.x * _columnsCount + (_columnsCount - 1) * _cellGap),
            y = (_cellSize.y * _rowsCount + (_rowsCount - 1) * _cellGap)
        };
    }

    private Vector2 CalcualteAnchorPosition()
    {
        Vector2 extents = _actualSize / 2;

        return new Vector2
        {
            x = transform.position.x - extents.x,
            y = transform.position.y + extents.y
        };
    }

    private Vector2 CalculateCellPosition(int rowIndex, int columnIndex, Vector2 anchor)
    {
        return new Vector2
        {
            x = anchor.x + columnIndex * (_cellSize.x + _cellGap) + _cellSize.x / 2,
            y = anchor.y - rowIndex * (_cellSize.y + _cellGap) - _cellSize.y / 2
        };
    }

    private void Scale()
    {
        float ratio = 1;

        if (_actualSize.x > _maxWidth)
        {
            ratio = _maxWidth / _actualSize.x;
        }

        if (_actualSize.y > _maxHeight)
        {
            ratio = _maxHeight / _actualSize.y;
        }

        transform.localScale = new Vector2(ratio, ratio);
    }

    private void OnCellTouched(Cell cell)
    {
        if (!_locked)
        {
            _cellTouchedEvent.Invoke(cell);
        }
    }

    public void Build(CardBundleData bundleData, bool playAnimation)
    {
        transform.localScale = Vector2.one;

        _rowsCount = (int)Math.Ceiling((double)(bundleData.Cards.Length / ColumnsCount));
        _actualSize = CalculateActualSize();

        Vector2 anchorPosition = CalcualteAnchorPosition();

        int cellIndex = 0;
        for (int i = 0; i < _rowsCount; i++)
        {
            for (int j = 0; j < _columnsCount; j++)
            {
                cellIndex = _columnsCount * i + j;

                if (cellIndex + 1 > transform.childCount)
                {
                    CellConfiguration cellConfiguration = new CellConfiguration()
                    {
                        CardData = bundleData.Cards[cellIndex],
                        TouchedEventListener = OnCellTouched,
                        LockedEventListener = Lock,
                        UnlockedEventListener = Unlock,
                        SpawnAnimationEnabled = playAnimation
                    };

                    this.Spawn(CellPrefab, CalculateCellPosition(i, j, anchorPosition), transform, cellConfiguration);
                }
                else
                {
                    Transform child = transform.GetChild(cellIndex);

                    child.position = CalculateCellPosition(i, j, anchorPosition);

                    if (child.TryGetComponent(out Cell cell))
                    {
                        cell.SetContent(bundleData.Cards[cellIndex]);
                    }
                }
            }
        }

        if (cellIndex + 1 < transform.childCount)
        {
            for (int i = transform.childCount - 1; i >= cellIndex + 1; i--)
            {
                GameObject gameObject1 = transform.GetChild(i).gameObject;
                Destroy(gameObject1);
                Debug.Log($"Destroyed ({transform.childCount}, {gameObject1.GetInstanceID()}, {gameObject1.transform.position})");
            }
        }

        Scale();
    }

    public void Clear()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        transform.DetachChildren();
    }
}
