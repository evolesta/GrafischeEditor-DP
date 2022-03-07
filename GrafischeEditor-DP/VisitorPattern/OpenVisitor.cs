using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace GrafischeEditor_DP.VisitorPattern
{
    internal class OpenVisitor : IVisitor
    {
        private readonly string _filepath;
        private Controller _controller;

        public OpenVisitor(string filepath, Controller controller)
        {
            _filepath = filepath;
            _controller = controller;
        }

        public void VisitFigure(Figuur element)
        {
            throw new NotImplementedException();
        }

        public void VisitGroup(Groep element)
        {
            var json = File.ReadAllText(_filepath);
            var groep = JsonConvert.DeserializeObject<Groep>(json, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
            _controller.HoofdGroep = groep;
        }
    }
}
