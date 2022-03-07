using System;
using Newtonsoft.Json;
using System.IO;

namespace GrafischeEditor_DP.VisitorPattern
{
    internal class OpslaanVisitor : IVisitor
    {
        private readonly string _filepath;

        public OpslaanVisitor(string filepath)
        {
            _filepath = filepath;
        }

        public void VisitFigure(Figuur element)
        {
            throw new NotImplementedException();
        }

        public void VisitGroup(Groep element)
        {
            var fileStream = File.CreateText(_filepath);
            var json = JsonConvert.SerializeObject(element, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
            fileStream.Write(json);
            fileStream.Close();
        }
    }
}
