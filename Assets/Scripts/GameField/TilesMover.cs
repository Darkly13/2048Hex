using System.Collections.Generic;
using UnityEngine;
using System;

public class TilesMover: MonoBehaviour
{
    public event Action OnNextTurn;

    [SerializeField] private Game _game;
    [SerializeField] private FieldController _fieldController;
    [SerializeField] private InputController _inputController;

    private bool _isAnyMove = false;
    private HexMassive _tiles;

    public void Awake()
    {
        if (_game == null)
            throw new NullReferenceException();
        if (_fieldController == null)
            throw new NullReferenceException();
    }
    private void GameFieldCreated(HexMassive tiles) => Initialize(tiles);
    private void Initialize(HexMassive tiles) => _tiles = tiles;

    public void TryMove(Vector2Int direction)
    {
        if (!_game.InGame)
            return;

        int width = _tiles.Size;
        int widthHalf = (width - 1) / 2;
        int directionModificator = 1;

        Vector2Int startPoint = direction * widthHalf;

        if (direction.y == 0)
        {
            startPoint.x *= 2;
            directionModificator = -1;
        }

        for (int i = -widthHalf; i <= widthHalf; i++)
        {
            int x;
            int y = startPoint.y - i;

            if (y < -widthHalf || y > widthHalf)
            {
                y = startPoint.y;
                x = startPoint.x + Mathf.Abs(i) * (-direction.x * 2);
            }
            else
                x = startPoint.x + Mathf.Abs(i) * direction.x * directionModificator;

            AddLine(new Vector2Int(x, y), direction, i);
        }

        if (_isAnyMove)
        {
            _isAnyMove = false;
            OnNextTurn?.Invoke();
            //_fieldController.SpawnTile();
        }
    }

    private void AddLine(Vector2Int startPoint, Vector2Int direction, int iteration)
    {
        if (direction.y == 0)
            direction.x *= 2;

        List<Tile> tileLine = new List<Tile>();
        tileLine.Add(_tiles.GetTile(startPoint.x, startPoint.y));
        int countOfIteraion = _tiles.Size - Mathf.Abs(iteration);
        for (int i = 1; i < countOfIteraion; i++)
        {
            Vector2Int tilePosition = startPoint - i * direction;
            tileLine.Add(_tiles.GetTile(tilePosition.x, tilePosition.y));
        }
        CheckLine(tileLine);
    }

    private void CheckLine(List<Tile> line)
    {
        int count = line.Count;
        List<Tile> wasMultiplied = new List<Tile>();
        for (int i = 1; i < count; i++)
        {
            int value = line[i].Value;
            if (value == 0)
                continue;
            TryToMultuply(value, i, line, wasMultiplied);
        }

        ClearMultiplyInAllTiles();
        foreach (var tile in wasMultiplied)
        {
            tile.ClearMultiply();
        }
    }

    private void ClearMultiplyInAllTiles()
    {
        List<Tile> tiles = _tiles.GetAllTiles();
        foreach (var tile in tiles)
            tile.ClearMultiply();
    }

    private void TryToMultuply(int value, int i, List<Tile> line, List<Tile> wasMultiplied)
    {
        Tile empty = null;

        for (int j = i - 1; j >= 0; j--)
        {
            Tile tile = line[j];
            if (tile.WasMultiplied)
                break;

            if (tile.IsEmpty)
            {
                empty = tile;
                continue;
            }
            if (tile.Value == value)
            {
                tile.MultiplyWith(line[i]);
                wasMultiplied.Add(tile);
                _isAnyMove = true;
                return;
            }
            break;
        }

        if (empty != null)
        {
            empty.SetInEmpty(line[i]);
            _isAnyMove = true;
        }
    }

    private void OnEnable()
    {
        _fieldController.OnGameFieldCreated += GameFieldCreated;
        _inputController.OnMove += TryMove;
    }

    private void OnDisable()
    {
        _fieldController.OnGameFieldCreated -= GameFieldCreated;
        _inputController.OnMove -= TryMove;
    }
}
