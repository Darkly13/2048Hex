using System.Collections.Generic;
using UnityEngine;

public class HexMassive
{
    public Tile[][] Massive { get; private set; }
    public int Size { get; private set; }

    private int _roundedHalf;
    
    public HexMassive(int size)
    {
        Size = size;
        _roundedHalf = (size - 1) / 2;
        Massive = new Tile[size][];
        for (int i = -_roundedHalf; i <= _roundedHalf; i++)      
            Massive[i + _roundedHalf] = new Tile[size - Mathf.Abs(i)];      
    }

    public void InitializeLoadedValues(List<int> values)
    {
        int index = 0;
        foreach(var line in Massive)
        {
            foreach(var tile in line)
            {
                tile.SetValue(values[index]);
                index++;
            }
        }
    }

    public void SetTile(int x, int y, Tile value)
    {
        Massive[x][y] = value;

        int hexY = _roundedHalf - x;
        int edge = Massive[x].Length - 1;
        int hexX = y * 2 - edge;

        value.SetXY(hexX, hexY);
    }

    public Tile GetTile(int hexX, int hexY)
    {
        int x = _roundedHalf - hexY;
        int edge = Massive[x].Length - 1;
        int y = (hexX + edge) / 2;
        return Massive[x][y];
    }

    public int GetLength(int row) => Massive[row].Length;

    public List<Tile> GetAllTiles()
    {
        List<Tile> array = new List<Tile>();
        foreach (var row in Massive)
        {
            foreach (var tile in row)
            {
                array.Add(tile);
            }
        }
        return array;
    }

    public List<Tile> GetFreeTiles()
    {
        List<Tile> free = new List<Tile>();
        List<Tile> allTiles = GetAllTiles();
        foreach (var tile in allTiles)
        {
            if (tile.IsEmpty)
                free.Add(tile);
        }
        return free;
    }

    public List<Tile> GetNeighbours(Tile tile)
    {
        List<Tile> neighbours = new List<Tile>();
        int roundedHalf = _roundedHalf;
        Vector2Int startPosition = new Vector2Int(tile.X, tile.Y);

        for(int y = -1; y<=0; y++)
        {
            if (tile.Y+y < -roundedHalf)
                continue;

            int rowLength = GetLength(Mathf.Abs(tile.Y +y-roundedHalf)) - 1;
            for (int x =-1; x<=1; x+=2)
            {
                Vector2Int position = startPosition + new Vector2Int(x, y);
                if (y == 0)
                {
                    if (x == -1)
                        continue;

                    position.x = startPosition.x + x * 2;
                }
                    
                if (position.x > rowLength || position.x < -rowLength)
                    continue;

                neighbours.Add(GetTile(position.x, position.y));
            }
        }
        return neighbours;
    }

    public void Clear()
    {
        List<Tile> tiles = GetAllTiles();
        foreach (var tile in tiles)
            tile.Clear();
    }
}
