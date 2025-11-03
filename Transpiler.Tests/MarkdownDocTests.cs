using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transpiler.Core.Docs;
using Transpiler.Core.Parsing;

namespace Transpiler.Tests
{
	public class MarkdownDocTests
	{
		[Fact]
		public void GenerateMarkdown_ReturnsValidDocument()
		{
			string src = "func main() { var x = 5; }";
			var parser = new Parser(src);
			var ast = parser.ParseProgram();

			var md = MarkdownDocGenerator.FromAst(ast, src);

			Assert.Contains("# Documentación", md);
			Assert.Contains("func main", md);
		}
	}
}
