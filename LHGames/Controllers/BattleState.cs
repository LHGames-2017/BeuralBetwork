using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StarterProject.Web.Api;

public class BattleState : State
{
    public override string ReturnAction(GameInfo gameInfo)
    {
        string action = "";

        Tile closestPlayer = GameController.FindClosest(TileContent.Player, GameController.carte);

        //path finder closestResources;

        if (GameController.IsNearTile(closestPlayer))
            action = AIHelper.CreateAttackAction(new Point(closestPlayer.X, closestPlayer.Y));
        else
            action = AIHelper.CreateMoveAction(
                GameController.GoToDestination(
                   closestPlayer,
                    gameInfo
                ));

        return action;
    }
}
