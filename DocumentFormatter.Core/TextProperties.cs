using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DocumentFormatter.Core
{
    public sealed class TextProperties : IEquatable<TextProperties>
    {
        private const string BeginSpanFormat = "<span style=\"{0}\">";
        private const string ItalicTextSetting = "font-style:italic;";
        private const string BoldTextSetting = "font-weight:bold;";
        private const string UnderlineTextSetting = "text-decoration:underline;";
        private const string EndSpan = "</span>";

        public bool Italic { get; set; }

        public bool Bold { get; set; }

        public bool Underline { get; set; }

        public bool HasAnyStyle => Italic || Bold || Underline;

        public void BeginTextStyle(StreamWriter writer)
        {
            if (!HasAnyStyle)
            {
                return;
            }

            var settingsString = new StringBuilder();
            if (Italic)
            {
                settingsString.Append(ItalicTextSetting);
            }

            if (Bold)
            {
                settingsString.Append(BoldTextSetting);
            }

            if (Underline)
            {
                settingsString.Append(UnderlineTextSetting);
            }

            var beginSpan = string.Format(BeginSpanFormat, settingsString.ToString());
            writer.Write(beginSpan);
        }

        public void EndTextStyle(StreamWriter writer)
        {
            if (!HasAnyStyle)
            {
                return;
            }

            writer.Write(EndSpan);

            Italic = false;
            Bold = false;
            Underline = false;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as TextProperties);
        }

        public bool Equals(TextProperties other)
        {
            return other is not null &&
                   Italic == other.Italic &&
                   Bold == other.Bold &&
                   Underline == other.Underline;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Italic, Bold, Underline);
        }

        public static bool operator ==(TextProperties left, TextProperties right)
        {
            return EqualityComparer<TextProperties>.Default.Equals(left, right);
        }

        public static bool operator !=(TextProperties left, TextProperties right)
        {
            return !(left == right);
        }
    }
}
