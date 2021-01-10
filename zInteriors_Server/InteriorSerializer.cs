using System;
using Newtonsoft.Json;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace zInteriors_Server
{
    public class InteriorSerializer : BaseScript
    {
        private static string resourceName = GetCurrentResourceName();

        public InteriorSerializer()
        {
            EventHandlers["zInteriors:serializeInterior"] += new Action<string>(SerializeInterior);
            EventHandlers["zInteriors:getInteriors"] += new Action<Player>(GetInteriors);
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

            LoadResourceFile(resourceName, writePath);
        }

        private Interior DeserializeInteriorFromFile(string filePath)
        {
            using (StreamReader file = File.OpenText(filePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                Interior parsedInterior = (Interior)serializer.Deserialize(file, typeof(Interior));
                return parsedInterior;
            }
            return null;
        }

        private void GetInteriors([FromSource]Player sourcePlayer)
        {
            string readPath = Path.Combine("resources", resourceName, "Interiors");

            IList<Interior> parsedInteriors = new List<Interior>();
            string[] interiorFilePaths = Directory.GetFiles(readPath, "*.json");

            foreach (string filePath in interiorFilePaths)
            {
                parsedInteriors.Add(DeserializeInteriorFromFile(filePath));
            }

            Debug.WriteLine(String.Format("Sending {0} interiors to {1}", parsedInteriors.Count, sourcePlayer.Name));

            sourcePlayer.TriggerEvent("zInteriors:getInteriors", parsedInteriors);
        }
    }
}
