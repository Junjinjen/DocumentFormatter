using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace DocumentFormatter.Core
{
    public class FormattingContext
    {
        public XElement Element { get; set; }

        public StreamWriter Writer { get; set; }

        public Action<XElement> InnerElementsHandler { get; set; }

        public Stack<string> ElementsStack { get; set; }
    }
}
