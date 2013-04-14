namespace Incoding.Data
{
    #region << Using >>

    using NHibernate.Engine;
    using NHibernate.Id;

    #endregion

    ////ncrunch: no coverage start
    public class GuidStringGenerator : IIdentifierGenerator
    {
        #region IIdentifierGenerator Members

        public object Generate(ISessionImplementor session, object obj)
        {
            return new GuidCombGenerator().Generate(session, obj).ToString();
        }

        #endregion
    }

    ////ncrunch: no coverage end
}