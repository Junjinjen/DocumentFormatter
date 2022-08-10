﻿using System;
using System.IO;
using System.Xml.Linq;

namespace DocumentFormatter.Core.Formatters
{
    public class ExponentFormatter : FormatterBase
    {
        protected override string TagName => "sSup";

        public override void Format(XElement element, StreamWriter writer, Action<XElement, StreamWriter> next)
        {
            var baseElement = GetChildNode(element, "e");
            var exponentElement = GetChildNode(element, "sup");

            next.Invoke(baseElement, writer);
            writer.Write(@"^{");
            next.Invoke(exponentElement, writer);
            writer.Write(@"}");
        }
    }
}
