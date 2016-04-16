namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using System.Collections.Specialized;
    using System.IO;
    using System.Web.Mvc;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(DispatcherControllerBase))]
    public class When_base_dispatcher_controller_typical_model : Context_dispatcher_controller_render
    {
        It should_be_bool = () =>
                            {
                                bool value = Pleasure.Generator.Bool();
                                requestBase.SetupGet(r => r.Params).Returns(new NameValueCollection() { { "incValue", value.ToString() } });
                                dispatcher.StubQuery(Pleasure.Generator.Invent<CreateByTypeQuery>(dsl => dsl.Tuning(r => r.Type, typeof(bool).Name)
                                                                                                            .Tuning(r => r.IsModel, true)), (object)value);
                                controller.Render(partialViewName, typeof(bool).Name, true, false);
                                Action<ViewContext> verify = s => s.ViewData.Model.ShouldEqual(value);
                                view.Verify(r => r.Render(Pleasure.MockIt.Is(verify), Pleasure.MockIt.IsAny<TextWriter>()));
                            };

        It should_be_date_time = () =>
                                 {
                                     var value = Pleasure.Generator.DateTime();
                                     requestBase.SetupGet(r => r.Params).Returns(new NameValueCollection() { { "incValue", value.ToString() } });
                                     dispatcher.StubQuery(Pleasure.Generator.Invent<CreateByTypeQuery>(dsl => dsl.Tuning(r => r.Type, typeof(DateTime).Name)
                                                                                                                 .Tuning(r => r.IsModel, true)), (object)value);
                                     controller.Render(partialViewName, typeof(DateTime).Name, true, false);
                                     Action<ViewContext> verify = s => s.ViewData.Model.ShouldEqual(value);
                                     view.Verify(r => r.Render(Pleasure.MockIt.Is(verify), Pleasure.MockIt.IsAny<TextWriter>()));
                                 };

        It should_be_enum = () =>
                            {
                                var value = Pleasure.Generator.Enum<FakeEnum>();
                                requestBase.SetupGet(r => r.Params).Returns(new NameValueCollection() { { "incValue", value.ToString() } });
                                dispatcher.StubQuery(Pleasure.Generator.Invent<CreateByTypeQuery>(dsl => dsl.Tuning(r => r.Type, typeof(FakeEnum).Name)
                                                                                                            .Tuning(r => r.IsModel, true)), (object)value);
                                controller.Render(partialViewName, typeof(FakeEnum).FullName, true, false);
                                Action<ViewContext> verify = s => s.ViewData.Model.ShouldEqual(value);
                                view.Verify(r => r.Render(Pleasure.MockIt.Is(verify), Pleasure.MockIt.IsAny<TextWriter>()));
                            };

        It should_be_int = () =>
                           {
                               int value = Pleasure.Generator.PositiveNumber();
                               requestBase.SetupGet(r => r.Params).Returns(new NameValueCollection() { { "incValue", value.ToString() } });
                               dispatcher.StubQuery(Pleasure.Generator.Invent<CreateByTypeQuery>(dsl => dsl.Tuning(r => r.Type, typeof(int).Name)
                                                                                                           .Tuning(r => r.IsModel, true)), (object)value);
                               controller.Render(partialViewName, typeof(int).Name, true, false);
                               Action<ViewContext> verify = s => s.ViewData.Model.ShouldEqual(value);
                               view.Verify(r => r.Render(Pleasure.MockIt.Is(verify), Pleasure.MockIt.IsAny<TextWriter>()));
                           };

        It should_be_string = () =>
                              {
                                  requestBase.SetupGet(r => r.Params).Returns(new NameValueCollection() { { "incValue", Pleasure.Generator.TheSameString() } });
                                  dispatcher.StubQuery(Pleasure.Generator.Invent<CreateByTypeQuery>(dsl => dsl.Tuning(r => r.Type, typeof(string).Name)
                                                                                                              .Tuning(r => r.IsModel, true)), (object)partialViewName);
                                  controller.Render(partialViewName, typeof(string).Name, true, false);
                                  Action<ViewContext> verify = s => s.ViewData.Model.ShouldEqual(Pleasure.Generator.TheSameString());
                                  view.Verify(r => r.Render(Pleasure.MockIt.Is(verify), Pleasure.MockIt.IsAny<TextWriter>()));
                              };

        #region Establish value

        public enum FakeEnum
        {
            Test,

            Test2,

            Test3
        }

        static string partialViewName = "View";

        #endregion
    }
}