
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StarterProject.Web.Api;

[Route("/")]
public class GameController : Controller
{
    AIHelper player = new AIHelper();
    AI ai = new AI();

    public static GameInfo gameInfo;
    public static Tile[,] carte;

    [HttpPost]
    public string Index([FromForm]string map)
    {
        GameController.gameInfo = JsonConvert.DeserializeObject<GameInfo>(map);
        carte = AIHelper.DeserializeMap(gameInfo.CustomSerializedMap);

        string action = ai.ReturnAction(gameInfo);
     
        Console.WriteLine(action);
        return action;
    }

    public static bool IsSameTile(Point tile)
    {
        if (gameInfo.Player.Position.X == tile.X && gameInfo.Player.Position.Y == tile.Y)
            return true;
        else
            return false;
    }

    public static bool IsSameTile(Tile tile)
    {
        if (gameInfo.Player.Position.X == tile.X && gameInfo.Player.Position.Y == tile.Y)
            return true;
        else
            return false;
    }

    public static bool IsNearTile(Tile tile)
    {
        if (Math.Abs(gameInfo.Player.Position.X - tile.X) == 1 && Math.Abs(gameInfo.Player.Position.Y - tile.Y) == 0)
            return true;
        else if (Math.Abs(gameInfo.Player.Position.X - tile.X) == 0 && Math.Abs(gameInfo.Player.Position.Y - tile.Y) == 1)
            return true;

        return false;
    }

    public static Tile FindClosest(TileContent content, Tile[,] tiles)
    {
        Tile closestTile = tiles[0, 0];
        int minDistance = int.MaxValue;

        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                Tile currentTile = tiles[x, y];
                if ((byte)content != currentTile.C)
                    continue;

                int distance = Math.Abs(currentTile.X - gameInfo.Player.Position.X) + Math.Abs(currentTile.Y - gameInfo.Player.Position.Y);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestTile = currentTile;
                }
            }
            
        }
        return closestTile;
    }

    public static Point GoToDestination(Tile destination, GameInfo gameInfo)
    {
        return (GoToDestination(new Point(destination.X, destination.Y), gameInfo));
    }
    public static Point GoToDestination(Point destination, GameInfo gameInfo)
    {
        int dx = gameInfo.Player.Position.X - destination.X;
        int dy = gameInfo.Player.Position.Y - destination.Y;
        Point nextPoint = gameInfo.Player.Position;

        if (dx < 0)
        {
            nextPoint = new Point(gameInfo.Player.Position.X + 1, gameInfo.Player.Position.Y);
        }
        else if (dx > 0)
        {
            nextPoint = new Point(gameInfo.Player.Position.X - 1, gameInfo.Player.Position.Y);
        }
        else if (dy < 0)
        {
            nextPoint = new Point(gameInfo.Player.Position.X, gameInfo.Player.Position.Y + 1);
        }
        else if (dy > 0)
        {
            nextPoint = new Point(gameInfo.Player.Position.X, gameInfo.Player.Position.Y - 1);
        }

        return nextPoint;
    }

    static ScoredTileList GetAdjacentTiles(Point destination, Tile[,] map, int iX, int iY, ScoredTile previous)
    {
        ScoredTileList list = new ScoredTileList();
        if ((TileContent)map[iX + 1, iY].C == TileContent.Empty)
        {
            int distanceTileUp = destination.X - map[iX + 1, iY].X + destination.Y - map[iX + 1, iY].Y;
            ScoredTile sTile = new ScoredTile(map[iX + 1, iY], distanceTileUp);
            sTile.previous = previous;
            list.Add(sTile);
        }
        else if ((TileContent)map[iX, iY - 1].C == TileContent.Empty)
        {
            int distanceTileLeft = destination.X - map[iX, iY - 1].X + destination.Y - map[iX, iY - 1].Y;
            ScoredTile sTile2 = new ScoredTile(map[iX, iY - 1], distanceTileLeft);
            sTile2.previous = previous;
            list.Add(sTile2);
        }
        else if ((TileContent)map[iX - 1, iY].C == TileContent.Empty)
        {
            int distanceTileDown = destination.X - map[iX - 1, iY].X + destination.Y - map[iX - 1, iY].Y;
            ScoredTile sTile3 = new ScoredTile(map[iX - 1, iY], distanceTileDown);
            sTile3.previous = previous;
            list.Add(sTile3);
        }
        else if ((TileContent)map[iX, iY + 1].C == TileContent.Empty)
        {
            int distanceTileRight = destination.X - map[iX, iY + 1].X + destination.Y - map[iX, iY + 1].Y;
            ScoredTile sTile4 = new ScoredTile(map[iX, iY + 1], distanceTileRight);
            sTile4.previous = previous;
            list.Add(sTile4);
        }

        return list;
    }

    public static List<Tile> GetPath(Point destination, Tile[,] map)
    {
        int dx = gameInfo.Player.Position.X - destination.X;
        int dy = gameInfo.Player.Position.Y - destination.Y;
        int iX = 0;
        int iY = 0;
        Point nextPoint = gameInfo.Player.Position;
        List<Tile> returnedList = new List<Tile>();
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if (map[i, j].X == gameInfo.Player.Position.X && map[i, j].Y == gameInfo.Player.Position.Y)
                {
                    iX = i;
                    iY = j;
                }
            }
        }
        ScoredTileList tiles = new ScoredTileList();
        List<ScoredTile> closedNodes = new List<ScoredTile>();
        tiles.Add(new ScoredTile(map[iX, iY], 0));
        int t = 100;
        while (tiles.Count != 0 && --t > 0)
        {
            tiles.SortList();
            ScoredTile n = tiles.Pop();
            if (n.tile.X == destination.X && n.tile.Y == destination.Y)
                return RetracePath(tiles);

            ScoredTileList l = GetAdjacentTiles(destination, map, iX, iY, n);
            l.SortList();
            foreach (ScoredTile sT in l)
            {
                tiles.Add(sT);
            }
        }
        return RetracePath(tiles);
    }

    public static List<Tile> RetracePath(ScoredTileList tiles)
    {
        List<Tile> rTiles = new List<Tile>();
        ScoredTile currentTile = tiles[tiles.Count - 1];
        rTiles.Add(currentTile.tile);
        while (currentTile.previous != null)
        {
            rTiles.Add(currentTile.previous.tile);
            currentTile = currentTile.previous;
        }
        return rTiles;
    }
    public class ScoredTileList : List<ScoredTile>
    {
        public ScoredTile Pop()
        {
            ScoredTile tile = this[0];
            this.RemoveAt(0);
            return tile;
        }

        public void SortList()
        {
            this.Sort(delegate (ScoredTile x, ScoredTile y)
            {
                if (x.score == y.score) return 0;
                else if (x.score > y.score) return -1;
                else return 1;
            });
        }
    }

    public class ScoredTile
    {
        public Tile tile;
        public int score;
        public ScoredTile previous;

        public ScoredTile(Tile tile, int score)
        {
            this.tile = tile;
            this.score = score;
        }
    }
}
