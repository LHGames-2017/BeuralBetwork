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




            string action;
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
            int minDistance = Integer.MaxValue;

            for (int x = 0; x < tiles.length; x++)
            {
                for (int x = 0; x < tiles.length; x++)
                {
                    Tile currentTile = tiles[x, y];
                    int distance = Math.Abs(current.x - gameInfo.Player.Position.x) + Math.Abs(current.y - gameInfo.Player.Position.y);
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
            return (new Point(destination.x, destination, y));
        }
        Point GoToDestination(Point destination)
        {
            int dx = current.x - gameInfo.Player.Position.x;
            int dy = current.y - gameInfo.Player.Position.y;
            Point nextPoint;

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
