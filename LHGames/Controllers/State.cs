using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StarterProject.Web.Api;

public abstract class State
{
    public abstract string ReturnAction(GameInfo gameInfo);
}
