namespace StarterProject.Web.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;

    [Route("/")]
    public class GameController : Controller
    {
        AIHelper player = new AIHelper();
        GameInfo gameInfo;
        bool goingHome = false;

        [HttpPost]
        public string Index([FromForm]string map)
        {
            gameInfo = JsonConvert.DeserializeObject<GameInfo>(map);
            var carte = AIHelper.DeserializeMap(gameInfo.CustomSerializedMap);

            // INSERT AI CODE HERE.

            Tile closestResource = FindClosest(TileContent.Resource, carte);
            Point nextPoint = GoToDestination(closestResource);
            string action = AIHelper.CreateMoveAction(nextPoint);

            if(IsNearTile(closestResource))
            {
                if (gameInfo.Player.CarriedResources < gameInfo.Player.CarryingCapacity)
                {
                    action = AIHelper.CreateCollectAction(nextPoint);
                }
                else
                {
                    goingHome = false;
                }
            }
            else
            {
               action = AIHelper.CreateMoveAction(nextPoint);
            }

            if(goingHome)
            {
                nextPoint = GoToDestination(gameInfo.Player.HouseLocation);
                if (IsSameTile(gameInfo.Player.HouseLocation))
                {
                    goingHome = false;
                }
                else
                {
                    action = AIHelper.CreateMoveAction(nextPoint);
                }
            }

            return action;
        }

        bool IsSameTile(Point tile)
        {
            if (gameInfo.Player.Position.X == tile.X && gameInfo.Player.Position.Y == tile.Y)
                return true;
            else
                return false;
        }

        bool IsSameTile(Tile tile)
        {
            if (gameInfo.Player.Position.X == tile.X && gameInfo.Player.Position.Y == tile.Y)
                return true;
            else
                return false;
        }

        bool IsNearTile(Tile tile)
        {
            if (Math.Abs(gameInfo.Player.Position.X - tile.X) <= 1 && Math.Abs(gameInfo.Player.Position.Y - tile.Y) <= 1)
                return true;

            return false;
        }

        Tile FindClosest(TileContent content, Tile[,] tiles)
        {
            Tile closestTile = tiles[0, 0];
            int minDistance = int.MaxValue;

            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                for (int y = 0; y < tiles.GetLength(1); x++)
                {
                    Tile currentTile = tiles[x, y];
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

        Point GoToDestination(Tile destination)
        {
            return (new Point(destination.X, destination.Y));
        }
        Point GoToDestination(Point destination)
        {
            int dx = destination.X - gameInfo.Player.Position.X;
            int dy = destination.Y - gameInfo.Player.Position.Y;
            Point nextPoint = new Point(0,0);

            if (dx > 0)
            {
               
            }
            else if (dx < 0)
            {

            }
            if (dy > 0)
            {

            }
            else if (dy < 0)
            {

            }
            return nextPoint;
        }

    }
}
