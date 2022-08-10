using System;
using System.IO;
using System.Xml.Linq;

namespace DocumentFormatter.Core.Formatters
{
    public class EquationFormatter : FormatterBase
    {
        protected override string TagName => "oMath";

        public override void Format(XElement element, StreamWriter writer, Action<XElement, StreamWriter> next)
        {
            writer.Write(@"\(");
            next.Invoke(element, writer);
            writer.Write(@"\)");
        }
    }
}
