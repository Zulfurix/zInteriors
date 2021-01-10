using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace zInteriors_Client
{
    class InteriorHandler : BaseScript
    {

        private static Interior tempInterior;

        private static List<dynamic> interiors;

        public InteriorHandler()
        {
            EventHandlers["onClientResourceStart"] += new Action<string>(OnClientResourceStart);
            EventHandlers["zInteriors:getInteriors"] += new Action<List<dynamic>>(GetInteriorsFromServer);
        }

        private void OnClientResourceStart(string resourceName)
        {
            if (GetCurrentResourceName() != resourceName) return;

            RequestInteriorsFromServer();
        }

        private void RequestInteriorsFromServer()
        {
            TriggerServerEvent("zInteriors:getInteriors");
        }

        private void GetInteriorsFromServer(List<dynamic> parsedInteriors)
        {
            if (parsedInteriors.Count > 0)
            {
                interiors = new List<dynamic>(parsedInteriors);
                Debug.WriteLine(String.Format("Loaded {0} interiors", interiors.Count));
                TriggerEvent("zInteriors:drawInteriorMarkers");
            }
        }

        public static Interior TempInterior
        {
            get { return tempInterior; }
            set { tempInterior = value; }
        }

        public static List<dynamic> Interiors
        {
            get { return interiors; }
        }
    }
}
