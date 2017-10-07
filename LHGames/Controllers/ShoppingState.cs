using StarterProject.Web.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


public class ShoppingState : State
{
    bool hasPick, hasPack;
    int levelSpeed, levelCarry;

    public override string ReturnAction(GameInfo gameInfo)
    {
        string action = "";
        if(AI.encercle)
        {
            //pathfinding maison
            if (GameController.IsSameTile(gameInfo.Player.HouseLocation))
                action = BuyStats(gameInfo);
            else
            {
                Point nextPoint = GameController.GoToDestination(gameInfo.Player.HouseLocation,gameInfo);
                action = AIHelper.CreateMoveAction(nextPoint); //next point
            }
        }
        else
        {
            Tile closest = GameController.FindClosest(TileContent.Shop, GameController.carte);

            if (GameController.IsNearTile(closest))
                action = BuyItem(gameInfo);
            else
            {
                Point nextPoint = GameController.GoToDestination(closest, gameInfo);
                action = AIHelper.CreateMoveAction(new Point()); //next point
             }
        }
        return action;
    }

    string BuyStats(GameInfo gameInfo)
    {
        string action = "";
        
        if(levelSpeed == 0)
        {
            action = AIHelper.CreateUpgradeAction(UpgradeType.CollectingSpeed);
            levelSpeed++;
        }
        else if(gameInfo.Player.CarryingCapacity == 1000)
        {
            action = AIHelper.CreateUpgradeAction(UpgradeType.CarryingCapacity);
            levelCarry++;
        }
        else if (gameInfo.Player.CarryingCapacity == 1500)
        {
            action = AIHelper.CreateUpgradeAction(UpgradeType.CarryingCapacity);

            levelCarry++;
        }
       
        return action;
    }
    string BuyItem(GameInfo gameInfo)
    {    
        string action = "";
        if(!hasPick)
        {
            action = AIHelper.CreatePurchaseAction(PurchasableItem.DevolutionsPickaxe);
            hasPick = true;
        }
        else if (!hasPack)
        {
            action = AIHelper.CreatePurchaseAction(PurchasableItem.DevolutionsBackpack);
            hasPack = true;
        }
        return action;
    }
              
     
}

