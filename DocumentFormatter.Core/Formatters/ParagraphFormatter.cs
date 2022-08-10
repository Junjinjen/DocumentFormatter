namespace DocumentFormatter.Core.Formatters
{
    public class ParagraphFormatter : FormatterBase
    {
        protected override string TagName => "p";

        public override void Format(FormattingContext context)
        {
            context.InnerElementsHandler.Invoke(context.Element);
            context.Writer.WriteLine();
        }
    }
}
