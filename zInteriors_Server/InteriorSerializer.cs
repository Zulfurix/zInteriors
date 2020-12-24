using System;
using Newtonsoft.Json;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using System.IO;
using Newtonsoft.Json.Linq;

namespace zInteriors_Server
{
    public class InteriorSerializer : BaseScript
    {
        private static string resourceName = GetCurrentResourceName();

        public InteriorSerializer()
        {
            EventHandlers["zInteriors:serializeInterior"] += new Action<string>(SerializeInterior);
        }

        private void SerializeInterior(string interiorJson)
        {
            JObject interiorJsonObject = (JObject)JsonConvert.DeserializeObject(interiorJson);
            string writePath = Path.Combine("resources", resourceName, "Interiors", interiorJsonObject.GetValue("Name").ToString() + ".json");
           
            using (StreamWriter file = File.CreateText(writePath))
            using (JsonTextWriter writer = new JsonTextWriter(file))
            {
                interiorJsonObject.WriteTo(writer);
            }

        }
    }
}
