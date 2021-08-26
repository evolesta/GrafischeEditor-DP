using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace GrafischeEditor_DP
{
    /// <summary>
    /// 'Receiver' class Bestand voor File I/O
    /// </summary>
    class Bestand
    {
        // leest het xml bestand uit en deserialized de content terug naar een list van objecten
        public List<Figuur> Open(string Bestandspad)
        {
            FileStream fileStream = new FileStream(Bestandspad, FileMode.Open);
            XmlSerializer serializer = new XmlSerializer(typeof(List<Figuur>));
            List<Figuur> figuren = (List<Figuur>)serializer.Deserialize(fileStream);
            fileStream.Close(); // sluit geopend bestand
            return figuren; // geeft lijst terug
        }

        // serialized de list van objecten naar een XML bestand op schijf
        public void Opslaan(string Bestandspad, List<Figuur> figuren)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Figuur>));
            FileStream filestream = new FileStream(Bestandspad, FileMode.Create);
            serializer.Serialize(filestream, figuren);
            filestream.Close();
        }
    }
}
