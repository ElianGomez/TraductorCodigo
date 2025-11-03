using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transpiler.Core.Ast;

namespace Transpiler.Core.Docs
{
	public static class MarkdownDocGenerator
	{
		public static string FromAst(ProgramNode program, string source)
		{
			var sb = new StringBuilder();
			sb.AppendLine("# Documentación generada automáticamente");
			sb.AppendLine();
			sb.AppendLine("```miniJava");
			sb.AppendLine(source.Trim());
			sb.AppendLine("```");
			sb.AppendLine();

			sb.AppendLine("## Estructura del programa");
			foreach (var m in program.Members)
			{
				if (m is FuncDecl f)
				{
					sb.AppendLine($"### Función `{f.Name}`");
					sb.AppendLine($"Parámetros: {string.Join(", ", f.Params)}");
					sb.AppendLine();
				}
				else if (m is VarDecl v)
				{
					sb.AppendLine($"- Variable `{v.Name}` inicializada con `{v.Init}`");
				}
			}

			sb.AppendLine();
			sb.AppendLine("_Generado por Transpiler.Core_");
			return sb.ToString();
		}
	}
}
