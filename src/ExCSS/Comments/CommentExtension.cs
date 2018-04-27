using System;
using System.Collections.Generic;
using System.Linq;

namespace ExCSS
{
    public static class CommentExtensions
    {
        public static IEnumerable<Comment> GetComments(this StylesheetNode node)
        {
            return node.GetAll<Comment>();
        }

        private static IEnumerable<T> GetAll<T>(this IStylesheetNode node)
            where T : IStyleFormattable
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            if (node is T)
            {
                yield return (T)node;
            }

            foreach (var entity in node.Children.SelectMany(m => m.GetAll<T>()))
            {
                yield return entity;
            }
        }
    }
}