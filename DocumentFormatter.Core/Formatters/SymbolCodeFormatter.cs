using System.Collections.Generic;
using System.Linq;

namespace DocumentFormatter.Core.Formatters
{
    public class SymbolCodeFormatter : FormatterBase
    {
        private readonly Dictionary<string, string> _replacements;

        public SymbolCodeFormatter(Dictionary<string, string> replacements)
        {
            _replacements = replacements;
        }

        protected override string TagName => "sym";

        public override void Format(FormattingContext context)
        {
            var code = context.Element.Attributes().FirstOrDefault(x => x.Name.LocalName == "char")?.Value;
            if (code == null)
            {
                return;
            }

            if (_replacements.TryGetValue(code, out var value))
            {
                context.Writer.Write(value);
            }
        }
    }
}
