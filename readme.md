# ExCSS StyleSheet Parser for .NET - It's *BADA55!*

ExCSS (Pronounced Excess) is a CSS 2.1 and CSS 3 parser for .NET.

The goal of ExCSS is to make it easy to read and parse stylesheets into a friendly object model with full LINQ support.

# Version 4.0
Version 4 is a move forward in framework support.  The new version targets the latest version of .NET; Core 3.1 and 4.8.  The API surface is the same as version 3, but will target the future-facing, unified .NET version including the upcoming .NET 5. Version 3 was rewritten from the ground up; version 4 makes full use of those enhancements plus new additions under development!  This is the most advanced ExCSS parser to date.  The parser has been rebuild to have better white spaces support as well as the ability to handle unknown rule sets in the ever-changing web and CSS landscape.

# NuGet
[![NuGet Status](https://img.shields.io/nuget/v/excss.svg)](https://www.nuget.org/packages/excss/)

# Lexing and Parsing - How it all Works
ExCSS uses a Lexer and a Parser based on a CSS3-specific grammar. The Lexer and Parser read CSS text and parse each 
character as individual tokens run against a complex set of rules that define what CSS segment each token represents.  
Once parsed, the input styles sheet is turned into a standard .NET object model. That means it's fully queryable using Linq to objects.

## A basic example: 
```cs
var parser = new StylesheetParser();
var stylesheet = parser.Parse(".someClass{color: red; background-image: url('/images/logo.png')");

var rule = stylesheet.StyleRules.First() as StyleRule;
var selector = rule.SelectorText; // Yields .someClass
var color = rule.Style.Color; // rgb(255, 0, 0)
var image = rule.Style.BackgroundImage; // url('/images/logo.png')
```

## CSS 3 Compatible
The project has a growing suite of tests.  Currently the tests account for and pass all CSS Level 3 selector definitions
found in [the W3 CSS 3 Release Candidate documentation](http://www.w3.org/TR/2001/CR-css3-selectors-20011113/)



