namespace DocumentFormatter.Core.Formatters
{
    public class TextWrapFormatter : FormatterBase
    {
        protected override string TagName => "br";

        public override void Format(FormattingContext context)
        {
            context.InnerElementsHandler.Invoke(context.Element);
            context.Writer.WriteLine();
        }
    }
}
