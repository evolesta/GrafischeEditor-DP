using System.Collections.Generic;
using System.IO;
using System;
using Newtonsoft.Json;

namespace GrafischeEditor_DP.Bestand
{
    /// <summary>
    /// 'Receiver' class Bestand voor File I/O
    /// </summary>
    public static class BestandIo
    {
        // leest het xml bestand uit en deserialized de content terug naar een list van objecten
        public static List<IComponent> Open(string Bestandspad)
        {
            var json = File.ReadAllText(Bestandspad);
            var components = JsonConvert.DeserializeObject<List<IComponent>>(json, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
            return components;
        }

        // serialized de list van objecten naar een XML bestand op schijf
        public static void Opslaan(string Bestandspad, IList<IComponent> componenten)
        {
            var fileStream = File.CreateText(Bestandspad);
            var json = JsonConvert.SerializeObject(componenten, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
            fileStream.Write(json);
            fileStream.Close();
        }
    }
}
