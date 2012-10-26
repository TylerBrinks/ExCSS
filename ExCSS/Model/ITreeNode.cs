using System.Collections.Generic;
using Antlr.Runtime.Tree;

namespace ExCSS.Model
{
    public interface ITreeNode
    {
        void Build(ITree tree);
    }

    public abstract class TreeNode : ITreeNode
    {
        public virtual void Build(ITree tree)
        {
            
        }

        internal List<T> BuildNodeList<T>(ITree tree, string childNodeType) where T : ITreeNode, new()
        {
            var nodes = tree.ChildrenOfType(childNodeType);
            var childList = new List<T>();

            foreach (var node in nodes)
            {
                var ruleset = CreateChildNode<T>(node);

                childList.Add(ruleset);
            }

            return childList;
        }

        internal T CreateChildNode<T>(ITree node) where T : ITreeNode, new()
        {
            var child = new T();
            child.Build(node);

            return child;
        }
    }
}