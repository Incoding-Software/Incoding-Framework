namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using System.Web.Mvc;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Moq;

    #endregion

    public class Context_dispatcher_controller_render : Context_dispatcher_controller
    {
        #region Establish value

        protected static Mock<IView> view;

        protected static Mock<IViewEngine> viewEngines;

        #endregion

        #region Fields

        Establish establish = () =>
                                  {
                                      Establish(new Type[0]);

                                      view = Pleasure.Mock<IView>();
                                      viewEngines = Pleasure.Mock<IViewEngine>();
                                      viewEngines.Setup(r => r.FindPartialView(Pleasure.MockIt.IsAny<ControllerContext>(), Pleasure.MockIt.IsAny<string>(), Pleasure.MockIt.IsAny<bool>())).Returns(new ViewEngineResult(view.Object, viewEngines.Object));
                                      ViewEngines.Engines.Clear();
                                      ViewEngines.Engines.Add(viewEngines.Object);
                                  };

        #endregion
    }
}