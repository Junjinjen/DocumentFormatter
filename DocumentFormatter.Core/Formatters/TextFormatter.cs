using System.Collections.Generic;
using System.Linq;

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
        private readonly List<Replacement> _replacements;

        public TextFormatter(List<Replacement> replacements)
        {
            _replacements = replacements;
        }

        protected override string TagName => "t";

        public override void Format(FormattingContext context)
        {
            var result = TryGetReplacement(context.Element.Value, context.ElementsStack, out var replacement);
            var value = result ? replacement : context.Element.Value;

            context.Writer.Write(value);
            context.InnerElementsHandler.Invoke(context.Element);
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
