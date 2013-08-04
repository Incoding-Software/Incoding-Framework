namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using Incoding.Block.IoC;
    using Machine.Specifications;

    #endregion

    public class Context_IoC_Factory_TestEx
    {
        #region Fields

        Establish establish = () => { IoCFactory.Instance.UnInitialize(); };

        #endregion
    }
}