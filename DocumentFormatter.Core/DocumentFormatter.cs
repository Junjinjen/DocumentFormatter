using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Xml.Linq;

namespace DocumentFormatter.Core
{
    public interface IDocumentFormatter
    {
        void Format(string filename, Stream output);
    }

    public class MicrosoftWordFormatter : IDocumentFormatter
    {
        private const string DocumentRelationshipType = "http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument";
        private const string DocumentPartPath = "/";
        private readonly List<IElementFormatter> _formatters;

        public MicrosoftWordFormatter(List<IElementFormatter> formatters)
        {
            _formatters = formatters;
        }

        public void Format(string filename, Stream output)
        {
            using var package = Package.Open(filename, FileMode.Open, FileAccess.Read);
            var packageRelationship = package.GetRelationshipsByType(DocumentRelationshipType).First();
            var partPath = PackUriHelper.ResolvePartUri(new Uri(DocumentPartPath, UriKind.Relative), packageRelationship.TargetUri);
            var part = package.GetPart(partPath);

            using var partStream = part.GetStream();
            var document = XDocument.Load(partStream);
            var streamWriter = new StreamWriter(output);
            FormatElement(document.Root, streamWriter);

            streamWriter.Flush();
        }

        private void FormatElement(XElement element, StreamWriter writer)
        {
            var formatter = _formatters.FirstOrDefault(x => x.IsApplicable(element));
            if (formatter != null)
            {
                formatter.Format(element, writer, FormatInnerElements);
                return;
            }

            FormatInnerElements(element, writer);
        }

        private void FormatInnerElements(XElement element, StreamWriter writer)
        {
            var childElements = element.Elements();
            foreach (var childElement in childElements)
            {
                FormatElement(childElement, writer);
            }
        }
    }
}
