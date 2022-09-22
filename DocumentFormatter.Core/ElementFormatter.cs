using System.Linq;
using System.Xml.Linq;

namespace DocumentFormatter.Core
{
    public interface IElementFormatter
    {
        bool IsApplicable(XElement element);

        void Format(FormattingContext context);
    }

    public abstract class FormatterBase : IElementFormatter
    {
        protected abstract string TagName { get; }

        public bool IsApplicable(XElement element)
        {
            return element.Name.LocalName == TagName;
        }

        public abstract void Format(FormattingContext context);

        protected static XElement GetChildNode(XElement element, string name)
        {
            return element.Elements().FirstOrDefault(x => x.Name.LocalName == name);
        }

        protected static bool HasChildNode(XElement element, string name)
        {
            return element.Elements().Any(x => x.Name.LocalName == name);
        }

        protected static bool HasNextNode(XElement element, string name, params string[] ignoredNodes)
        {
            var nextElement = element.ElementsAfterSelf().FirstOrDefault(x => !ignoredNodes.Contains(x.Name.LocalName));
            return nextElement?.Name.LocalName == name;
        }
    }
}
