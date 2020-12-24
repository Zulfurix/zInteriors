using System;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace zInteriors_Client
{
    class TextInput : BaseScript
    {
        public static async System.Threading.Tasks.Task<string> GetInputFromOnScreenInputAsync()
        {
            DisplayOnscreenKeyboard(1, "Enter interior name", "", "", "", "", "", 64);
            while (UpdateOnscreenKeyboard() == 0)
            {
                await Delay(0);
                DisableAllControlActions(0);
            }

            if (string.IsNullOrEmpty(GetOnscreenKeyboardResult())) return "Interior";

            return GetOnscreenKeyboardResult();
        }
    }
}
