using System;
using ICSharpCode.AvalonEdit.Folding;

namespace RazorPad.UI.Editors.Folding
{
    public class RazorElementFold : NewFolding
    {
        string elementName = String.Empty;

        public string ElementName
        {
            get { return elementName; }
            set
            {
                elementName = value;
                UpdateFoldName();
            }
        }

        void UpdateFoldName()
        {
            Name = String.Format("{0}", elementName);
        }

        public int Line { get; set; }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == typeof(RazorElementFold) && Equals((RazorElementFold)obj);
        }

        protected bool Equals(RazorElementFold other)
        {
            return string.Equals(elementName, other.elementName) && Line == other.Line;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return String.Format(
                "[RazorElementFold Name='{0}', StartOffset={1}, EndOffset={2}]",
                Name,
                StartOffset,
                EndOffset);
        }

    }
}