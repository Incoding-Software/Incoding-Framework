namespace Incoding.SiteTest
{
    #region << Using >>

    using Incoding.CQRS;
    using Incoding.Extensions;

    #endregion

    public abstract class SampleOfCQRS
    {

        public class Command:CommandBase
        {
            public string Value { get; set; }

            protected override void Execute()
            {
                Result = Value;
            }
        }

        public class MultipleGenericCommand<T, T2, T3> : CommandBase
        {
            protected override void Execute()
            {
                Result = "{0},{1},{2}".F(typeof(T).Name, typeof(T2).Name, typeof(T3).Name);
            }
        }
    }
}