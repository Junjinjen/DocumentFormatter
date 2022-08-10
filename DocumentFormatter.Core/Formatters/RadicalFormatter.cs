using System;
using System.IO;
using System.Xml.Linq;

namespace DocumentFormatter.Core.Formatters
{
    public class RadicalFormatter : FormatterBase
    {
        protected override string TagName => "rad";

        public override void Format(XElement element, StreamWriter writer, Action<XElement, StreamWriter> innerElementsHandler)
        {
            var radicandElement = GetChildNode(element, "e");

            writer.Write(@"\sqrt{");
            innerElementsHandler.Invoke(radicandElement, writer);
            writer.Write(@"}");
        }
    }
}
