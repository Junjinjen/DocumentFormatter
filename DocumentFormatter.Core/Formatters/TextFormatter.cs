using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace DocumentFormatter.Core.Formatters
{
    public class Replacement
    {
        public string SearchValue { get; set; }

        public string ReplacementValue { get; set; }

        public bool AllowPartialMatch { get; set; }

        public string RequiredParent { get; set; }
    }

    public class TextFormatter : FormatterBase
    {
        private static TextProperties _scopeTextProperties = new();
        private readonly List<Replacement> _replacements;

        public TextFormatter(List<Replacement> replacements)
        {
            _replacements = replacements.OrderBy(x => x.AllowPartialMatch).ToList();
        }

        protected override string TagName => "r";

        public override void Format(FormattingContext context)
        {
            if (!HasChildNode(context.Element, "t"))
            {
                context.InnerElementsHandler.Invoke(context.Element);
                return;
            }

            CheckTextPropertiesBegin(context);
            FormatText(context);
            CheckTextPropertiesEnd(context);
        }

        private static void CheckTextPropertiesBegin(FormattingContext context)
        {
            var textProperties = GetTextProperties(context);
            if (_scopeTextProperties != textProperties)
            {
                _scopeTextProperties.EndTextStyle(context.Writer);
                textProperties.BeginTextStyle(context.Writer);

                _scopeTextProperties = textProperties;
            }
        }

        private static void CheckTextPropertiesEnd(FormattingContext context)
        {
            if (!HasNextNode(context.Element, "r", "proofErr"))
            {
                _scopeTextProperties.EndTextStyle(context.Writer);
            }
        }

        private static TextProperties GetTextProperties(FormattingContext context)
        {
            var propertiesElement = GetChildNode(context.Element, "rPr");
            return new TextProperties
            {
                Bold = HasChildNode(propertiesElement, "b"),
                Italic = HasChildNode(propertiesElement, "i"),
                Underline = HasChildNode(propertiesElement, "u"),
            };
        }

        private static bool CheckReplacement(Replacement replacement, string elementValue, Stack<string> elementsStack)
        {
            if (!string.IsNullOrEmpty(replacement.RequiredParent) && !elementsStack.Contains(replacement.RequiredParent))
            {
                return false;
            }

            return replacement.AllowPartialMatch
                ? elementValue.Contains(replacement.SearchValue)
                : elementValue == replacement.SearchValue;
        }

        private void FormatText(FormattingContext context)
        {
            var textElement = GetChildNode(context.Element, "t");
            var result = TryGetReplacement(textElement.Value, context.ElementsStack, out var replacement);
            var value = result ? replacement : textElement.Value;

            context.Writer.Write(value);
            context.InnerElementsHandler.Invoke(textElement);
        }

        private bool TryGetReplacement(string elementValue, Stack<string> elementsStack, out string formattedString)
        {
            var replacement = _replacements.FirstOrDefault(x => CheckReplacement(x, elementValue, elementsStack));
            if (replacement != null)
            {
                formattedString = elementValue.Replace(replacement.SearchValue, replacement.ReplacementValue);
                return true;
            }

            formattedString = default;
            return false;
        }
    }
}
