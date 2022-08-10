using System;
using System.IO;
using System.Xml.Linq;

namespace DocumentFormatter.Core.Formatters
{
    public class RoundBracketsFormatter : FormatterBase
    {
        protected override string TagName => "d";

        public override void Format(XElement element, StreamWriter writer, Action<XElement, StreamWriter> innerElementsHandler)
        {
            writer.Write(@"\left(");
            innerElementsHandler.Invoke(element, writer);
            writer.Write(@"\right)");
        }
    }
}
