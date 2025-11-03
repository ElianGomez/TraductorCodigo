using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transpiler.Core.Ast
{
	public abstract record Node;

	public abstract record Stmt : Node;
	public abstract record Expr : Node;

	public record ProgramNode(List<Node> Members) : Node;
	public record BlockStmt(List<Stmt> Statements) : Stmt;

	public record FuncDecl(string Name, List<string> Params, BlockStmt Body) : Node;

	public record VarDecl(string? Type, string Name, Expr? Init) : Stmt; // Type=null => var
	public record Assign(string Name, Expr Value) : Stmt;
	public record IfStmt(Expr Cond, Stmt Then, Stmt? Else) : Stmt;
	public record WhileStmt(Expr Cond, Stmt Body) : Stmt;
	public record ReturnStmt(Expr? Value) : Stmt;
	public record PrintStmt(Expr Value) : Stmt;

	public record CallExpr(string Callee, List<Expr> Args) : Expr;
	public record Binary(Expr Left, string Op, Expr Right) : Expr;
	public record Unary(string Op, Expr Right) : Expr;
	public record Literal(object? Value) : Expr;
	public record VarExpr(string Name) : Expr;
	internal class Nodes
	{
	}
}
