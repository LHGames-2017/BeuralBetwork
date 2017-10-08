using StarterProject.Web.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


 
public class AI
{
    State currentState;
    MiningState miningState = new MiningState();
    ShoppingState shoppingState = new ShoppingState();
    BreakTreeState breakTreeState = new BreakTreeState();
    BattleState battleState = new BattleState();

    public static bool encercle = true;

    public string ReturnAction(GameInfo gameInfo)
    {
        if (gameInfo.Player.totalResources >= 50000)
        {
            Console.WriteLine("SHOPPING");
            currentState = shoppingState;
            if (gameInfo.Player.Position == gameInfo.Player.HouseLocation)
                return AIHelper.CreateMoveAction(
                    new Point(gameInfo.Player.Position.X+1, gameInfo.Player.Position.Y)
                    );
        }
        else if(PlayerIsClose(gameInfo))
        {
            currentState = battleState;
        }
        else
        {
            Console.WriteLine("MINING");
            currentState = miningState;
        }
        if(false /* to do */)
        {
            currentState = breakTreeState;
        }
          
        return currentState.ReturnAction(gameInfo);
    }

    bool PlayerIsClose(GameInfo gameInfo)
    {
        Tile closestPlayer = GameController.FindClosest(TileContent.Player, GameController.carte);

        if(Math.Abs(closestPlayer.X - gameInfo.Player.Position.X ) < 5 && Math.Abs(closestPlayer.Y - gameInfo.Player.Position.Y) < 5)
            return true;

        return false;
    }
}

