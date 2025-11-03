using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transpiler.Core
{
	public static class SampleCode
	{
		public const string MiniJavaHello = @"
        func main() {
            var x = 2 + 3 * 4;
            int y = 10;
            if (x > y) { print(""x es mayor""); } else { print(""y es mayor""); }
            while (y < 15) { y = y + 1; }
            return;
        }";
	}
}
