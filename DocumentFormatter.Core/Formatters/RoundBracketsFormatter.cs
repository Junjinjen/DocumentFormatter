namespace DocumentFormatter.Core.Formatters
{
    public class RoundBracketsFormatter : FormatterBase
    {
        protected override string TagName => "d";

        public override void Format(FormattingContext context)
        {
            context.Writer.Write(@"\left(");
            context.InnerElementsHandler.Invoke(context.Element);
            context.Writer.Write(@"\right)");
        }
    }
}
