namespace Incoding.MvcContrib
{
    using System;
    using System.Reflection;
    using System.Web.Routing;
    using Incoding.Extensions;

    public static class SelectorMapEx
    {
        public static RouteValueDictionary MapAsNames<T>(this T target) where T : class, new()
        {
            return target.MapAs(s => s.ToName());
        }

        public static RouteValueDictionary MapAs<T>(this T target, Func<string, Selector> getSelector) where T : class, new()
        {
            var res = new RouteValueDictionary();
            foreach (var propertyInfo in target.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                res.Add(propertyInfo.Name, getSelector(propertyInfo.Name));
            return res;
        }

        public static RouteValueDictionary MapAsQueryString<T>(this T target) where T : class, new()
        {
            return target.MapAs(s => Selector.Incoding.QueryString(s));
        }

        public static RouteValueDictionary MapAsHash<T>(this T target) where T : class, new()
        {
            return target.MapAs(s => Selector.Incoding.HashQueryString(s));
        }
    }
}