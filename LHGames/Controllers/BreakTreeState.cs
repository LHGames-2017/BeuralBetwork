﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StarterProject.Web.Api;

public class BreakTreeState : State
{
    public override string ReturnAction(GameInfo gameInfo)
    {
        string action = "";

        Tile closestResource = GameController.FindClosest(TileContent.Wall, GameController.carte);

        //path finder closestResources;

        if (GameController.IsNearTile(closestResource))
            action = AIHelper.CreateAttackAction(new Point(closestResource.X, closestResource.Y));
        else
            action = AIHelper.CreateMoveAction(
                GameController.GoToDestination(
                   closestResource,
                    gameInfo
                ));

        return action;
    }
}

