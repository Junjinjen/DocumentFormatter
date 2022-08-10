using System.Collections.Generic;

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

        public override void Format(FormattingContext context)
        {
            var result = TryGetReplacement(context.Element.Value, out var replacement);
            var value = result ? replacement : context.Element.Value;

            context.Writer.Write(value);
            context.InnerElementsHandler.Invoke(context.Element);
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
