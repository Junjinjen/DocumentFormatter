using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace DocumentFormatter.Core.Formatters
{
    public class TextFormatter : FormatterBase
    {
        private readonly Dictionary<string, string> _replacementDictionary;

        public TextFormatter(Dictionary<string, string> replacementDictionary)
        {
            _replacementDictionary = replacementDictionary;
        }

        protected override string TagName => "t";

        public override void Format(XElement element, StreamWriter writer, Action<XElement, StreamWriter> innerElementsHandler)
        {
            var result = _replacementDictionary.TryGetValue(element.Value, out var replacement);
            var value = result ? replacement : element.Value;

            writer.Write(value);
            innerElementsHandler.Invoke(element, writer);
        }
    }
}
