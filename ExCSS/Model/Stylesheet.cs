using System.Collections.Generic;
using Antlr.Runtime.Tree;

namespace ExCSS.Model
{
    /// <summary>
    /// Represents a stylesheet with a list of rules
    /// </summary>
    public class Stylesheet : TreeNode
    {
        public Stylesheet()
        {
            RuleSets = new List<RuleSet>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tree"></param>
        public override void Build(ITree tree)
        {
            RuleSets.AddRange(BuildNodeList<RuleSet>(tree, NodeType.RuleSet));
        }

        /// <summary>
        /// Gets the rule sets.
        /// </summary>
        public List<RuleSet> RuleSets { get; private set; }
    }
}