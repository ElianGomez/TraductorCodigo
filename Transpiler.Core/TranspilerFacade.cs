using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transpiler.Core.Ast;
using Transpiler.Core.Parsing;
using Transpiler.Core.Emitters;
using Transpiler.Core.Docs;

namespace Transpiler.Core
{
	public static class TranspilerFacade
	{
		public static (ProgramNode ast, string output) Transpile(string source, TargetLang target)
		{
			var parser = new Parser(source);
			var ast = parser.ParseProgram();

			IEmitter emitter = target switch
			{
				TargetLang.JavaScript => new JsEmitter(),
				TargetLang.Cpp => new CppEmitter(),
				_ => throw new NotSupportedException()
			};

			var code = emitter.Emit(ast);
			return (ast, code);
		}
	}
}
