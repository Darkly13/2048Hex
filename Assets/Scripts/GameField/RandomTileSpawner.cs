using System.Collections.Generic;
using UnityEngine;

public static class RandomTileSpawner
{
    public static bool TrySpawnTile(HexMassive tiles)
    {
        List<Tile> freeList = tiles.GetFreeTiles();
        Tile tile = ChoseFreeTile(freeList);
        int rand = Random.Range(0, 10);
        tile.Create(rand == 0 ? 2 : 1);

        if (freeList.Count <= 1)
        {
            if (CheckGameOver(tiles))
                return false;
        }
        return true;
    }

    private static Tile ChoseFreeTile(List<Tile> free)
    {
        int tile_index = Random.Range(0, free.Count);
        return free[tile_index];
    }

    private static bool CheckGameOver(HexMassive tiles)
    {
        List<Tile> allTiles = tiles.GetAllTiles();
        foreach (var tile in allTiles)
        {
            int value = tile.Value;
            List<Tile> neighbours = tiles.GetNeighbours(tile);
            foreach (var neoghbour in neighbours)
            {
                if (value == neoghbour.Value)
                {
                    return false;
                }
            }
        }
        return true;
    }
}
