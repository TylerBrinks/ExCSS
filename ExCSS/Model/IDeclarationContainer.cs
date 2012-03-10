using System.Collections.Generic;

namespace ExCSS.Model 
{
	public interface IDeclarationContainer 
    {
		List<Declaration> Declarations { get; }
	}
}