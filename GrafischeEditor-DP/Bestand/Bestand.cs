using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace GrafischeEditor_DP
{
    // Receiver class met concrete handelingen
    class Bestand
    {
        // leest het xml bestand uit en deserialized de content terug naar een list van objecten
        public List<Figuur> Open(string Bestandspad)
        {
            Controller controller = new Controller(); // nieuw controller object aanmaken
            controller.ResetFiguren(); // verwijder alle figuren uit de lijst

            FileStream fileStream = new FileStream(Bestandspad, FileMode.Open);
            XmlSerializer serializer = new XmlSerializer(controller.GetFiguren().GetType());
            List<Figuur> figuren = (List<Figuur>)serializer.Deserialize(fileStream);
            fileStream.Close(); // sluit geopend bestand
            return figuren; // geeft lijst terug
        }

        // serialized de list van objecten naar een XML bestand op schijf
        public void Opslaan(string Bestandspad, List<Figuur> figuren)
        {
            XmlSerializer serializer = new XmlSerializer(figuren.GetType());
            FileStream filestream = new FileStream(Bestandspad, FileMode.Create);
            serializer.Serialize(filestream, figuren);
            filestream.Close();
        }
    }
}
