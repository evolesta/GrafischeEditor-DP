using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

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
            var fileStream = new FileStream(Bestandspad, FileMode.Open);
            var serializer = new XmlSerializer(typeof(List<Figuur>));
            var figuren = (List<IComponent>)serializer.Deserialize(fileStream);
            fileStream.Close(); // sluit geopend bestand
            return figuren; // geeft lijst terug
        }

        // serialized de list van objecten naar een XML bestand op schijf
        public static void Opslaan(string Bestandspad, IList<IComponent> componenten)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Figuur>));
            FileStream filestream = new FileStream(Bestandspad, FileMode.Create);
            serializer.Serialize(filestream, componenten);
            filestream.Close();
        }
    }
}
