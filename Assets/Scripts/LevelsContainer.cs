using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class LevelsContainer : MonoBehaviour
{
    [SerializeField]
    private Level[] _levels;

    [SerializeField]
    private GridObject _grid;

    [SerializeField]
    private QuestionGenerator _questionGenerator;

    [SerializeField]
    private LoaderScreen _loaderScreen;

    [SerializeField]
    private UnityEvent _allLevelsComplete;

    [SerializeField]
    private UnityEvent _levelLoaded;

    [SerializeField]
    private int _levelPointer = 0;

    public Level[] Levels => _levels;
    
    public GridObject Grid => _grid;

    public QuestionGenerator QuestionGenerator => _questionGenerator;

    public LoaderScreen LoaderScreen => _loaderScreen;

    public UnityEvent AllLevelsComplete => _allLevelsComplete;

    public UnityEvent LevelLoaded => _levelLoaded;

    private void Start()
    {
        _grid.InitializedEvent.AddListener(Reset);
    }

    private void LevelUp()
    {
        _grid.UnlockedEvent.RemoveListener(LevelUp);

        if (++_levelPointer > _levels.Length)
        {
            // Start ignore cell touches.
            _grid.CellTouchedEvent.RemoveListener(ProcessAnswer);

            _allLevelsComplete.Invoke();

            return;
        }

        Level currentLevel = _levels[_levelPointer - 1];
        int bundleIndex = UnityEngine.Random.Range(0, currentLevel.DataBundles.Length);

        _grid.Build(currentLevel.DataBundles[bundleIndex], _levelPointer == 1);

        _questionGenerator.Generate(currentLevel.DataBundles[bundleIndex], _levelPointer == 1);
    }

    private void ProcessAnswer(Cell cell)
    {
        bool result = _questionGenerator.CheckAnswer(cell.CardData);

        if (result)
        {
            _grid.UnlockedEvent.AddListener(LevelUp);
        }

        cell.RevealCheckResult(result);
    }

    public void Reset()
    {
        _levelPointer = 0;

        _grid.Clear();

        // Resume process cell touches.
        _grid.CellTouchedEvent.AddListener(ProcessAnswer);

        LevelUp();

        _levelLoaded.Invoke();
    }
}
