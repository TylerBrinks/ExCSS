using ExCSS.Model;



// Generated
using System;

namespace ExCSS {



internal class Parser {
	public const int _EOF = 0;
	public const int _ident = 1;
	public const int _newline = 2;
	public const int _digit = 3;
	public const int _whitespace = 4;
	public const int maxT = 50;

	const bool T = true;
	const bool x = false;
	const int minErrDist = 2;
	
	internal Scanner scanner;
	internal Errors  errors;

	internal Token t;    // last recognized token
	internal Token la;   // lookahead token
	int errDist = minErrDist;

public Stylesheet Stylesheet;

		bool PartOfHex(string value) {
			if (value.Length == 7) { return false; }
			if (value.Length + la.val.Length > 7) { return false; }
			System.Collections.Generic.List<string> hexes = new System.Collections.Generic.List<string>(new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F", "a", "b", "c", "d", "e", "f" });
			foreach (char c in la.val) {
				if (!hexes.Contains(c.ToString())) {
					return false;
				}
			}
			return true;
		}
		bool IsUnit() {
			if (la.kind != 1) { return false; }
			System.Collections.Generic.List<string> units = new System.Collections.Generic.List<string>(new string[] { "em", "ex", "px", "gd", "rem", "vw", "vh", "vm", "ch", "mm", "cm", "in", "pt", "pc", "deg", "grad", "rad", "turn", "ms", "s", "hz", "khz" });
			return units.Contains(la.val.ToLower());
		}
		bool IsNumber() {
			if (la.val.Length > 0) {
				return char.IsDigit(la.val[0]);
			}
			return false;
		}

/*------------------------------------------------------------------------*
 *----- SCANNER DESCRIPTION ----------------------------------------------*
 *------------------------------------------------------------------------*/



	public Parser(Scanner scanner) {
		this.scanner = scanner;
		errors = new Errors();
	}

	void SynErr (int n) {
		if (errDist >= minErrDist) errors.SynErr(la.line, la.col, n);
		errDist = 0;
	}

	public void SemErr (string msg) {
		if (errDist >= minErrDist) errors.SemErr(t.line, t.col, msg);
		errDist = 0;
	}
	
	void Get () {
		for (;;) {
			t = la;
			la = scanner.Scan();
			if (la.kind <= maxT) { ++errDist; break; }

			la = t;
		}
	}
	
	void Expect (int n) {
		if (la.kind==n) Get(); else { SynErr(n); }
	}
	
	bool StartOf (int s) {
		return set[s, la.kind];
	}
	
	void ExpectWeak (int n, int follow) {
		if (la.kind == n) Get();
		else {
			SynErr(n);
			while (!StartOf(follow)) Get();
		}
	}


	bool WeakSeparator(int n, int syFol, int repFol) {
		int kind = la.kind;
		if (kind == n) {Get(); return true;}
		else if (StartOf(repFol)) {return false;}
		else {
			SynErr(n);
			while (!(set[syFol, kind] || set[repFol, kind] || set[0, kind])) {
				Get();
				kind = la.kind;
			}
			return StartOf(syFol);
		}
	}

	
	void Css3() {
		Stylesheet = new Stylesheet();
		string cset;
		RuleSet rset;
		Directive dir;
		
		while (la.kind == 4) {
			Get();
		}
		while (la.kind == 5 || la.kind == 6) {
			if (la.kind == 5) {
				Get();
			} else {
				Get();
			}
		}
		while (StartOf(1)) {
			if (StartOf(2)) {
				ruleset(out rset);
				Stylesheet.RuleSets.Add(rset); 
			} else {
				directive(out dir);
				Stylesheet.Directives.Add(dir); 
			}
			while (la.kind == 5 || la.kind == 6) {
				if (la.kind == 5) {
					Get();
				} else {
					Get();
				}
			}
			while (la.kind == 4) {
				Get();
			}
		}
	}

	void ruleset(out RuleSet rset) {
		rset = new RuleSet();
		Selector sel;
		Declaration dec;
		
		selector(out sel);
		rset.Selectors.Add(sel); 
		while (la.kind == 4) {
			Get();
		}
		while (la.kind == 25) {
			Get();
			while (la.kind == 4) {
				Get();
			}
			selector(out sel);
			rset.Selectors.Add(sel); 
			while (la.kind == 4) {
				Get();
			}
		}
		Expect(26);
		while (la.kind == 4) {
			Get();
		}
		if (StartOf(3)) {
			declaration(out dec);
			rset.Declarations.Add(dec); 
			while (la.kind == 4) {
				Get();
			}
			while (la.kind == 27) {
				Get();
				while (la.kind == 4) {
					Get();
				}
				if (la.val.Equals("}")) { Get(); return; }
				
				declaration(out dec);
				rset.Declarations.Add(dec); 
				while (la.kind == 4) {
					Get();
				}
			}
			if (la.kind == 27) {
				Get();
				while (la.kind == 4) {
					Get();
				}
			}
		}
		Expect(28);
		while (la.kind == 4) {
			Get();
		}
	}

	void directive(out Directive dir) {
		dir = new Directive();
		Declaration dec;
		RuleSet rset;
		Expression expression;
		Directive dr;
		string ident;
		Medium m;
		
		Expect(23);
		dir.Name = "@"; 
		if (la.kind == 24) {
			Get();
			dir.Name += "-"; 
		}
		identity(out ident);
		dir.Name += ident;
		switch (dir.Name.ToLower()) {
		case "@media": dir.Type = DirectiveType.Media; break;
		case "@import": dir.Type = DirectiveType.Import; break;
		case "@charset": dir.Type = DirectiveType.Charset; break;
		case "@page": dir.Type = DirectiveType.Page; break;
		case "@font-face": dir.Type = DirectiveType.FontFace; break;
		case "@namespace": dir.Type = DirectiveType.Namespace; break;
		default: dir.Type = DirectiveType.Other; break;
		}
		
		while (la.kind == 4) {
			Get();
		}
		if (StartOf(4)) {
			if (StartOf(5)) {
				medium(out m);
				dir.Mediums.Add(m); 
				while (la.kind == 4) {
					Get();
				}
				while (la.kind == 25) {
					Get();
					while (la.kind == 4) {
						Get();
					}
					medium(out m);
					dir.Mediums.Add(m); 
					while (la.kind == 4) {
						Get();
					}
				}
			} else {
				expr(out expression);
				dir.Expression = expression; 
				while (la.kind == 4) {
					Get();
				}
			}
		}
		if (la.kind == 26) {
			Get();
			while (la.kind == 4) {
				Get();
			}
			if (StartOf(6)) {
				while (StartOf(1)) {
					if (dir.Type == DirectiveType.Page || dir.Type == DirectiveType.FontFace) {
						declaration(out dec);
						dir.Declarations.Add(dec); 
						while (la.kind == 4) {
							Get();
						}
						while (la.kind == 27) {
							Get();
							while (la.kind == 4) {
								Get();
							}
							if (la.val.Equals("}")) { Get(); return; } 
							declaration(out dec);
							dir.Declarations.Add(dec); 
							while (la.kind == 4) {
								Get();
							}
						}
						if (la.kind == 27) {
							Get();
							while (la.kind == 4) {
								Get();
							}
						}
					} else if (StartOf(2)) {
						ruleset(out rset);
						dir.RuleSets.Add(rset); 
						while (la.kind == 4) {
							Get();
						}
					} else {
						directive(out dr);
						dir.Directives.Add(dr); 
						while (la.kind == 4) {
							Get();
						}
					}
				}
			}
			Expect(28);
			while (la.kind == 4) {
				Get();
			}
		} else if (la.kind == 27) {
			Get();
			while (la.kind == 4) {
				Get();
			}
		} else SynErr(51);
	}

	void QuotedString(out string qs) {
		qs = ""; 
		var quote = '\n'; 
		if (la.kind == 7) {
			Get();
			quote = '\''; 
			while (StartOf(7)) {
				Get();
				qs += t.val; 
				if (la.val.Equals("'") && !t.val.Equals("\\")) { break; } 
			}
			Expect(7);
		} else if (la.kind == 8) {
			Get();
			quote = '"'; 
			while (StartOf(8)) {
				Get();
				qs += t.val; 
				if (la.val.Equals("\"") && !t.val.Equals("\\")) { break; } 
			}
			Expect(8);
		} else SynErr(52);
	}

	void QuotedStringPreserved(out string qs) {
		qs = ""; 
		var quote = '\n'; 
		if (la.kind == 7) {
			Get();
			quote = '\''; 
			while (StartOf(7)) {
				Get();
				qs += t.val; 
				if (la.val.Equals("'") && !t.val.Equals("\\")) { break; } 
			}
			Expect(7);
			qs = "'" + qs + "'"; 
		} else if (la.kind == 8) {
			Get();
			quote = '"'; 
			while (StartOf(8)) {
				Get();
				qs += t.val; 
				if (la.val.Equals("\"") && !t.val.Equals("\\")) { break; } 
			}
			Expect(8);
			qs = '"' + qs + '"'; 
		} else SynErr(53);
	}

	void URI(out string url) {
		url = ""; 
		Expect(9);
		while (la.kind == 4) {
			Get();
		}
		if (la.kind == 10) {
			Get();
		}
		while (la.kind == 4) {
			Get();
		}
		if (la.kind == 7 || la.kind == 8) {
			QuotedString(out url);
		} else if (StartOf(9)) {
			while (StartOf(10)) {
				Get();
				url += t.val; 
				if (la.val.Equals(")")) { break; } 
			}
		} else SynErr(54);
		while (la.kind == 4) {
			Get();
		}
		if (la.kind == 11) {
			Get();
		}
	}

	void medium(out Medium m) {
		m = Medium.All; 
		switch (la.kind) {
		case 12: {
			Get();
			m = Medium.All; 
			break;
		}
		case 13: {
			Get();
			m = Medium.Aural; 
			break;
		}
		case 14: {
			Get();
			m = Medium.Braille; 
			break;
		}
		case 15: {
			Get();
			m = Medium.Embossed; 
			break;
		}
		case 16: {
			Get();
			m = Medium.Handheld; 
			break;
		}
		case 17: {
			Get();
			m = Medium.Print; 
			break;
		}
		case 18: {
			Get();
			m = Medium.Projection; 
			break;
		}
		case 19: {
			Get();
			m = Medium.Screen; 
			break;
		}
		case 20: {
			Get();
			m = Medium.Tty; 
			break;
		}
		case 21: {
			Get();
			m = Medium.Tv; 
			break;
		}
		default: SynErr(55); break;
		}
	}

	void identity(out string ident) {
		ident = ""; 
		switch (la.kind) {
		case 1: {
			Get();
			break;
		}
		case 22: {
			Get();
			break;
		}
		case 9: {
			Get();
			break;
		}
		case 12: {
			Get();
			break;
		}
		case 13: {
			Get();
			break;
		}
		case 14: {
			Get();
			break;
		}
		case 15: {
			Get();
			break;
		}
		case 16: {
			Get();
			break;
		}
		case 17: {
			Get();
			break;
		}
		case 18: {
			Get();
			break;
		}
		case 19: {
			Get();
			break;
		}
		case 20: {
			Get();
			break;
		}
		case 21: {
			Get();
			break;
		}
		default: SynErr(56); break;
		}
		ident += t.val; 
	}

	void expr(out Expression expression) {
		expression = new Expression();
		char? sep = null;
		Term trm = null;
		
		term(out trm);
		expression.Terms.Add(trm); 
		while (la.kind == 4) {
			Get();
		}
		while (StartOf(11)) {
			if (la.kind == 25 || la.kind == 47) {
				if (la.kind == 47) {
					Get();
					sep = '/'; 
				} else {
					Get();
					sep = ','; 
				}
				while (la.kind == 4) {
					Get();
				}
			}
			term(out trm);
			if (sep.HasValue) { trm.Seperator = sep.Value; }
			expression.Terms.Add(trm);
			sep = null;
			
			while (la.kind == 4) {
				Get();
			}
		}
	}

	void declaration(out Declaration dec) {
		dec = new Declaration();
		Expression expression;
		string ident = "";
		
		if (la.kind == 32) {
			Get();
			dec.Name += "*"; 
		}
		if (la.kind == 24) {
			Get();
			dec.Name += "-"; 
		}
		identity(out ident);
		dec.Name += ident; 
		while (la.kind == 4) {
			Get();
		}
		Expect(44);
		while (la.kind == 4) {
			Get();
		}
		expr(out expression);
		dec.Expression = expression; 
		while (la.kind == 4) {
			Get();
		}
		if (la.kind == 45) {
			Get();
			while (la.kind == 4) {
				Get();
			}
			Expect(46);
			dec.Important = true; 
			while (la.kind == 4) {
				Get();
			}
		}
	}

	void selector(out Selector sel) {
		sel = new Selector();
		SimpleSelector ss = null;
		Combinator? cb = null;
		
		simpleselector(out ss);
		sel.SimpleSelectors.Add(ss); 
		while (la.kind == 4) {
			Get();
		}
		while (StartOf(12)) {
			if (la.kind == 29 || la.kind == 30 || la.kind == 31) {
				if (la.kind == 29) {
					Get();
					cb = Combinator.PrecededImmediatelyBy; 
				} else if (la.kind == 30) {
					Get();
					cb = Combinator.ChildOf; 
				} else {
					Get();
					cb = Combinator.PrecededBy; 
				}
			}
			while (la.kind == 4) {
				Get();
			}
			simpleselector(out ss);
			if (cb.HasValue) { ss.Combinator = cb.Value; }
			sel.SimpleSelectors.Add(ss);
			
			cb = null; 
			while (la.kind == 4) {
				Get();
			}
		}
	}

	void simpleselector(out SimpleSelector ss) {
		ss = ss = new SimpleSelector {ElementName = ""};
		string psd;
		Model.Attribute attribute;
		var parent = ss;
		string ident;
		
		if (StartOf(13)) {
			if (la.kind == 24) {
				Get();
				ss.ElementName += "-"; 
			}
			identity(out ident);
			ss.ElementName += ident; 
		} else if (la.kind == 32) {
			Get();
			ss.ElementName = "*"; 
		} else if (la.kind == 33) {
			Get();
			ss.Combinator = Combinator.Namespace; 
		} else if (StartOf(14)) {
			if (la.kind == 34) {
				Get();
				if (la.kind == 24) {
					Get();
					ss.ID = "-"; 
				}
				identity(out ident);
				if (ss.ID == null) { ss.ID = ident; } else { ss.ID += ident; } 
			} else if (la.kind == 35) {
				Get();
				ss.Class = ""; 
				if (la.kind == 24) {
					Get();
					ss.Class += "-"; 
				}
				identity(out ident);
				ss.Class += ident; 
			} else if (la.kind == 36) {
				attrib(out attribute);
				ss.Attribute = attribute; 
			} else {
				pseudo(out psd);
				ss.Pseudo = psd; 
			}
		} else SynErr(57);
		while (StartOf(14)) {
			var child = new SimpleSelector(); 
			if (la.kind == 34) {
				Get();
				if (la.kind == 24) {
					Get();
					child.ID = "-"; 
				}
				identity(out ident);
				if (child.ID == null) { child.ID = ident; } else { child.ID += "-"; } 
			} else if (la.kind == 35) {
				Get();
				child.Class = ""; 
				if (la.kind == 24) {
					Get();
					child.Class += "-"; 
				}
				identity(out ident);
				child.Class += ident; 
			} else if (la.kind == 36) {
				attrib(out attribute);
				child.Attribute = attribute; 
			} else {
				pseudo(out psd);
				child.Pseudo = psd; 
			}
			parent.Child = child;
			parent = child;
			
		}
	}

	void attrib(out Model.Attribute attribute) {
		attribute = new Model.Attribute { Value = "" };
		string quote;
		string ident;
		
		Expect(36);
		while (la.kind == 4) {
			Get();
		}
		identity(out ident);
		attribute.Operand = ident; 
		while (la.kind == 4) {
			Get();
		}
		if (StartOf(15)) {
			switch (la.kind) {
			case 37: {
				Get();
				attribute.Operator = AttributeOperator.Equals; 
				break;
			}
			case 38: {
				Get();
				attribute.Operator = AttributeOperator.InList; 
				break;
			}
			case 39: {
				Get();
				attribute.Operator = AttributeOperator.Hyphenated; 
				break;
			}
			case 40: {
				Get();
				attribute.Operator = AttributeOperator.EndsWith; 
				break;
			}
			case 41: {
				Get();
				attribute.Operator = AttributeOperator.BeginsWith; 
				break;
			}
			case 42: {
				Get();
				attribute.Operator = AttributeOperator.Contains; 
				break;
			}
			}
			while (StartOf(16)) {
				while (la.kind == 4) {
					Get();
				}
				if (StartOf(17)) {
					if (la.kind == 24) {
						Get();
						attribute.Value += "-"; 
					}
					while (la.kind == 4) {
						Get();
					}
					identity(out ident);
					attribute.Value += ident; 
				} else if (la.kind == 7 || la.kind == 8) {
					QuotedStringPreserved(out quote);
					attribute.Value = quote; 
				} else SynErr(58);
				while (la.kind == 4) {
					Get();
				}
			}
		}
		Expect(43);
	}

	void pseudo(out string pseudo) {
		pseudo = "";
		Expression expression;
		string ident;
		
		Expect(44);
		if (la.kind == 44) {
			Get();
			pseudo += ":"; 
		}
		while (la.kind == 4) {
			Get();
		}
		if (la.kind == 24) {
			Get();
			pseudo += "-"; 
		}
		identity(out ident);
		pseudo += ident; 
		if (la.kind == 10) {
			Get();
			pseudo += t.val; 
			while (la.kind == 4) {
				Get();
			}
			expr(out expression);
			pseudo += expression.ToString(); 
			while (la.kind == 4) {
				Get();
			}
			Expect(11);
			pseudo += t.val; 
		}
	}

	void term(out Term trm) {
		trm = new Term();
		var val = "";
		Expression expression;
		string ident;
		
		if (la.kind == 7 || la.kind == 8) {
			QuotedString(out val);
			trm.Value = val; trm.Type = TermType.String; 
		} else if (la.kind == 9) {
			URI(out val);
			trm.Value = val; trm.Type = TermType.Url; 
		} else if (la.kind == 48) {
			Get();
			identity(out ident);
			trm.Value = "U\\" + ident; trm.Type = TermType.Unicode; 
		} else if (la.kind == 34) {
			HexValue(out val);
			trm.Value = val; trm.Type = TermType.Hex; 
		} else if (StartOf(18)) {
			bool minus = false; 
			if (la.kind == 24) {
				Get();
				minus = true; 
			}
			if (StartOf(19)) {
				identity(out ident);
				trm.Value = ident; trm.Type = TermType.String; 
				if (minus) { trm.Value = "-" + trm.Value; } 
				if (StartOf(20)) {
					while (la.kind == 35 || la.kind == 37 || la.kind == 44) {
						if (la.kind == 44) {
							Get();
							trm.Value += t.val; 
							if (StartOf(21)) {
								if (la.kind == 44) {
									Get();
									trm.Value += t.val; 
								}
								if (la.kind == 24) {
									Get();
									trm.Value += t.val; 
								}
								identity(out ident);
								trm.Value += ident; 
							} else if (la.kind == 34) {
								HexValue(out val);
								trm.Value += val; 
							} else if (StartOf(22)) {
								while (la.kind == 3) {
									Get();
									trm.Value += t.val; 
								}
								if (la.kind == 35) {
									Get();
									trm.Value += "."; 
									while (la.kind == 3) {
										Get();
										trm.Value += t.val; 
									}
								}
							} else SynErr(59);
						} else if (la.kind == 35) {
							Get();
							trm.Value += t.val; 
							if (la.kind == 24) {
								Get();
								trm.Value += t.val; 
							}
							identity(out ident);
							trm.Value += ident; 
						} else {
							Get();
							trm.Value += t.val; 
							if (la.kind == 24) {
								Get();
								trm.Value += t.val; 
							}
							if (StartOf(19)) {
								identity(out ident);
								trm.Value += ident; 
							} else if (StartOf(22)) {
								while (la.kind == 3) {
									Get();
									trm.Value += t.val; 
								}
							} else SynErr(60);
						}
					}
				}
				if (la.kind == 10) {
					Get();
					while (la.kind == 4) {
						Get();
					}
					expr(out expression);
					Function func = new Function();
					func.Name = trm.Value;
					func.Expression = expression;
					trm.Value = null;
					trm.Function = func;
					trm.Type = TermType.Function;
					
					while (la.kind == 4) {
						Get();
					}
					Expect(11);
				}
			} else if (StartOf(18)) {
				if (la.kind == 29) {
					Get();
					trm.Sign = '+'; 
				}
				if (minus) { trm.Sign = '-'; } 
				while (la.kind == 3) {
					Get();
					val += t.val; 
				}
				if (la.kind == 35) {
					Get();
					val += t.val; 
					while (la.kind == 3) {
						Get();
						val += t.val; 
					}
				}
				if (StartOf(23)) {
					if (la.val.ToLower().Equals("n")) {
						Expect(22);
						val += t.val; 
						if (la.kind == 24 || la.kind == 29) {
							if (la.kind == 29) {
								Get();
								val += t.val; 
							} else {
								Get();
								val += t.val; 
							}
							Expect(3);
							val += t.val; 
							while (la.kind == 3) {
								Get();
								val += t.val; 
							}
						}
					} else if (la.kind == 49) {
						Get();
						trm.Unit = Unit.Percent; 
					} else {
						if (IsUnit()) {
							identity(out ident);
							try {
							trm.Unit = (Unit)Enum.Parse(typeof(Unit), ident, true);
							} catch {
							errors.SemErr(t.line, t.col, string.Format("Unrecognized unit '{0}'", ident));
							}
							
						}
					}
				}
				trm.Value = val; trm.Type = TermType.Number; 
			} else SynErr(61);
		} else SynErr(62);
	}

	void HexValue(out string val) {
		val = "";
		var found = false;
		
		Expect(34);
		val += t.val; 
		if (StartOf(22)) {
			while (la.kind == 3) {
				Get();
				val += t.val; 
			}
		} else if (PartOfHex(val)) {
			Expect(1);
			val += t.val; found = true; 
		} else SynErr(63);
		if (!found && PartOfHex(val)) {
			Expect(1);
			val += t.val; 
		}
	}



	internal void Parse() {
		la = new Token();
		la.val = "";		
		Get();
		Css3();
		Expect(0);

	}
	
	static readonly bool[,] set = {
		{T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,T,x,x, x,x,x,x, x,T,x,x, T,T,T,T, T,T,T,T, T,T,T,T, T,x,x,x, x,x,x,x, T,T,T,T, T,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x},
		{x,T,x,x, x,x,x,x, x,T,x,x, T,T,T,T, T,T,T,T, T,T,T,x, T,x,x,x, x,x,x,x, T,T,T,T, T,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x},
		{x,T,x,x, x,x,x,x, x,T,x,x, T,T,T,T, T,T,T,T, T,T,T,x, T,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,T,x,T, T,x,x,T, T,T,x,x, T,T,T,T, T,T,T,T, T,T,T,x, T,T,T,T, x,T,x,x, x,x,T,T, x,x,x,x, x,x,x,x, x,x,x,T, T,T,x,x},
		{x,x,x,x, x,x,x,x, x,x,x,x, T,T,T,T, T,T,T,T, T,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,T,x,x, x,x,x,x, x,T,x,x, T,T,T,T, T,T,T,T, T,T,T,T, T,x,x,x, T,x,x,x, T,T,T,T, T,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x},
		{x,T,T,T, T,T,T,x, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,x},
		{x,T,T,T, T,T,T,T, x,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,x},
		{x,T,T,T, T,T,T,T, T,T,x,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,x},
		{x,T,T,T, x,T,T,x, x,T,x,x, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,x},
		{x,T,x,T, T,x,x,T, T,T,x,x, T,T,T,T, T,T,T,T, T,T,T,x, T,T,x,x, x,T,x,x, x,x,T,T, x,x,x,x, x,x,x,x, x,x,x,T, T,T,x,x},
		{x,T,x,x, T,x,x,x, x,T,x,x, T,T,T,T, T,T,T,T, T,T,T,x, T,x,x,x, x,T,T,T, T,T,T,T, T,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x},
		{x,T,x,x, x,x,x,x, x,T,x,x, T,T,T,T, T,T,T,T, T,T,T,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,T, T,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,T,T, T,T,T,x, x,x,x,x, x,x,x,x},
		{x,T,x,x, T,x,x,T, T,T,x,x, T,T,T,T, T,T,T,T, T,T,T,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,T,x,x, T,x,x,x, x,T,x,x, T,T,T,T, T,T,T,T, T,T,T,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,T,x,T, T,x,x,T, T,T,x,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,x,x, T,T,T,T, T,x,x,x, x,x,x,x, T,T,x,T, T,T,x,x},
		{x,T,x,x, x,x,x,x, x,T,x,x, T,T,T,T, T,T,T,T, T,T,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,T,x,x, x,x,x,x, T,x,x,x, x,x,x,x},
		{x,T,x,x, x,x,x,x, x,T,x,x, T,T,T,T, T,T,T,T, T,T,T,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x},
		{x,T,x,T, T,x,x,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,x,x, T,T,T,T, T,T,x,x, x,x,x,x, T,T,x,T, T,T,x,x},
		{x,T,x,x, x,x,x,x, x,T,x,x, T,T,T,T, T,T,T,T, T,T,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x}

	};
} // end Parser


internal class Errors {
	internal int count = 0;                                    // number of errors detected
	internal System.IO.TextWriter errorStream = Console.Out;   // error messages go to this stream
	internal string errMsgFormat = "-- line {0} col {1}: {2}"; // 0=line, 1=column, 2=text

	internal virtual void SynErr (int line, int col, int n) {
		string s;
		switch (n) {
			case 0: s = "EOF expected"; break;
			case 1: s = "ident expected"; break;
			case 2: s = "newline expected"; break;
			case 3: s = "digit expected"; break;
			case 4: s = "whitespace expected"; break;
			case 5: s = "\"<!--\" expected"; break;
			case 6: s = "\"-->\" expected"; break;
			case 7: s = "\"\'\" expected"; break;
			case 8: s = "\"\"\" expected"; break;
			case 9: s = "\"url\" expected"; break;
			case 10: s = "\"(\" expected"; break;
			case 11: s = "\")\" expected"; break;
			case 12: s = "\"all\" expected"; break;
			case 13: s = "\"aural\" expected"; break;
			case 14: s = "\"braille\" expected"; break;
			case 15: s = "\"embossed\" expected"; break;
			case 16: s = "\"handheld\" expected"; break;
			case 17: s = "\"print\" expected"; break;
			case 18: s = "\"projection\" expected"; break;
			case 19: s = "\"screen\" expected"; break;
			case 20: s = "\"tty\" expected"; break;
			case 21: s = "\"tv\" expected"; break;
			case 22: s = "\"n\" expected"; break;
			case 23: s = "\"@\" expected"; break;
			case 24: s = "\"-\" expected"; break;
			case 25: s = "\",\" expected"; break;
			case 26: s = "\"{\" expected"; break;
			case 27: s = "\";\" expected"; break;
			case 28: s = "\"}\" expected"; break;
			case 29: s = "\"+\" expected"; break;
			case 30: s = "\">\" expected"; break;
			case 31: s = "\"~\" expected"; break;
			case 32: s = "\"*\" expected"; break;
			case 33: s = "\"|\" expected"; break;
			case 34: s = "\"#\" expected"; break;
			case 35: s = "\".\" expected"; break;
			case 36: s = "\"[\" expected"; break;
			case 37: s = "\"=\" expected"; break;
			case 38: s = "\"~=\" expected"; break;
			case 39: s = "\"|=\" expected"; break;
			case 40: s = "\"$=\" expected"; break;
			case 41: s = "\"^=\" expected"; break;
			case 42: s = "\"*=\" expected"; break;
			case 43: s = "\"]\" expected"; break;
			case 44: s = "\":\" expected"; break;
			case 45: s = "\"!\" expected"; break;
			case 46: s = "\"important\" expected"; break;
			case 47: s = "\"/\" expected"; break;
			case 48: s = "\"U\\\\\" expected"; break;
			case 49: s = "\"%\" expected"; break;
			case 50: s = "??? expected"; break;
			case 51: s = "invalid directive"; break;
			case 52: s = "invalid QuotedString"; break;
			case 53: s = "invalid QuotedStringPreserved"; break;
			case 54: s = "invalid URI"; break;
			case 55: s = "invalid medium"; break;
			case 56: s = "invalid identity"; break;
			case 57: s = "invalid simpleselector"; break;
			case 58: s = "invalid attrib"; break;
			case 59: s = "invalid term"; break;
			case 60: s = "invalid term"; break;
			case 61: s = "invalid term"; break;
			case 62: s = "invalid term"; break;
			case 63: s = "invalid HexValue"; break;

			default: s = "error " + n; break;
		}
		errorStream.WriteLine(errMsgFormat, line, col, s);
		count++;
	}

	internal virtual void SemErr (int line, int col, string s) {
		errorStream.WriteLine(errMsgFormat, line, col, s);
		count++;
	}
	
	internal virtual void SemErr (string s) {
		errorStream.WriteLine(s);
		count++;
	}
	
	internal virtual void Warning (int line, int col, string s) {
		errorStream.WriteLine(errMsgFormat, line, col, s);
	}
	
	internal virtual void Warning(string s) {
		errorStream.WriteLine(s);
	}
} // Errors


internal class FatalError: Exception {
	internal FatalError(string m): base(m) {}
}
}