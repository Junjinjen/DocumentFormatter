namespace DocumentFormatter.Core.Formatters
{
    public class ExponentFormatter : FormatterBase
    {
        protected override string TagName => "sSup";

        public override void Format(FormattingContext context)
        {
            var baseElement = GetChildNode(context.Element, "e");
            var exponentElement = GetChildNode(context.Element, "sup");

            context.Writer.Write(@"{");
            context.InnerElementsHandler.Invoke(baseElement);
            context.Writer.Write(@"}");

            context.Writer.Write(@"^{");
            context.InnerElementsHandler.Invoke(exponentElement);
            context.Writer.Write(@"}");
        }
    }
}
