using System;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public event Action<int> OnValueChanged;

    [SerializeField] private Game _game;
    [SerializeField] private FieldController _fieldController;
    [SerializeField] private TextMeshProUGUI _scoreText;

    public int Value { get; private set; }

    public void Awake()
    {
        if (_game == null)
            throw new NullReferenceException();
        if (_scoreText == null)
            throw new NullReferenceException();
    }

    public void AddScore(int value)
    {
         if (value < 0)
            throw new ArgumentOutOfRangeException();

        Value += value;
        OnValueChanged?.Invoke(Value);
        UpdateScoreText();
    }

    public void Clear()
    {
        Value = 0;
        UpdateScoreText();
    }

    private void UpdateScoreText() => _scoreText.text = Value.ToString();
    private void GameFieldCreated(HexMassive gameField) => SubscribeOnTiles(gameField);
    private void GameFieldDeleted(HexMassive gameField)
    {
        UnsubscribeOnTiles(gameField);
        Clear();
    } 
    private void GameRestart() => Clear();

    private void SubscribeOnTiles(HexMassive gameField)
    {
        var tiles = gameField.GetAllTiles();
        foreach (var tile in tiles)
            tile.OnMultiplied += TileMultiplied;
    }

    private void UnsubscribeOnTiles(HexMassive gameField)
    {
        var tiles = gameField.GetAllTiles();
        foreach (var tile in tiles)
            tile.OnMultiplied -= TileMultiplied;
    }

    private void TileMultiplied(int value) => AddScore(value);

    private void OnEnable()
    {
        _game.OnGameRestart += GameRestart;
        _fieldController.OnGameFieldCreated += GameFieldCreated;
        _fieldController.OnGameFieldDeleted += GameFieldDeleted;       
    }

    private void OnDisable()
    {
        _game.OnGameRestart -= GameRestart;
        _fieldController.OnGameFieldCreated -= GameFieldCreated;
        _fieldController.OnGameFieldDeleted -= GameFieldDeleted;
    }
}
