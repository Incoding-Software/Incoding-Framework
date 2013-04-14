namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Linq;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;

    #endregion

    public static class IncodingMetaLanguageDslHelper
    {
        #region Factory constructors

        public static IEnumerable<T> GetActions<T>(this IIncodingMetaLanguageEventBuilderDsl meta)
        {
            var merges = (List<ExecutableBase>)meta
                                                       .TryGetValue("meta")
                                                       .TryGetValue("merges");

            return merges.OfType<T>();
        }

        public static T GetExecutable<T>(this IIncodingMetaLanguageEventBuilderDsl meta)
        {
            return meta.GetActions<T>().FirstOrDefault();
        }

        public static void ShouldEqualData(this ExecutableBase executable, Dictionary<string, object> data)
        {
            var ignore = new[] { "onBind", "target", "onStatus", "onEventStatus" };
            foreach (var keyValue in executable.Data)
            {
                if (ignore.Contains(keyValue.Key))
                    continue;

                data.ShouldBeKeyValue(keyValue.Key, keyValue.Value);
            }

            foreach (var keyValue in data)
                executable.Data.ShouldBeKeyValue(keyValue.Key, keyValue.Value);
        }

        #endregion
    }
}