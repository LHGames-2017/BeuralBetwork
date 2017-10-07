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


    public static bool encercle = true;

    public string ReturnAction(GameInfo gameInfo)
    {
        if (gameInfo.Player.totalResources >= 15000)
        {
            
            Console.WriteLine("SHOPPING");
            currentState = shoppingState;
            if (gameInfo.Player.Position == gameInfo.Player.HouseLocation)
                return AIHelper.CreateMoveAction(
                    new Point(gameInfo.Player.Position.X, gameInfo.Player.Position.Y)
                    );
        }
        else
        {
            Console.WriteLine("MINING");
            currentState = miningState;
        }
        if(false)
        {
            currentState = breakTreeState;
        }
          
        return currentState.ReturnAction(gameInfo);
    }
}

