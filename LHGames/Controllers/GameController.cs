
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
        // INSERT AI CODE HERE.
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

        if (Math.Abs(dx) <= Math.Abs(dy))
        {
                if (dy < 0)
                {
                    nextPoint = new Point(gameInfo.Player.Position.X, gameInfo.Player.Position.Y + 1);
                }
            else if (dy > 0)
            {
                    nextPoint = new Point(gameInfo.Player.Position.X, gameInfo.Player.Position.Y - 1);
            }
        }
        else
        {
            if (dx < 0)
            {
                nextPoint = new Point(gameInfo.Player.Position.X + 1, gameInfo.Player.Position.Y);
            }
            else if (dx > 0)
            {
                nextPoint = new Point(gameInfo.Player.Position.X - 1, gameInfo.Player.Position.Y);
            }
        }
        return nextPoint;
    }

}
