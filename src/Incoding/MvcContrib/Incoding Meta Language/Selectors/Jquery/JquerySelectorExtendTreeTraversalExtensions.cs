namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using Incoding.Extensions;

    #endregion

    public static class JquerySelectorExtendTreeTraversalExtensions
    {
        #region Add

        /// <summary>
        ///     Add elements to the set of matched elements.
        /// </summary>
        /// <param name="original"></param>
        /// <param name="action">
        ///     action expression to match elements against.
        /// </param>
        public static JquerySelectorExtend Add(this JquerySelectorExtend original, Func<JquerySelector, JquerySelector> action)
        {
            return AddTree(original, action, "add");
        }

        /// <summary>
        ///     Add elements to the set of matched elements.
        /// </summary>
        /// <param name="original"></param>
        /// <param name="bOfClass">
        ///     bootstrap of classes
        /// </param>
        public static JquerySelectorExtend Add(this JquerySelectorExtend original, B bOfClass)
        {
            return original.Add(r => r.Class(bOfClass));
        }

        /// <summary>
        ///     Add elements to the set of matched elements.
        /// </summary>
        /// <param name="original"></param>
        /// <param name="selector">
        ///     selector to match elements against.
        /// </param>
        public static JquerySelectorExtend Add(this JquerySelectorExtend original, JquerySelectorExtend selector)
        {
            return AddTree(original, jquerySelector => selector, "add");
        }

        #endregion

        #region Children

        /// <summary>
        ///     Get the children of each element in the set of matched elements, optionally filtered by a action.
        /// </summary>
        /// <param name="original"></param>
        /// <param name="action">
        ///     action expression to match elements against.
        /// </param>
        public static JquerySelectorExtend Children(this JquerySelectorExtend original, Func<JquerySelector, JquerySelector> action)
        {
            return AddTree(original, action, "children");
        }

        /// <summary>
        ///     Get the children of each element in the set of tag, optionally filtered by a action.
        /// </summary>
        /// <param name="original"></param>
        /// <param name="tag">
        ///     Html tag
        /// </param>
        public static JquerySelectorExtend Children(this JquerySelectorExtend original, HtmlTag tag)
        {
            return original.Children(selector => selector.Tag(tag));
        }

        /// <summary>
        ///     Get the children of each element
        /// </summary>
        /// <param name="original"></param>
        public static JquerySelectorExtend Children(this JquerySelectorExtend original)
        {
            return original.Children(null);
        }

        #endregion

        #region Closest

        /// <summary>
        ///     For each element in the set, get the first element that matches the action by testing the element itself and
        ///     traversing up through its ancestors in the DOM tree.
        /// </summary>
        /// <param name="original"></param>
        /// <param name="action">
        ///     action expression to match elements against.
        /// </param>
        public static JquerySelectorExtend Closest(this JquerySelectorExtend original, Func<JquerySelector, JquerySelector> action)
        {
            return AddTree(original, action, "closest");
        }

        /// <summary>
        ///     For each element in the set, get the first element that tag the action by testing the element itself and traversing
        ///     up through its ancestors in the DOM tree.
        /// </summary>
        /// <param name="original">
        /// </param>
        /// <param name="tag">
        ///     Html tag
        /// </param>
        public static JquerySelectorExtend Closest(this JquerySelectorExtend original, HtmlTag tag)
        {
            return original.Closest(selector => selector.Tag(tag));
        }

        /// <summary>
        ///     For each element in the set, get the first element that tag the action by testing the element itself and traversing
        ///     up through its ancestors in the DOM tree.
        /// </summary>
        /// <param name="original">
        /// </param>
        /// <param name="classes">
        ///     Bootstrapp classes
        /// </param>
        public static JquerySelectorExtend Closest(this JquerySelectorExtend original, B classes)
        {
            return original.Closest(selector => selector.Class(classes));
        }

        /// <summary>
        ///     For each element in the set, get the first element that tag the action by testing the element itself and traversing
        ///     up through its ancestors in the DOM tree.
        /// </summary>
        /// <param name="original">
        /// </param>
        /// <param name="selector">
        ///     Selector
        /// </param>
        public static JquerySelectorExtend Closest(this JquerySelectorExtend original, JquerySelector selector)
        {
            return original.Closest(r => selector);
        }

        /// <summary>
        ///     For each element in the set, get the first element that expression the action by testing the element itself and
        ///     traversing up through its ancestors in the DOM tree.
        /// </summary>
        /// <param name="original">
        /// </param>
        /// <param name="expression">
        ///     Jquery expression
        /// </param>
        public static JquerySelectorExtend Closest(this JquerySelectorExtend original, JqueryExpression expression)
        {
            return original.Closest(selector => selector.Expression(expression));
        }

        #endregion
        
        #region Filter

        /// <summary>
        ///     Reduce the set of matched elements to those that match the action or pass the function's test.
        /// </summary>
        /// <param name="original"></param>
        /// <param name="action">
        ///     action expression to match elements against.
        /// </param>
        public static JquerySelectorExtend Filter(this JquerySelectorExtend original, Func<JquerySelector, JquerySelector> action)
        {
            return AddTree(original, action, "filter");
        }

        /// <summary>
        ///     Reduce the set of matched elements to those that match the action or pass the function's test.
        /// </summary>
        /// <param name="original"></param>
        /// <param name="selector">
        ///     selector to match elements against.
        /// </param>
        public static JquerySelectorExtend Filter(this JquerySelectorExtend original, JquerySelector selector)
        {
            return AddTree(original, jquerySelector => selector, "filter");
        }

        /// <summary>
        ///     Reduce the set of tag to those that match the action or pass the function's test.
        /// </summary>
        public static JquerySelectorExtend Filter(this JquerySelectorExtend original, HtmlTag tag)
        {
            return original.Filter(selector => selector.Tag(tag));
        }

        /// <summary>
        ///     Reduce the set of expression to those that match the action or pass the function's test.
        /// </summary>
        public static JquerySelectorExtend Filter(this JquerySelectorExtend original, JqueryExpression expression)
        {
            return original.Filter(selector => selector.Expression(expression));
        }


        /// <summary>
        ///     Reduce the set of expression to those that match the action or pass the function's test.
        /// </summary>
        public static JquerySelectorExtend Filter(this JquerySelectorExtend original, B bOfClass)
        {
            return original.Filter(selector => selector.Class(bOfClass));
        }

        #endregion

        #region Find
        
        /// <summary>
        ///     Get the descendants of each element in the current set of matched elements, filtered by a action, jQuery object, or
        ///     element
        /// </summary>
        /// <param name="original"></param>
        /// <param name="action">
        ///     action expression to match elements against.
        /// </param>
        public static JquerySelectorExtend Find(this JquerySelectorExtend original, Func<JquerySelector, JquerySelector> action)
        {
            return AddTree(original, action, "find");
        }

        /// <summary>
        ///     Get the descendants of each element in the current set of tag, filtered by a action, jQuery object, or element
        /// </summary>
        /// <param name="original"></param>
        /// <param name="tag">
        ///     Html tag
        /// </param>
        public static JquerySelectorExtend Find(this JquerySelectorExtend original, HtmlTag tag)
        {
            return original.Find(selector => selector.Tag(tag));
        }

        /// <summary>
        ///     Get the descendants of each element in the current set of tag, filtered by a action, jQuery object, or element
        /// </summary>
        /// <param name="original"></param>
        /// <param name="B">
        ///     Bootstrapp class
        /// </param>
        public static JquerySelectorExtend Find(this JquerySelectorExtend original, B b)
        {
            return original.Find(selector => selector.Class(b));
        }

        /// <summary>
        ///     Get the descendants of each element in the current set of tag, filtered by a action, jQuery object, or element
        /// </summary>
        /// <param name="original"></param>
        /// <param name="expression">
        ///     Jquery expression
        /// </param>
        public static JquerySelectorExtend Find(this JquerySelectorExtend original, JqueryExpression expression)
        {
            return original.Find(selector => selector.Expression(expression));
        }

        #endregion

        #region All Next

        /// <summary>
        ///     Get the descendants of each element in the current set of matched elements, filtered by a action, jQuery object, or
        ///     element
        /// </summary>
        /// <param name="original"></param>
        /// <param name="action">
        ///     action expression to match elements against.
        /// </param>
        public static JquerySelectorExtend Next(this JquerySelectorExtend original, Func<JquerySelector, JquerySelector> action)
        {
            return AddTree(original, action, "next");
        }

        /// <summary>
        ///     Get the descendants of each element in the current set of matched elements, filtered by a action, jQuery object, or
        ///     element
        /// </summary>
        /// <param name="original"></param>
        /// <param name="tag">
        ///     Html Tag
        /// </param>
        public static JquerySelectorExtend Next(this JquerySelectorExtend original, HtmlTag tag)
        {
            return original.Next(s => s.Tag(tag));
        }

        /// <summary>
        ///     Get the descendants of each element in the current set of matched elements, filtered by a action, jQuery object, or
        ///     element
        /// </summary>
        /// <param name="original"></param>
        /// <param name="class">
        ///     Bootstrap classes
        /// </param>
        public static JquerySelectorExtend Next(this JquerySelectorExtend original, B @class)
        {
            return original.Next(s => s.Class(@class));
        }

        /// <summary>
        ///     Get the descendants of each element in the current set of matched elements, filtered by a action, jQuery object, or
        ///     element
        /// </summary>
        public static JquerySelectorExtend Next(this JquerySelectorExtend original)
        {
            return original.Next(null);
        }

        /// <summary>
        ///     Get all following siblings of each element in the set of matched elements, optionally filtered by a action.
        /// </summary>
        /// <param name="original"></param>
        /// <param name="action">
        ///     action expression to match elements against.
        /// </param>
        public static JquerySelectorExtend NextAll(this JquerySelectorExtend original, Func<JquerySelector, JquerySelector> action)
        {
            return AddTree(original, action, "nextAll");
        }

        /// <summary>
        ///     Get all following siblings of each element in the set of matched elements, optionally filtered by a action.
        /// </summary>
        public static JquerySelectorExtend NextAll(this JquerySelectorExtend original)
        {
            return original.NextAll(null);
        }

        /// <summary>
        ///     Get all following siblings of each element up to but not including the element matched by the action, DOM node, or
        ///     jQuery object passed.
        /// </summary>
        /// <param name="original"></param>
        /// <param name="action">
        ///     action expression to match elements against.
        /// </param>
        public static JquerySelectorExtend NextUntil(this JquerySelectorExtend original, Func<JquerySelector, JquerySelector> action)
        {
            return AddTree(original, action, "nextUntil");
        }

        /// <summary>
        ///     Get all following siblings of each element up to but not including the element matched by the action, DOM node, or
        ///     jQuery object passed.
        /// </summary>
        public static JquerySelectorExtend NextUntil(this JquerySelectorExtend original)
        {
            return original.NextUntil(null);
        }

        #endregion

        #region Not

        /// <summary>
        ///     Selects all elements that do not match the given <paramref name="action" />.
        /// </summary>
        public static JquerySelectorExtend Not(this JquerySelectorExtend original, Func<JquerySelector, JquerySelector> action)
        {
            return AddTree(original, action, "not");
        }

        /// <summary>
        ///     Selects all elements that do not match the given <paramref name="action" />.
        /// </summary>
        public static JquerySelectorExtend Not(this JquerySelectorExtend original, JqueryExpression expression)
        {
            return original.Not(r => r.Expression(expression));
        }

        /// <summary>
        ///     Selects all elements that do not match the given <paramref name="selector" />.
        /// </summary>
        public static JquerySelectorExtend Not(this JquerySelectorExtend original, JquerySelectorExtend selector)
        {
            return AddTree(original, jquerySelector => selector, "not");
        }

        /// <summary>
        ///     Selects all elements that do not match the given <paramref name="action" />.
        /// </summary>
        public static JquerySelectorExtend Not(this JquerySelectorExtend original, HtmlTag tag)
        {
            return original.Not(r => r.Tag(tag));
        }

        #endregion
  
        #region Parent

        /// <summary>
        ///     Get the closest ancestor element that is positioned.
        /// </summary>
        public static JquerySelectorExtend OffsetParent(this JquerySelectorExtend original)
        {
            return AddTree(original, null, "offsetParent");
        }

        /// <summary>
        ///     Get the parent of each element in the current set of matched elements, optionally filtered by a action.
        /// </summary>
        /// <param name="original"></param>
        /// <param name="action">
        ///     action expression to match elements against.
        /// </param>
        public static JquerySelectorExtend Parent(this JquerySelectorExtend original, Func<JquerySelector, JquerySelector> action)
        {
            return AddTree(original, action, "parent");
        }

        /// <summary>
        ///     Get the parent of each element in the current set of matched elements, optionally filtered by a action.
        /// </summary>
        /// <param name="original">
        /// </param>
        public static JquerySelectorExtend Parent(this JquerySelectorExtend original)
        {
            return AddTree(original, null, "parent");
        }




        /// <summary>
        ///     Get the parent of each element in the current set of tag, optionally filtered by a action.
        /// </summary>
        /// <param name="original"></param>
        /// <param name="tag">
        ///     Tag
        /// </param>
        public static JquerySelectorExtend Parent(this JquerySelectorExtend original, HtmlTag tag)
        {
            return original.Parent(selector => selector.Tag(tag));
        }

        /// <summary>
        ///     Get the parent of each element in the current set of tag, optionally filtered by a action.
        /// </summary>
        /// <param name="original"></param>
        /// <param name="class">
        ///     Bootstrap classes
        /// </param>
        public static JquerySelectorExtend Parent(this JquerySelectorExtend original, B @class)
        {
            return original.Parent(selector => selector.Class(@class));
        }


        /// <summary>
        ///     Get the parents of each element in the current set of tag, optionally filtered by a action.
        /// </summary>
        /// <param name="original"></param>
        /// <param name="tag">
        ///     Tag
        /// </param>
        public static JquerySelectorExtend Parents(this JquerySelectorExtend original, HtmlTag tag)
        {
            return original.Parents(selector => selector.Tag(tag));
        }

        /// <summary>
        ///     Get the parents of each element in the current set of matched elements, optionally filtered by a action.
        /// </summary>
        /// <param name="original"></param>
        /// <param name="action">
        ///     action expression to match elements against.
        /// </param>
        public static JquerySelectorExtend Parents(this JquerySelectorExtend original, Func<JquerySelector, JquerySelector> action)
        {
            return AddTree(original, action, "parents");
        }

        /// <summary>
        ///     Get the parents of each element in the current set of matched elements, optionally filtered by a action.
        /// </summary>
        /// <param name="original"></param>
        public static JquerySelectorExtend Parents(this JquerySelectorExtend original)
        {
            return AddTree(original, null, "parents");
        }

        /// <summary>
        ///     Get the ancestors of each element in the current set of matched elements, up to but not including the element
        ///     matched by the action, DOM node, or jQuery object
        /// </summary>
        /// <param name="original"></param>
        /// <param name="action">
        ///     action expression to match elements against.
        /// </param>
        public static JquerySelectorExtend ParentsUntil(this JquerySelectorExtend original, Func<JquerySelector, JquerySelector> action)
        {
            return AddTree(original, action, "parentsUntil");
        }

        /// <summary>
        ///     Get the ancestors of each element in the current set of matched elements, up to but not including the element
        ///     matched by the action, DOM node, or jQuery object
        /// </summary>
        public static JquerySelectorExtend ParentsUntil(this JquerySelectorExtend original)
        {
            return original.ParentsUntil(null);
        }

        #endregion

        #region Prev

        /// <summary>
        ///     Get the immediately preceding sibling of each element in the set of matched elements, optionally filtered by a
        ///     action.
        /// </summary>
        /// <param name="original"></param>
        /// <param name="action">
        ///     action expression to match elements against.
        /// </param>
        public static JquerySelectorExtend Prev(this JquerySelectorExtend original, Func<JquerySelector, JquerySelector> action)
        {
            return AddTree(original, action, "prev");
        }

        /// <summary>
        ///     Get the immediately preceding sibling of each element in the set of matched elements, optionally filtered by a
        ///     action.
        /// </summary>
        public static JquerySelectorExtend Prev(this JquerySelectorExtend original)
        {
            return original.Prev(null);
        }

        /// <summary>
        ///     Get all preceding siblings of each element in the set of matched elements, optionally filtered by a action.
        /// </summary>
        /// <param name="original"></param>
        /// <param name="action">
        ///     action expression to match elements against.
        /// </param>
        public static JquerySelectorExtend PrevAll(this JquerySelectorExtend original, Func<JquerySelector, JquerySelector> action)
        {
            return AddTree(original, action, "prevAll");
        }

        /// <summary>
        ///     Get all preceding siblings of each element in the set of matched elements, optionally filtered by a action.
        /// </summary>
        public static JquerySelectorExtend PrevAll(this JquerySelectorExtend original)
        {
            return original.PrevAll(null);
        }

        /// <summary>
        ///     Get all preceding siblings of each element up to but not including the element matched by the action, DOM node, or
        ///     jQuery object.
        /// </summary>
        /// <param name="original"></param>
        /// <param name="action">
        ///     Action expression to match elements against.
        /// </param>
        public static JquerySelectorExtend PrevUntil(this JquerySelectorExtend original, Func<JquerySelector, JquerySelector> action)
        {
            return AddTree(original, action, "prevUntil");
        }

        /// <summary>
        ///     Get all preceding siblings of each element up to but not including the element matched by the action, DOM node, or
        ///     jQuery object.
        /// </summary>
        public static JquerySelectorExtend PrevUntil(this JquerySelectorExtend original)
        {
            return original.PrevUntil(null);
        }

        #endregion

        #region Siblings

        /// <summary>
        ///     Get the siblings of each element in the set of matched elements, optionally filtered by a action
        /// </summary>
        /// <param name="original"></param>
        /// <param name="action">
        ///     Action expression to match elements against.
        /// </param>
        public static JquerySelectorExtend Siblings(this JquerySelectorExtend original, Func<JquerySelector, JquerySelector> action)
        {
            return AddTree(original, action, "siblings");
        }


        /// <summary>
        ///     Get the siblings of each element in the set of matched elements, optionally filtered by a action
        /// </summary>
        /// <param name="original"></param>
        /// <param name="tag">
        ///     Html tag
        /// </param>
        public static JquerySelectorExtend Siblings(this JquerySelectorExtend original, HtmlTag tag)
        {
           return original.Siblings(r => r.Tag(tag));
        }   
        
        /// <summary>
        ///     Get the siblings of each element in the set of matched elements, optionally filtered by a action
        /// </summary>
        /// <param name="original"></param>
        /// <param name="bOfClass">
        ///     Bootstrap class
        /// </param>
        public static JquerySelectorExtend Siblings(this JquerySelectorExtend original, B bOfClass)
        {
           return original.Siblings(r => r.Class(bOfClass));
        }

        /// <summary>
        ///     Get the siblings of each element in the set of matched elements, optionally filtered by a action
        /// </summary>
        public static JquerySelectorExtend Siblings(this JquerySelectorExtend original)
        {
            return original.Siblings(null);
        }

        #endregion

        #region Slice

        /// <summary>
        ///     Reduce the set of matched elements to a subset specified by a range of indices.
        /// </summary>
        /// <param name="original"></param>
        /// <param name="start">
        ///     An integer indicating the 0-based position at which the elements begin to be selected. If negative, it indicates an
        ///     offset from the end of the set.
        /// </param>
        /// <param name="end">
        ///     An integer indicating the 0-based position at which the elements stop being selected. If negative, it indicates an
        ///     offset from the end of the set. If omitted, the range continues until the end of the set.
        /// </param>
        public static JquerySelectorExtend Slice(this JquerySelectorExtend original, Selector start, Selector end = null)
        {
            return original.Method("slice", new object[] { start, end ?? 0 });
        }

        #endregion

        static JquerySelectorExtend AddTree(JquerySelectorExtend original, Func<JquerySelector, JquerySelector> action, string type)
        {
            if (action == null)
                original.AddMethod("{0}".F(type));
            else
            {
                var selector = action(Selector.Jquery);
                if (selector.IsSimple)
                    original.AddMethod(type, selector.ToSelector());
                else
                    original.AddMethod(type, selector);
            }

            return new JquerySelectorExtend(original);
        }
    }
}