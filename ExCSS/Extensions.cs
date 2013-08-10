//using System.Collections.Generic;
//using System.Linq;
//using Antlr.Runtime.Tree;

//namespace ExCSS
//{
//    public static class Extensions
//    {
//        //public static string ToUnitString(this Unit unit)
//        //{
//        //    switch (unit)
//        //    {
//        //        case Unit.Percent:
//        //            return "%";

//        //        case Unit.Khz:
//        //        case Unit.Hz:

//        //            return unit.ToString();

//        //        case Unit.None: // Account for empty units.  i.e. border: 10px 0 0 5px;
//        //            return " ";
//        //    }

//        //    return unit.ToString().ToLower();
//        //}

//        public static List<ITree> AllChildren(this ITree tree)
//        {
//            var children = new List<ITree>();

//            for (var i = 0; i < tree.ChildCount; i++)
//            {
//                children.Add(tree.GetChild(i));
//            }

//            return children;
//        }

//        public static List<ITree> ChildrenOfType(this ITree tree, string type)
//        {
//            return tree.AllChildren().Where(t => t.Text == type).ToList();
//        }

//        public static ITree FirstChildOfType(this ITree tree, string type)
//        {
//            return tree.AllChildren().FirstOrDefault(t => t.Text == type);
//        }
//    }
//}
