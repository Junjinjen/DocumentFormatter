namespace DocumentFormatter.Core.Formatters
{
    public class EquationFormatter : FormatterBase
    {
        protected override string TagName => "oMath";

        public override void Format(FormattingContext context)
        {
            context.Writer.Write(@"\(");
            context.InnerElementsHandler.Invoke(context.Element);
            context.Writer.Write(@"\)");
        }
    }
}
