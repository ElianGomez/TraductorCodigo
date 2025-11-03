using System;
using System.Collections.Generic;
using Transpiler.Core.Ast;
using Transpiler.Core.Lexing;

namespace Transpiler.Core.Parsing
{
	public sealed class Parser
	{
		private readonly Lexer _lex;
		private Token _t;

		public Parser(string src)
		{
			_lex = new Lexer(src);
			_t = _lex.Next();
		}

		private void Eat() => _t = _lex.Next();
		private bool Is(string s) => _t.Lexeme == s;
		private bool Kind(TokKind k) => _t.Kind == k;

		private Token Expect(string s)
		{
			if (!Is(s)) throw new Exception($"Se esperaba '{s}', se encontró '{_t.Lexeme}'.");
			var tok = _t;
			Eat();
			return tok;
		}

		public ProgramNode ParseProgram()
		{
			var members = new List<Node>();
			while (_t.Kind != TokKind.Eof)
			{
				if (Is("func")) members.Add(ParseFunc());
				else members.Add(ParseStatement());
			}
			return new ProgramNode(members);
		}

		private FuncDecl ParseFunc()
		{
			Eat();
			var name = ExpectId();
			Expect("(");
			var ps = new List<string>();
			if (!Is(")"))
			{
				ps.Add(ExpectId());
				while (Is(","))
				{
					Eat();
					ps.Add(ExpectId());
				}
			}
			Expect(")");
			var body = ParseBlock();
			return new FuncDecl(name, ps, body);
		}

		private string ExpectId()
		{
			if (_t.Kind != TokKind.Identifier)
				throw new Exception($"Identificador esperado: '{_t.Lexeme}'");
			var id = _t.Lexeme;
			Eat();
			return id;
		}

		private BlockStmt ParseBlock()
		{
			Expect("{");
			var list = new List<Stmt>();
			while (!Is("}"))
				list.Add(ParseStatement());
			Expect("}");
			return new BlockStmt(list);
		}

		private Stmt ParseStatement()
		{
			if (Is("{")) return ParseBlock();
			if (Is("if")) return ParseIf();
			if (Is("while")) return ParseWhile();
			if (Is("return")) return ParseReturn();
			if (Is("print"))
			{
				Eat();
				Expect("(");
				var e = ParseExpr();
				Expect(")");
				Expect(";");
				return new PrintStmt(e);
			}
			if (Is(";")) { Eat(); return new BlockStmt(new()); }

			if (_t.Kind == TokKind.Keyword && (Is("var") || Is("int") || Is("double") || Is("bool") || Is("string")))
			{
				string? type = Is("var") ? null : _t.Lexeme;
				Eat();
				var name = ExpectId();
				Expr? init = null;
				if (Is("=")) { Eat(); init = ParseExpr(); }
				Expect(";");
				return new VarDecl(type, name, init);
			}

			if (_t.Kind == TokKind.Identifier)
			{
				var id = _t.Lexeme;
				Eat();
				if (Is("="))
				{
					Eat();
					var e = ParseExpr();
					Expect(";");
					return new Assign(id, e);
				}
				throw new Exception($"Declaración no válida cerca de '{id}'. Falta '=' o ';'.");
			}

			throw new Exception($"Sentencia no válida cerca de '{_t.Lexeme}'.");
		}

		private Stmt ParseIf()
		{
			Eat();
			Expect("(");
			var c = ParseExpr();
			Expect(")");
			var thenS = ParseStatement();
			Stmt? els = null;
			if (Is("else"))
			{
				Eat();
				els = ParseStatement();
			}
			return new IfStmt(c, thenS, els);
		}

		private Stmt ParseWhile()
		{
			Eat();
			Expect("(");
			var c = ParseExpr();
			Expect(")");
			var b = ParseStatement();
			return new WhileStmt(c, b);
		}

		private Stmt ParseReturn()
		{
			Eat();
			if (Is(";")) { Eat(); return new ReturnStmt(null); }
			var e = ParseExpr();
			Expect(";");
			return new ReturnStmt(e);
		}

		private Expr ParseExpr() => ParseEquality();

		private Expr ParseEquality()
		{
			var e = ParseComp();
			while (Is("==") || Is("!="))
			{
				var op = _t.Lexeme;
				Eat();
				e = new Binary(e, op, ParseComp());
			}
			return e;
		}

		private Expr ParseComp()
		{
			var e = ParseTerm();
			while (Is("<") || Is(">") || Is("<=") || Is(">="))
			{
				var op = _t.Lexeme;
				Eat();
				e = new Binary(e, op, ParseTerm());
			}
			return e;
		}

		private Expr ParseTerm()
		{
			var e = ParseFactor();
			while (Is("+") || Is("-"))
			{
				var op = _t.Lexeme;
				Eat();
				e = new Binary(e, op, ParseFactor());
			}
			return e;
		}

		private Expr ParseFactor()
		{
			var e = ParseUnary();
			while (Is("*") || Is("/"))
			{
				var op = _t.Lexeme;
				Eat();
				e = new Binary(e, op, ParseUnary());
			}
			return e;
		}

		private Expr ParseUnary()
		{
			if (Is("!") || Is("-"))
			{
				var op = _t.Lexeme;
				Eat();
				return new Unary(op, ParseUnary());
			}
			return ParsePrimary();
		}

		private Expr ParsePrimary()
		{
			if (_t.Kind == TokKind.Number)
			{
				var n = _t.Lexeme.Contains('.') ? double.Parse(_t.Lexeme) : int.Parse(_t.Lexeme);
				Eat();
				return new Literal(n);
			}

			if (_t.Kind == TokKind.String)
			{
				var s = _t.Lexeme;
				Eat();
				return new Literal(s);
			}

			if (_t.Kind == TokKind.Keyword && (Is("true") || Is("false")))
			{
				bool v = Is("true");
				Eat();
				return new Literal(v);
			}

			if (Is("("))
			{
				Eat();
				var e = ParseExpr();
				Expect(")");
				return e;
			}

			if (_t.Kind == TokKind.Identifier)
			{
				string id = _t.Lexeme;
				Eat();
				if (Is("("))
				{
					Eat();
					var args = new List<Expr>();
					if (!Is(")"))
					{
						args.Add(ParseExpr());
						while (Is(","))
						{
							Eat();
							args.Add(ParseExpr());
						}
					}
					Expect(")");
					return new CallExpr(id, args);
				}
				return new VarExpr(id);
			}

			throw new Exception($"Expresión no válida cerca de '{_t.Lexeme}'.");
		}
	}
}
