using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transpiler.Core.Emitters;
using Transpiler.Core.Parsing;

namespace Transpiler.Tests
{
	public class CppEmitterTests
	{
		[Fact]
		public void Emit_SimpleProgram_ReturnsCppCode()
		{
			string src = "func main() { var a = 1; print(a); }";
			var parser = new Parser(src);
			var ast = parser.ParseProgram();
			var emitter = new CppEmitter();

			var cpp = emitter.Emit(ast);

			Assert.Contains("int main", cpp);
			Assert.Contains("cout <<", cpp);
		}
	}
}
