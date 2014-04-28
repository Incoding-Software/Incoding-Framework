namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using FluentAutomation;
    using FluentAutomation.Interfaces;
    using Machine.Specifications;

    #endregion

    public class FluentAct : FluentTest
    {
        #region Constructors

        public FluentAct()
        {
            SeleniumWebDriver.Bootstrap(SeleniumWebDriver.Browser.PhantomJs);
        }

        #endregion

        #region Api Methods

        public void Run(Action<INativeActionSyntaxProvider> i)
        {
            i(I);
        }

        #endregion
    }

    [Ignore]
    public class When_auto_inc : FluentTest
    {
        It should_be_inc_271 = () => new FluentAct().Run(i => i.Open("http://localhost:64225/Labs/Inc_271")
                                                               .Wait(1)
                                                               .Expect
                                                               .Class("success").On(".success"));
    }
}