﻿using System;
using System.IO;
using System.Xml.Linq;

namespace DocumentFormatter.Core.Formatters
{
    public class FractionFormatter : FormatterBase
    {
        protected override string TagName => "f";

        public override void Format(XElement element, StreamWriter writer, Action<XElement, StreamWriter> next)
        {
            var numeratorElement = GetChildNode(element, "num");
            var denominatorElement = GetChildNode(element, "den");

            writer.Write(@"\frac{");
            next.Invoke(numeratorElement, writer);
            writer.Write(@"}{");
            next.Invoke(denominatorElement, writer);
            writer.Write(@"}");
        }
    }
}