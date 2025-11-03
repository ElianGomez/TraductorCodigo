using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transpiler.Core.Ast;

namespace Transpiler.Core.Emitters
{
	public interface IEmitter
	{
		string Emit(ProgramNode program);
	}
}
