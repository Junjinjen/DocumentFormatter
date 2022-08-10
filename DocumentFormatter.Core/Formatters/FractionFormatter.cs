namespace DocumentFormatter.Core.Formatters
{
    public class FractionFormatter : FormatterBase
    {
        protected override string TagName => "f";

        public override void Format(FormattingContext context)
        {
            var numeratorElement = GetChildNode(context.Element, "num");
            var denominatorElement = GetChildNode(context.Element, "den");

            context.Writer.Write(@"\frac{");
            context.InnerElementsHandler.Invoke(numeratorElement);
            context.Writer.Write(@"}{");
            context.InnerElementsHandler.Invoke(denominatorElement);
            context.Writer.Write(@"}");
        }
    }
}
