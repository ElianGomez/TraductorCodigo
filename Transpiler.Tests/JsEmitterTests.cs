using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transpiler.Core.Emitters;
using Transpiler.Core.Parsing;

namespace Transpiler.Tests
{
	public class JsEmitterTests
	{
		[Fact]
		public void Emit_SimpleProgram_ReturnsJavaScriptCode()
		{
			string src = "func main() { var a = 1; print(a); }";
			var parser = new Parser(src);
			var ast = parser.ParseProgram();
			var emitter = new JsEmitter();

			var js = emitter.Emit(ast);

			Assert.Contains("function main()", js);
			Assert.Contains("console.log", js);
		}
	}
}
