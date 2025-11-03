using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transpiler.Core;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace Transpiler.Desktop.ViewModels
{

	public partial class MainViewModel : ObservableObject
	{
		[ObservableProperty] private string sourceCode = SampleCode.MiniJavaHello;
		[ObservableProperty] private string outputCode = string.Empty;
		[ObservableProperty] private string status = "Listo";

		public string[] SourceLangs { get; } = new[] { "MiniJava" };
		public string SourceLang { get; set; } = "MiniJava";
		public string[] TargetLangs { get; } = new[] { "JavaScript", "C++" };
		public string TargetLang { get; set; } = "JavaScript";

		
		public IRelayCommand TranspileCommand { get; }
		public IRelayCommand OpenFileCommand { get; }
		public IRelayCommand SaveFileCommand { get; }

		public MainViewModel()
		{
			TranspileCommand = new RelayCommand(Transpile);
			OpenFileCommand = new RelayCommand(OpenFile);
			SaveFileCommand = new RelayCommand(SaveFile);
		}

		private void Transpile()
		{

			try
			{
				var target = TargetLang == "JavaScript"
					? Transpiler.Core.TargetLang.JavaScript
					: Transpiler.Core.TargetLang.Cpp;

				var (ast, code) = TranspilerFacade.Transpile(SourceCode, target);
				OutputCode = code;
				Status = "Transpilación exitosa";
			}
			catch (System.Exception ex)
			{
				Status = "Error al transpilar";
			}
		}

		

		private void OpenFile()
		{
			var (ok, text) = Services.FileService.OpenText();
			if (ok) SourceCode = text;
		}

		private void SaveFile()
		{
			Services.FileService.SaveText(OutputCode);
		}
	}
}
