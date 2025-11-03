using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transpiler.Core.Ast;
using Transpiler.Core.Parsing;

namespace Transpiler.Tests
{
	public class ParserTests
	{
		[Fact]
		public void Parse_MainFunction_Succeeds()
		{
			string src = "func main() { var x = 1 + 2; return; }";
			var parser = new Parser(src);

			var ast = parser.ParseProgram();

			Assert.NotNull(ast);
			Assert.Single(ast.Members);
			Assert.IsType<FuncDecl>(ast.Members[0]);
		}
	}
}
