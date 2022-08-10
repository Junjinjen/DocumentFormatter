using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace DocumentFormatter.Core.Formatters
{
    public class Replacement
    {
        public string SearchValue { get; set; }

        public string ReplacementValue { get; set; }

        public bool AllowPartialMatch { get; set; }
    }

    public class TextFormatter : FormatterBase
    {
        private readonly List<Replacement> _replacements;

        public TextFormatter(List<Replacement> replacements)
        {
            _replacements = replacements;
        }

        protected override string TagName => "t";

        public override void Format(XElement element, StreamWriter writer, Action<XElement, StreamWriter> innerElementsHandler)
        {
            var result = TryGetReplacement(element.Value, out var replacement);
            var value = result ? replacement : element.Value;

            writer.Write(value);
            innerElementsHandler.Invoke(element, writer);
        }

        private bool TryGetReplacement(string elementValue, out string formattedString)
        {
            foreach (var replacement in _replacements)
            {
                var result = replacement.AllowPartialMatch
                    ? elementValue.Contains(replacement.SearchValue)
                    : elementValue == replacement.SearchValue;

                if (result)
                {
                    formattedString = elementValue.Replace(replacement.SearchValue, replacement.ReplacementValue);
                    return true;
                }
            }

            formattedString = default;
            return false;
        }
    }
}
