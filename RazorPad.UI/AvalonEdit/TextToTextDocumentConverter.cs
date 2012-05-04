using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using ICSharpCode.AvalonEdit.Document;

namespace RazorPad.UI.AvalonEdit
{

	public class TextToTextDocumentConverter : IValueConverter
	{
		public object Convert(object value, Type targetType,
			object parameter, CultureInfo culture)
		{
			var text = (value ?? string.Empty).ToString();
			return new TextDocument(text);
		}

		public object ConvertBack(object value, Type targetType,
			object parameter, CultureInfo culture)
		{
			var document = value as TextDocument;

			return document == null ? string.Empty : document.Text;
		}

	}
}
