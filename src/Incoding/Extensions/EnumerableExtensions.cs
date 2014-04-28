namespace Incoding.Extensions
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    #endregion

    public static class EnumerableExtensions
    {
        #region Factory constructors

        public static IEnumerable<T> Unique<T>(this IEnumerable<T> source, IEnumerable<T> dest)
        {
            var sourceAsList = source.ToList();

            foreach (var canDuplicate in dest)
                sourceAsList.Remove(canDuplicate);

            return sourceAsList.AsEnumerable();
        }

        public static IEnumerable<T> Merger<T>(this IEnumerable<T> source, IEnumerable<T> dest)
        {
            var sourceAsList = source.ToList();
            sourceAsList.AddRange(dest.Where(item => !sourceAsList.Contains(item)));
            return sourceAsList.AsEnumerable();
        }

        public static IEnumerable<TEntity> Page<TEntity>(this IEnumerable<TEntity> source, int currentPage, int pageSize)
        {
            return source.AsQueryable().Page(currentPage, pageSize);
        }

        public static IQueryable<TEntity> Page<TEntity>(this IQueryable<TEntity> source, int currentPage, int pageSize)
        {
            currentPage = currentPage == 0 ? 1 : currentPage;
            return currentPage == 1
                           ? source.Take(pageSize)
                           : source.Skip((currentPage - 1) * pageSize).Take(pageSize);
        }

        public static string ToCsv<T>(this IEnumerable<T> items)
        {
            var itemType = typeof(T);
            var props = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                .Where(r => r.Name != "Id")
                                .OrderBy(p => p.Name);

            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine(string.Join(", ", props.Select(p => p.Name)));

            foreach (var item in items)
                stringBuilder.AppendLine(string.Join(", ", props.Select(p => p.GetValue(item, null).ToString().Replace(",", " "))));

            return stringBuilder.ToString();
        }

        #endregion
    }
}