#Stylesheet Parser for .NET#

StylesheetParser .NET for parsing CSS2 and CSS3 style sheet documents.

The goal of StylesheetParser is to make it easy to parse read stylesheets into a friendly object model with full LINQ support.

Author: 
Tyler Brinks - tyler.brinks [at] gmail

Follow: [@tylerbrinks](http://twitter.com/tylerbrinks) on Twitter.

#Original Work#
StylesheetParser is based on the original work by Scott Green, and is used with his expressed permission
http://www.codeproject.com/Articles/20450/Simple-CSS-Parser?msg=4179839#xx4179839xx

##A basic namespace example parsing a stylesheet document:##

	var parser = StylesheetParser();
	var stylesheet = parser.Parse();

##Advanced Stuff##
