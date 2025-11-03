using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transpiler.Core.Lexing
{
	public enum TokKind { Identifier, Number, String, Keyword, Symbol, Eof }

	public record Token(TokKind Kind, string Lexeme, int Line, int Col);

	public sealed class Lexer
	{
		private readonly string _src;
		private int _i;
		private int _line = 1;
		private int _col = 1;

		private static readonly HashSet<string> _kw = new(new[]
		{
			"func","var","int","double","bool","string",
			"if","else","while","return","true","false","print"
		});

		public Lexer(string src) => _src = src ?? string.Empty;

		public Token Next()
		{
			Skip();
			if (Eof) return new Token(TokKind.Eof, string.Empty, _line, _col);

			char c = _src[_i];
			if (char.IsLetter(c) || c == '_') return Identifier();
			if (char.IsDigit(c)) return Number();
			if (c == '"') return String();
			return Symbol();
		}

		private bool Eof => _i >= _src.Length;

		private void Skip()
		{
			while (!Eof)
			{
				char c = _src[_i];
				if (char.IsWhiteSpace(c)) { Adv(); continue; }
				if (c == '/' && Peek() == '/')
				{
					while (!Eof && _src[_i] != '\n') Adv();
					continue;
				}
				break;
			}
		}

		private void Adv()
		{
			if (_src[_i] == '\n') { _line++; _col = 1; }
			else _col++;
			_i++;
		}

		private char Peek(int k = 1) => _i + k < _src.Length ? _src[_i + k] : '\0';

		private Token Identifier()
		{
			int s = _i;
			while (!Eof && (char.IsLetterOrDigit(_src[_i]) || _src[_i] == '_')) Adv();
			var lex = _src[s.._i];
			return _kw.Contains(lex)
				? new Token(TokKind.Keyword, lex, _line, _col)
				: new Token(TokKind.Identifier, lex, _line, _col);
		}

		private Token Number()
		{
			int s = _i; bool dot = false;
			while (!Eof && (char.IsDigit(_src[_i]) || (!dot && _src[_i] == '.')))
			{
				if (_src[_i] == '.') dot = true;
				Adv();
			}
			return new Token(TokKind.Number, _src[s.._i], _line, _col);
		}

		private Token String()
		{
			Adv(); // consume quote
			int s = _i;
			while (!Eof && _src[_i] != '"') Adv();
			var text = _src[s.._i];
			if (!Eof) Adv(); // closing quote
			return new Token(TokKind.String, text, _line, _col);
		}

		private Token Symbol()
		{
			string two = !Eof ? _src[_i].ToString() + Peek() : "";
			string[] d = { "==", "!=", "<=", ">=" };
			if (d.Contains(two)) { Adv(); Adv(); return new Token(TokKind.Symbol, two, _line, _col); }
			var ch = _src[_i];
			Adv();
			return new Token(TokKind.Symbol, ch.ToString(), _line, _col);
		}
	}
}
