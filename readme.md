#ExCSS StyleSheet Parser for .NET - It's \#BADA55!#

ExCSS (Pronoundec Excess) is a CSS 2.1 and CSS 3 parser for .NET.

The goal of ExCSS is to make it easy to read and parse stylesheets into a friendly object model with full LINQ support.

#Original Work#
ExCSS is based on the original work by Scott Green, and is used with his expressed permission
http://www.codeproject.com/Articles/20450/Simple-CSS-Parser?msg=4179839#xx4179839xx

#NuGet#
Install the pagckage from the NuGet Package Manager or via command line

	Install-Package ExCSS 

#Lexing and Parsing - How it all Wors#
ExCSS builds a Scanner (a.k.a. a Lexer) and a Parser based on a CSS3-specific grammar.  The Scanner and Parser
are generated using [Coco/r](http://http://ssw.jku.at/Coco/), and generate .NET code required to parse any common
stylesheet.  Once parsed, the input styles sheet is turned into a sandard .NET object model.  That means it's
fully queryable using Linq to objects.

##A basic example:##

	var parser = StylesheetParser();
	var stylesheet = parser.Parse(".someClass{color: red; background-image: url('/images/logo.png')");
	
	var imageUrl = var image = parsed.RuleSets
                .SelectMany(r => r.Declarations)
                .SelectMany(d => d.Expression.Terms)
                .Where(t => t.Type == TermType.Url)
                .First(); // Finds the '/images/logo.png' image
				
##CSS 3 Compatible##
The project has a growing suite of tests.  Currently the tests account for and pass all CSS Level 3 selector definitions
found in [the W3 CSS 3 Release Candidate documentation](http://www.w3.org/TR/2001/CR-css3-selectors-20011113/)

Take a look at the currently tested selectors: https://github.com/TylerBrinks/ExCSS/wiki/CSS-3-Selector-Test-Status

*Please consider helping* this project out by helping our test list grow to match any valid CSS selector we're missing.

##Advanced Stuff##
ExCSS uses [Coco/r](http://http://ssw.jku.at/Coco/) to build a Lexer and Parser based on a CSS3 compatible grammar.  
The grammar defines valid CSS3 directives, selectors, rules, attributes, elements, and types that make up the body
ofa style sheet.

Coco/r is responsible for translating the grammar into usable C#.  The grammar is a living document; the goal of
this project is to keep it up to date wtih the CSS Level 3 specifications as they evolve.

The grammar is an Attributed Grammar (ATG) based on an Extended Backus-Naur Form (EBNF).  That means the output
parser allows for injecting C# coding conventions.  In the case of ExCSS, that means an entire object model can 
be constructed from simple and isolated grammar-based rules.

Take a look at the css.v3.02.1.atg file for the full grammar


