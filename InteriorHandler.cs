using System;
using System.Collections.Generic;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace zInteriors_Client
{
    class InteriorHandler : BaseScript
    {

        private static Interior tempInterior;

        public static Interior TempInterior
        {
            get { return tempInterior; }
            set { tempInterior = value; }
        }
    }
}
