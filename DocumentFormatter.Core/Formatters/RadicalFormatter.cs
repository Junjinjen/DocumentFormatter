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
            var indexElement = GetChildNode(element, "deg");

            if (indexElement?.IsEmpty == false)
            {
                FormatWithIndex(radicandElement, indexElement, writer, innerElementsHandler);
                return;
            }

            FormatWithoutIndex(radicandElement, writer, innerElementsHandler);
        }

        private void FormatWithIndex(XElement radicandElement, XElement indexElement, StreamWriter writer, Action<XElement, StreamWriter> innerElementsHandler)
        {
            writer.Write(@"\sqrt[");
            innerElementsHandler.Invoke(indexElement, writer);
            writer.Write(@"]{");
            innerElementsHandler.Invoke(radicandElement, writer);
            writer.Write(@"}");
        }

        private void FormatWithoutIndex(XElement radicandElement, StreamWriter writer, Action<XElement, StreamWriter> innerElementsHandler)
        {
            writer.Write(@"\sqrt{");
            innerElementsHandler.Invoke(radicandElement, writer);
            writer.Write(@"}");
        }
    }
}
