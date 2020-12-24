using System;
using System.Collections.Generic;
using CitizenFX.Core;
using Newtonsoft.Json;
using static CitizenFX.Core.Native.API;

namespace zInteriors_Client
{
    class CommandHandler : BaseScript
    {
        public CommandHandler()
        {
            EventHandlers["onClientResourceStart"] += new Action<string>(OnClientResourceStart);
        }

        private void OnClientResourceStart(string resourceName)
        {
            if (GetCurrentResourceName() != resourceName) return;


            //----------Create Interior Command----------//

            RegisterCommand("createinterior", new Action<int, List<object>, string>((source, args, raw) =>
            {

                if (InteriorCreationState.PlayerCreationState == InteriorCreationState.State.NOT_CREATING_INTERIOR)
                {
                    TriggerEvent("chat:addMessage", new
                    {
                        color = new[] { 0, 255, 0 },
                        args = new[] { "[zInteriors]", $"Type /place to place down the entrance marker" }
                    });

                    InteriorCreationState.PlayerCreationState = InteriorCreationState.State.PLACING_ENTRANCE;
                    TriggerEvent("zInteriors:drawFollowingMarker");
                }
                else
                {
                    TriggerEvent("chat:addMessage", new
                    {
                        color = new[] { 255, 0, 0 },
                        args = new[] { "[zInteriors]", $"You are already creating an interior" }
                    });
                }
            }), false);


            //----------Cancel Command----------//

            RegisterCommand("cancel", new Action<int, List<object>, string>((source, args, raw) =>
            {

                if (InteriorCreationState.PlayerCreationState == InteriorCreationState.State.PLACING_ENTRANCE
                || InteriorCreationState.PlayerCreationState == InteriorCreationState.State.PLACING_EXIT)
                {
                    TriggerEvent("chat:addMessage", new
                    {
                        color = new[] { 0, 255, 0 },
                        args = new[] { "[zInteriors]", $"Interior creation cancelled" }
                    });

                    InteriorCreationState.PlayerCreationState = InteriorCreationState.State.NOT_CREATING_INTERIOR;
                    TriggerEvent("zInteriors:undrawFollowingMarker");
                    TriggerEvent("zInteriors:undrawTemporaryMarkers");
                }
                else
                {
                    TriggerEvent("chat:addMessage", new
                    {
                        color = new[] { 255, 0, 0 },
                        args = new[] { "[zInteriors]", $"You aren't creating an interior" }
                    });
                }
            }), false);


            //----------Place Command----------//

            RegisterCommand("place", new Action<int, List<object>, string>(async (source, args, raw) =>
            {

                if (InteriorCreationState.PlayerCreationState == InteriorCreationState.State.PLACING_ENTRANCE)
                {
                    TriggerEvent("chat:addMessage", new
                    {
                        color = new[] { 0, 255, 0 },
                        args = new[] { "[zInteriors]", $"Entrance placed" }
                    });

                    InteriorHandler.TempInterior = new Interior("Interior", Game.PlayerPed.Position + new Vector3(0, 0, -1), Vector3.Zero);
                    InteriorCreationState.PlayerCreationState = InteriorCreationState.State.PLACING_EXIT;
                    
                    TriggerEvent("zInteriors:drawTemporaryMarkers");
                }
                else if (InteriorCreationState.PlayerCreationState == InteriorCreationState.State.PLACING_EXIT)
                {
                    TriggerEvent("chat:addMessage", new
                    {
                        color = new[] { 0, 255, 0 },
                        args = new[] { "[zInteriors]", $"Exit placed" }
                    });

                    InteriorHandler.TempInterior.Exit = Game.PlayerPed.Position + new Vector3(0, 0, -1);
                    InteriorHandler.TempInterior.Name = await TextInput.GetInputFromOnScreenInputAsync();

                    TriggerEvent("chat:addMessage", new
                    {
                        color = new[] { 0, 255, 0 },
                        args = new[] { "[zInteriors]", $"Interior created: " + InteriorHandler.TempInterior.Name }
                    });

                    InteriorCreationState.PlayerCreationState = InteriorCreationState.State.NOT_CREATING_INTERIOR;

                    TriggerEvent("zInteriors:undrawFollowingMarker");
                    TriggerEvent("zInteriors:undrawTemporaryMarkers");

                    TriggerServerEvent("zInteriors:serializeInterior", JsonConvert.SerializeObject(InteriorHandler.TempInterior));
                }
                else
                {
                    TriggerEvent("chat:addMessage", new
                    {
                        color = new[] { 255, 0, 0 },
                        args = new[] { "[zInteriors]", $"You aren't creating an interior" }
                    });
                }
            }), false);
        }
    }
}
