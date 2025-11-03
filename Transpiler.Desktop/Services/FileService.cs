using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.IO;

namespace Transpiler.Desktop.Services
{
	public static class FileService
	{
		public static (bool ok, string text) OpenText()
		{
			var ofd = new OpenFileDialog
			{
				Filter = "Todos|*.*|MiniJava|*.mj|Texto|*.txt",
				Multiselect = false
			};

			if (ofd.ShowDialog() == true)
				return (true, File.ReadAllText(ofd.FileName));

			return (false, "");
		}

		public static void SaveText(string? text)
		{
			var sfd = new SaveFileDialog
			{
				Filter = "Texto|*.txt|Markdown|*.md|Código|*.js;*.cpp",
				FileName = "output"
			};

			if (sfd.ShowDialog() == true)
				File.WriteAllText(sfd.FileName, text ?? string.Empty);
		}
	}
}
