using System.Collections.Generic;
using UnityEngine;
using System;

public class FieldGenerator : MonoBehaviour
{
    [SerializeField] private float _backgroundSizeMultiply;
    [SerializeField] private float _spacing;

    [SerializeField] private Tile _tilePrefab;
    [SerializeField]private Background _tileBackgroundPrefab;
    [SerializeField]private Transform _tilesContainer;
    [SerializeField]private Transform _backgroundContainer;

    private HexMassive _tiles;

    private void Awake()
    {
        if (_tilePrefab == null)
            throw new NullReferenceException();

        if (_tileBackgroundPrefab == null)
            throw new NullReferenceException();

        if (_tilesContainer == null)
            throw new NullReferenceException();

        if (_backgroundContainer == null)
            throw new NullReferenceException();
    }

    public HexMassive GenerateGameField(int width)
    {
        InitializeMassive(width);

        Vector2 tileSize = CountTileSize(width, _spacing);
        List<Vector2> tilesPositions = CountTilesPosition(width, tileSize);
        GenerateTiles(tilesPositions, tileSize);
        GenerateFieldBackground(tilesPositions, tileSize);
        return _tiles;
    }

    public void DeleteGameField()
    {
        foreach (Transform child in _tilesContainer)
            Destroy(child.gameObject);
        foreach(Transform child in _backgroundContainer)
            Destroy(child.gameObject);
    }

    private void InitializeMassive(int width)
    {
        _tiles = new HexMassive(width);
    }

    private Vector2 CountTileSize(int width, float spacing)
    {
        Vector2 scale = transform.GetComponent<RectTransform>().sizeDelta;
        float tileSizeY = (scale.x - spacing * (width + 1)) / width;
        float tileSizeX = scale.x * tileSizeY / scale.y;
        return new Vector2(tileSizeX, tileSizeY);
    }

    private List<Vector2> CountTilesPosition(int width, Vector2 tileSize)
    {
        List<Vector2> tilesPositions = new List<Vector2>();
        float roundedHalf = (width - 1) / 2;
        for (float i = roundedHalf; i >= -roundedHalf; i--)
        {
            float y = (tileSize.x) * i;

            for (float j = roundedHalf - Mathf.Abs(i / 2); j >= -roundedHalf + Mathf.Abs(i / 2); j--)
            {
                float x = (tileSize.y + _spacing) * j;
                tilesPositions.Add(new Vector2(y, x));
            }
        }
        return tilesPositions;
    }

    private void GenerateTiles(List<Vector2> tilesPositions, Vector2 tileSize)
    {
        int i = 0;
        int j = 0;

        int length = _tiles.GetLength(i);

        foreach (var tilesPosition in tilesPositions)
        {
            var tile = Instantiate(_tilePrefab, _tilesContainer);
            tile.GetComponent<RectTransform>().sizeDelta = tileSize;
            tile.transform.localPosition = tilesPosition;

            if (j >= length)
            {
                j = 0;
                i++;
                length = _tiles.GetLength(i);
            }
            _tiles.SetTile(i, j, tile);
            j++;
        }
    }

    private void GenerateFieldBackground(List<Vector2> tilesPositions, Vector2 tileSize)
    {
        foreach (var tilePosition in tilesPositions)
        {
            var tile = Instantiate(_tileBackgroundPrefab, _backgroundContainer);
            tile.GetComponent<RectTransform>().sizeDelta = tileSize * _backgroundSizeMultiply;
            tile.transform.localPosition = tilePosition;
        }
    }
}
