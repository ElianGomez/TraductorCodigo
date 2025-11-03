using ICSharpCode.AvalonEdit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Transpiler.Desktop.Helpers
{
	public static class AvalonEditBinding
	{
		public static readonly DependencyProperty BoundTextProperty =
			DependencyProperty.RegisterAttached(
				"BoundText",
				typeof(string),
				typeof(AvalonEditBinding),
				new FrameworkPropertyMetadata(default(string),
					FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
					OnBoundTextChanged));

		public static string GetBoundText(DependencyObject obj) =>
			(string)obj.GetValue(BoundTextProperty);

		public static void SetBoundText(DependencyObject obj, string value) =>
			obj.SetValue(BoundTextProperty, value);

		private static bool _isUpdating;

		private static void OnBoundTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is TextEditor editor)
			{
				if (!_isUpdating)
				{
					editor.Text = e.NewValue as string ?? string.Empty;
					editor.TextChanged -= Editor_TextChanged;
					editor.TextChanged += Editor_TextChanged;
				}
			}
		}

		private static void Editor_TextChanged(object? sender, EventArgs e)
		{
			if (sender is TextEditor editor)
			{
				_isUpdating = true;
				SetBoundText(editor, editor.Text);
				_isUpdating = false;
			}
		}
	}
}
