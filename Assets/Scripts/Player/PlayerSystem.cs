using System;
using System.Collections.Generic;
using Core.Services.Updater;
using InputReader;
using NPC.Controller;
using UI;

namespace Player
{
    public class PlayerSystem : IDisposable
    {
        private readonly ProjectUpdater _projectUpdater;
        public PlayerBrain PlayerBrain { get; }


        public PlayerSystem(UIContext uiContext, PlayerEntity playerEntity, List<IEntityInputSource> inputSources)
        {
            PlayerBrain = new PlayerBrain(uiContext, playerEntity, inputSources);
        }

        public void Dispose()
        {
            PlayerBrain?.Dispose();
        }
    }
}