using Assets.Scripts.Events;
using Caliburn.Micro;
using DarkCluster.Core;
using DarkCluster.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Services
{
    // Responds to game start event and does any work to set up the world.
    public class CreateUniverseService : IService, IHandle<GameStartedEvent>
    {
        public void Handle(GameStartedEvent message)
        {
            Util.GetEventAggregator().Publish(new SpawnObjectEvent() { ObjectName = "StarShip" });
        }
    }
}
