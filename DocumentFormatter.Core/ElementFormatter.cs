using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace DocumentFormatter.Core
{
    public interface IElementFormatter
    {
        bool IsApplicable(XElement element);

        void Format(XElement element, StreamWriter writer, Action<XElement, StreamWriter> innerElementsHandler);
    }

    public abstract class FormatterBase : IElementFormatter
    {
        protected abstract string TagName { get; }

        public bool IsApplicable(XElement element)
        {
            return element.Name.LocalName == TagName;
        }

        public abstract void Format(XElement element, StreamWriter writer, Action<XElement, StreamWriter> innerElementsHandler);

        protected static XElement GetChildNode(XElement element, string name)
        {
            return element.Elements().First(x => x.Name.LocalName == name);
        }
    }
}
