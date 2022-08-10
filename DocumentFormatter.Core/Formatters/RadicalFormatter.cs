using System.Xml.Linq;

namespace DocumentFormatter.Core.Formatters
{
    public class RadicalFormatter : FormatterBase
    {
        protected override string TagName => "rad";

        public override void Format(FormattingContext context)
        {
            var radicandElement = GetChildNode(context.Element, "e");
            var indexElement = GetChildNode(context.Element, "deg");

            if (indexElement?.IsEmpty == false)
            {
                FormatWithIndex(radicandElement, indexElement, context);
                return;
            }

            FormatWithoutIndex(radicandElement, context);
        }

        private static void FormatWithIndex(XElement radicandElement, XElement indexElement, FormattingContext context)
        {
            context.Writer.Write(@"\sqrt[");
            context.InnerElementsHandler.Invoke(indexElement);
            context.Writer.Write(@"]{");
            context.InnerElementsHandler.Invoke(radicandElement);
            context.Writer.Write(@"}");
        }

        private static void FormatWithoutIndex(XElement radicandElement, FormattingContext context)
        {
            context.Writer.Write(@"\sqrt{");
            context.InnerElementsHandler.Invoke(radicandElement);
            context.Writer.Write(@"}");
        }
    }
}
