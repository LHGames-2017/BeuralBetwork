using StarterProject.Web.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


public class MiningState : State
{
    bool goingHome = false;
    public override string ReturnAction(GameInfo gameInfo)
    {
        if (gameInfo.Player.CarriedResources >= gameInfo.Player.CarryingCapacity)
            goingHome = true;

        Tile closestResource = GameController.FindClosest(TileContent.Resource, GameController.carte);
        Point nextPoint = (goingHome) ? GameController.GoToDestination(gameInfo.Player.HouseLocation, gameInfo) : GameController.GoToDestination(closestResource, gameInfo);
        string action = AIHelper.CreateMoveAction(nextPoint);



        if (GameController.IsNearTile(closestResource) && !goingHome)
        {
            if (gameInfo.Player.CarriedResources < gameInfo.Player.CarryingCapacity)
            {
                action = AIHelper.CreateCollectAction(nextPoint);
            }
        }

        if (goingHome)
        {
            if (GameController.IsSameTile(gameInfo.Player.HouseLocation))
            {
                goingHome = false;
            }
        }
        return action;
    }
}
