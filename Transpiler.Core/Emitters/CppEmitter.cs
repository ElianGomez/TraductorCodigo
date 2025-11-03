using System.Linq;
using System.Text;
using Transpiler.Core.Ast;

namespace Transpiler.Core.Emitters
{
	public sealed class CppEmitter : IEmitter
	{
		public string Emit(ProgramNode p)
		{
			var sb = new StringBuilder();
			sb.AppendLine("#include <bits/stdc++.h>");
			sb.AppendLine("using namespace std;");
			foreach (var m in p.Members)
			{
				EmitMember(sb, m);
				sb.AppendLine();
			}
			return sb.ToString();
		}

		private void EmitMember(StringBuilder sb, Node n)
		{
			if (n is FuncDecl f)
			{
				sb.Append("int ").Append(f.Name).Append("(")
				  .Append(string.Join(", ", f.Params.Select(x => $"auto {x}"))).Append(") ");
				EmitBlock(sb, f.Body);
			}
			else if (n is Stmt s)
			{
				EmitStmt(sb, s);
			}
		}

		private void EmitBlock(StringBuilder sb, BlockStmt b)
		{
			sb.AppendLine("{");
			foreach (var s in b.Statements) EmitStmt(sb, s);
			sb.AppendLine("}");
		}

		private void EmitStmt(StringBuilder sb, Stmt s)
		{
			switch (s)
			{
				case VarDecl v:
					var t = v.Type == null ? "auto" : v.Type switch
					{
						"int" => "int",
						"double" => "double",
						"bool" => "bool",
						"string" => "string",
						_ => "auto"
					};
					sb.Append($"{t} {v.Name}");
					if (v.Init != null) { sb.Append(" = "); EmitExpr(sb, v.Init); }
					sb.AppendLine(";");
					break;

				case Assign a:
					sb.Append($"{a.Name} = "); EmitExpr(sb, a.Value); sb.AppendLine(";");
					break;

				case IfStmt i:
					sb.Append("if ("); EmitExpr(sb, i.Cond); sb.Append(") ");
					EmitBlock(sb, i.Then is BlockStmt b1 ? b1 : new BlockStmt(new() { i.Then }));
					if (i.Else != null)
					{
						sb.Append("else ");
						EmitBlock(sb, i.Else is BlockStmt b2 ? b2 : new BlockStmt(new() { i.Else }));
					}
					break;

				case WhileStmt w:
					sb.Append("while ("); EmitExpr(sb, w.Cond); sb.Append(") ");
					EmitBlock(sb, w.Body is BlockStmt wb ? wb : new BlockStmt(new() { w.Body }));
					break;

				case ReturnStmt r:
					sb.Append("return");
					if (r.Value != null) { sb.Append(" "); EmitExpr(sb, r.Value); }
					sb.AppendLine(";");
					break;

				case PrintStmt p:
					sb.Append("cout << "); EmitExpr(sb, p.Value); sb.AppendLine(" << endl;");
					break;

				case BlockStmt b:
					EmitBlock(sb, b);
					break;
			}
		}

		private void EmitExpr(StringBuilder sb, Expr e)
		{
			switch (e)
			{
				case Literal lit:
					sb.Append(lit.Value is string ? $"\"{lit.Value}\"" : lit.Value?.ToString()?.ToLower());
					break;
				case VarExpr v:
					sb.Append(v.Name);
					break;
				case Unary u:
					sb.Append(u.Op); EmitExpr(sb, u.Right);
					break;
				case Binary b:
					EmitExpr(sb, b.Left);
					sb.Append($" {b.Op} ");
					EmitExpr(sb, b.Right);
					break;
				case CallExpr c:
					sb.Append(c.Callee).Append("(");
					for (int i = 0; i < c.Args.Count; i++)
					{
						if (i > 0) sb.Append(", ");
						EmitExpr(sb, c.Args[i]);
					}
					sb.Append(")");
					break;
			}
		}
	}
}
