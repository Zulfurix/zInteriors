using System;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace zInteriors_Server
{
    class Interior
    {
        private string name;
        private Vector3 entrance;
        private Vector3 exit;

        public Interior(string name, Vector3 entrance, Vector3 exit)
        {
            this.name = name;
            this.entrance = entrance;
            this.exit = exit;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public Vector3 Entrance
        {
            get { return entrance; }
            set { entrance = value; }
        }

        public Vector3 Exit
        {
            get { return exit; }
            set { exit = value; }
        }
    }
}
