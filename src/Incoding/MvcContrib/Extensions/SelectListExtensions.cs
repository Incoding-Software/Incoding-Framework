namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using Incoding.Extensions;

    #endregion

    public static class SelectListExtensions
    {
        #region Factory constructors

        public static SelectList ToSelectList(this Type @enum)
        {
            return ToSelectList(@enum, null, null);
        }

        public static SelectList ToSelectList(this Type @enum, int selectedValue, string defaultOption = null)
        {
            return ToSelectList(@enum, selectedValue, !string.IsNullOrWhiteSpace(defaultOption) ? new KeyValueVm(defaultOption, defaultOption) : null);
        }

        public static SelectList ToSelectList<TItem>(this IEnumerable<TItem> source, Expression<Func<TItem, object>> value, Expression<Func<TItem, object>> text, object selected, TItem defaultOption = null)
                where TItem : class, new()
        {
            Guard.NotNull("value", value);
            Guard.NotNull("text", text);
            Guard.NotNull("selected", selected);

            return ItemToSelectList(source, value.GetMemberName(), text.GetMemberName(), selected.ToString(), defaultOption);
        }

        public static SelectList ToSelectList<TItem>(this IEnumerable<TItem> source, Expression<Func<TItem, object>> value, Expression<Func<TItem, object>> text)
        {
            return ItemToSelectList(source, value.GetMemberName(), text.GetMemberName(), null, null);
        }

        public static SelectList ToSelectList(this IEnumerable<IKeyValueVm> source)
        {
            Guard.NotNull("source", source);

            const string value = "Value";
            const string text = "Text";

            if (!source.Any())
                return ItemToSelectList(new List<IKeyValueVm>(), value, text, string.Empty, null);

            var itemSelected = source.FirstOrDefault(r => r.Selected);
            string selectedValue = (itemSelected != null) ? itemSelected.Value : string.Empty;
            return ItemToSelectList(source, value, text, selectedValue, null);
        }

        #endregion

        static SelectList ToSelectList(Type typeEnum, int? selectedValue, KeyValueVm defaultOption)
        {
            var enumCollection = Enum
                    .GetValues(typeEnum)
                    .Cast<Enum>()
                    .Select(r => new KeyValueVm(r.ToString("d"), r.ToLocalization(), selectedValue.HasValue));
            return ItemToSelectList(enumCollection, "Value", "Text", selectedValue.HasValue ? selectedValue.ToString() : string.Empty, defaultOption);
        }

        static SelectList ItemToSelectList<TItem>(IEnumerable<TItem> source, string value, string text, string selectedValue, object defaultValue)
        {
            var items = source.Cast<object>().ToList();
            if (defaultValue != null)
                items.Insert(0, defaultValue);

            return string.IsNullOrWhiteSpace(selectedValue)
                           ? new SelectList(items, value, text)
                           : new SelectList(items, value, text, selectedValue);
        }
    }
}