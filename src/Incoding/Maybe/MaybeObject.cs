namespace Incoding.Maybe
{
    #region << Using >>

    using System;
    using System.Collections.Generic;

    #endregion

    public static class MaybeObject
    {
        #region Factory constructors

        public static void DoEach<TInput>(this IEnumerable<TInput> source, Action<TInput> action)
        {
            if (source == null)
                return;

            foreach (var input in source)
                action(input);
        }

        public static void Do<TSource>(this TSource source, Action<TSource> action)
                where TSource : class
        {
            if (source == null)
                return;

            action(source);
        }

        public static bool Has<TSource>(this TSource source)
                where TSource : class
        {
            return source != null;
        }

        public static TSource If<TSource>(this TSource source, Predicate<TSource> conditional)
                where TSource : class
        {
            if (source == null)
                return null;

            return conditional(source) ? source : null;
        }

        public static TSource Not<TSource>(this TSource source, Predicate<TSource> conditional)
                where TSource : class
        {
            if (source == null)
                return null;

            return !conditional(source) ? source : null;
        }

        public static TSource Recovery<TSource>(this TSource source, Func<TSource> recovery)
                where TSource : class
        {
            return source ?? recovery();
        }

        public static TSource Recovery<TSource>(this TSource source, TSource recovery)
                where TSource : class
        {
            return source.Recovery(() => recovery);
        }

        public static TOutput ReturnOrDefault<TSource, TOutput>(this TSource source, Func<TSource, TOutput> evaluator, TOutput defaultValue)
        {
            return source == null ? defaultValue : evaluator(source);
        }

        public static TOutput ReturnOrDefault<TSource, TOutput>(this TSource source, TOutput value, TOutput defaultValue)
        {
            return source.ReturnOrDefault(r => value, defaultValue);
        }

        public static TOutput ReturnOrThrows<TSource, TOutput>(this TSource source, Func<TSource, TOutput> evaluator, Exception exception)
                where TSource : class
        {
            if (source == null)
                throw exception;

            return evaluator(source);
        }

        public static TSource Then<TSource>(this TSource source)
        {
            return source;
        }

        public static TOutput With<TSource, TOutput>(this TSource source, Func<TSource, TOutput> evaluator)
                where TSource : class
        {
            return source == null
                           ? default(TOutput)
                           : evaluator(source);
        }

        #endregion
    }
}