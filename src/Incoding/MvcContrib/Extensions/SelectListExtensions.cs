namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using Incoding.Extensions;
    using Incoding.Maybe;

    #endregion

    public static class SelectListExtensions
    {
        #region Factory constructors

        public static IEnumerable<KeyValueVm> ToKeyValueVm<TItem>(this IEnumerable<TItem> source, Func<TItem, object> value, Func<TItem, object> text)
        {
            return source.Select(item => new KeyValueVm(value(item), text(item).With(r => r.ToString())));
        }

        public static IEnumerable<KeyValueVm> ToKeyValueVm<TItem>(this IEnumerable<TItem> source, Func<TItem, object> value)
        {
            return source.ToKeyValueVm(value, value);
        }

        public static IEnumerable<KeyValueVm> ToKeyValueVm(this Type typeEnum, Enum selectedValue = null)
        {
            Guard.IsConditional("typeEnum", typeEnum.IsEnum, "Type should be Enum");

            var items = Enum
                    .GetValues(typeEnum)
                    .Cast<Enum>()
                    .Select(@enum => new KeyValueVm(@enum, @enum.ToLocalization(), @enum.ToString("d") == selectedValue.With(r => r.ToString("d"))))
                    .ToList();
            return items;
        }

        public static IEnumerable<KeyValueVm> ToKeyValues(this IEnumerable<SelectListItem> list)
        {
            return list.Select(r => new KeyValueVm(r.Value, r.Text, r.Selected));
        }

        public static OptGroupVm ToOptGroup(this IEnumerable<KeyValueVm> source, string title = "")
        {
            return new OptGroupVm(title, source);
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

        public static SelectList ToSelectList(this Type typeEnum, Enum selectedValue = null, KeyValueVm defaultOption = null)
        {
            return ItemToSelectList(typeEnum.ToKeyValueVm(), "Value", "Text", selectedValue.With(r => r.ToString()), defaultOption);
        }

        #endregion

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