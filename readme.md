#ExCSS Parser for .NET#

ExCSS (Pronoundec Excess) is a CSS 2.1 and CSS 3 parser for .NET.

The goal of ExCSS is to make it easy to read and parse stylesheets into a friendly object model with full LINQ support.

Author: 
Tyler Brinks - tyler.brinks [at] gmail

Follow: [@tylerbrinks](http://twitter.com/tylerbrinks) on Twitter.

#Original Work#
ExCSS is based on the original work by Scott Green, and is used with his expressed permission
http://www.codeproject.com/Articles/20450/Simple-CSS-Parser?msg=4179839#xx4179839xx

#Lexing and Parsing#
ExCSS

##A basic example:##

	var parser = StylesheetParser();
	var stylesheet = parser.Parse(".someClass{color: red; border: solid 1px red; background-image: url('/images/logo.png')");
	
	var imageUrl = var image = parsed.RuleSets
                .SelectMany(r => r.Declarations)
                .SelectMany(d => d.Expression.Terms)
                .Where(t => t.Type == TermType.Url)
                .First(); // Finds the ''images/logo.png' image
				
##Does it Work?##
The project has a growing suite of tests.  Currently the tests account for and pass all CSS Level 3 selector definitions
found in [the W3 CSS 3 Release Candidate documentation](http://www.w3.org/TR/2001/CR-css3-selectors-20011113/)

Please consider helping this project out by helping our test list grow to match any valid CSS selector we're missing.

##Advanced Stuff##
ExCSS uses [Coco/r](http://http://ssw.jku.at/Coco/) to build a Lexer and Parser based on a CSS3 compatible grammar.  
The grammar defines valid CSS3 directives, selectors, rules, attributes, elements, and types that make up the body
ofa style sheet.

Coco/r is responsible for translating the grammar into usable C#.  The grammar is a living document; the goal of
this project is to keep it up to date wtih the CSS Level 3 specifications.

The grammar is an Attributed Grammar (ATG) based on an Extended Backus-Naur Form (EBNF).


