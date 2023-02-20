using UnityEngine;
using System;
using System.Collections.Generic;

public class FieldController : MonoBehaviour
{
    public event Action<HexMassive> OnGameFieldCreated;
    public event Action<HexMassive> OnGameFieldDeleted;
    public event Action OnWin;
    public event Action OnLose;

    [SerializeField] private Game _game;
    [SerializeField] private FieldGenerator _generator;
    [SerializeField] private TilesMover _tilesMover;

    public HexMassive Tiles { get; private set; }

    public void Awake()
    {
        if (_game == null)
            throw new NullReferenceException();
        if (_generator == null)
            throw new NullReferenceException();
        if (_tilesMover == null)
            throw new NullReferenceException();
    }

    private void GameStart(int size) => InitializeNewField(size);

    private void GameRestart()
    {
        Tiles.Clear();
        MakeDefaultSituation();
    }

    private void Exit() => DeleteGameField();

    private void InitializeNewField(int size)
    {
        Tiles = _generator.GenerateGameField(size);
        SubscribeOnTiles();
        FakeTilesPool.instant.CreatePool(size);
        MakeDefaultSituation();
        OnGameFieldCreated?.Invoke(Tiles);
    }

    private void MakeDefaultSituation()
    {
        TrySpawnTile();
        TrySpawnTile();
    }

    private void TrySpawnTile() 
    {
        if (!RandomTileSpawner.TrySpawnTile(Tiles))
            OnLose?.Invoke();
    } 

    private void DeleteGameField()
    {
        OnGameFieldDeleted?.Invoke(Tiles);
        UnsubscribeOnTiles();
        _generator.DeleteGameField();
    }

    private void NextTurn() => TrySpawnTile();

    private void Win() => OnWin?.Invoke();

    private void SubscribeOnTiles()
    {
        List<Tile> tiles = Tiles.GetAllTiles();
        foreach (var tile in tiles)
            tile.OnWin += Win;
    }

    private void UnsubscribeOnTiles()
    {
        List<Tile> tiles = Tiles.GetAllTiles();
        foreach (var tile in tiles)
            tile.OnWin -= Win;
    }

    private void OnEnable()
    {
        _game.OnGameStart += GameStart;
        _game.OnExit += Exit;
        _game.OnGameRestart += GameRestart;
        _tilesMover.OnNextTurn += NextTurn;
    } 

    private void OnDisable()
    {
        _game.OnGameStart -= GameStart;
        _game.OnExit -= Exit;
        _game.OnGameRestart -= GameRestart;
        _tilesMover.OnNextTurn -= NextTurn;
    }
}
