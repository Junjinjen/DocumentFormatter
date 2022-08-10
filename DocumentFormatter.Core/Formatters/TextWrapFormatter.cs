using System;
using System.IO;
using System.Xml.Linq;

namespace DocumentFormatter.Core.Formatters
{
    public class TextWrapFormatter : FormatterBase
    {
        protected override string TagName => "br";

        public override void Format(XElement element, StreamWriter writer, Action<XElement, StreamWriter> innerElementsHandler)
        {
            innerElementsHandler.Invoke(element, writer);
            writer.WriteLine();
        }
    }
}
