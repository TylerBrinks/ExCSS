using System.Collections.Generic;
using Antlr.Runtime.Tree;

namespace ExCSS.Model
{
    public class RuleSet : TreeNode
    {
        public RuleSet()
        {
            Selectors = new List<Selector>();
        }

        public override void Build(ITree tree)
        {
            Selectors.AddRange(BuildNodeList<Selector>(tree, NodeType.Selector));
        }

        public List<Selector> Selectors { get; private set; }
    }

    public class Selector : TreeNode
    {
        
    }
}