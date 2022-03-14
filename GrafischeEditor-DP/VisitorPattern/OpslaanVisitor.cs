using System;
using Newtonsoft.Json;
using System.IO;

namespace GrafischeEditor_DP.VisitorPattern
{
    /// <summary>
    /// A visitor that saves the current drawing, which consists of a toplevel group and all of its children.
    /// Therefore, only VisitGroup is implemented, and visiting a single figure is considered invalid. 
    /// </summary>
    internal class OpslaanVisitor : IVisitor
    {
        private readonly string _filepath;

        public OpslaanVisitor(string filepath)
        {
            _filepath = filepath;
        }
        
        public void VisitFigure(Figuur element)
        {
            throw new InvalidOperationException("A single figure can not be saved in a file");
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
