using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using static CitizenFX.Core.Native.API;

namespace zInteriors_Client
{
    class TeleportHandler : BaseScript
    {
        private readonly float ACTIVATION_DISTANCE = 1.2f;

        private Vector3 teleportDestination;
        private bool isTeleportPromptDisplayed = false;

        public TeleportHandler()
        {
            EventHandlers["zInteriors:initTeleportHandler"] += new Action(InitTeleportHandler);
            Tick += InputHandlerTick;
        }

        private void InitTeleportHandler()
        {
            Tick += TeleportHandlerTick;
        }

        private async Task TeleportHandlerTick()
        {
            foreach (dynamic interior in InteriorHandler.Interiors)
            {
                if (Game.PlayerPed.IsInRangeOf(interior.Entrance, ACTIVATION_DISTANCE))
                {
                    Screen.DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to enter " + interior.Name);
                    teleportDestination = interior.Exit;
                    isTeleportPromptDisplayed = true;
                }
                else if (Game.PlayerPed.IsInRangeOf(interior.Exit, ACTIVATION_DISTANCE))
                {
                    Screen.DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to exit " + interior.Name);
                    teleportDestination = interior.Entrance;
                    isTeleportPromptDisplayed = true;
                }
                else
                {
                    isTeleportPromptDisplayed = false;
                }
            }

            await Delay(0);
        }

        private async Task InputHandlerTick()
        {
            if (isTeleportPromptDisplayed)
            {
                if (Game.IsControlJustPressed(0, Control.Context))
                {
                    if (teleportDestination != null)
                    {
                        Game.PlayerPed.Position = teleportDestination;
                    }
                }
            }
        }
    }
}
