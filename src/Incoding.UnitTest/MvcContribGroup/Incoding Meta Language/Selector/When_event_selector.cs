namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(EventSelector))]
    public class When_event_selector
    {
        
        It should_be_meta_key = () => Selector.Event.MetaKey
                                              .ToString()
                                              .ShouldEqual("||javascript*this.event.metaKey||");

        It should_be_multiple_t_data = () => Selector.Event
                                                     .Data<FakeEvent>(@event => @event.Inner.Name)
                                                     .ToString()
                                                     .ShouldEqual("||javascript*this.event.Inner.Name||");

        It should_be_page_x = () => Selector.Event
                                            .PageX
                                            .ToString()
                                            .ShouldEqual("||javascript*this.event.pageX||");

        It should_be_page_y = () => Selector.Event
                                            .PageY
                                            .ToString()
                                            .ShouldEqual("||javascript*this.event.pageY||");

        It should_be_screen_x = () => Selector.Event
                                              .ScreenX
                                              .ToString()
                                              .ShouldEqual("||javascript*this.event.screenX||");

        It should_be_screen_y = () => Selector.Event
                                              .ScreenY
                                              .ToString()
                                              .ShouldEqual("||javascript*this.event.screenY||");

        It should_be_t_data = () => Selector.Event
                                            .Data<FakeEvent>(@event => @event.Name)
                                            .ToString()
                                            .ShouldEqual("||javascript*this.event.Name||");

        It should_be_type = () => Selector.Event.Type
                                          .ToString()
                                          .ShouldEqual("||javascript*this.event.type||");

        It should_be_which = () => Selector.Event.Which
                                           .ToString()
                                           .ShouldEqual("||javascript*this.event.which||");

        class FakeEvent
        {
            public string Name { get; set; }

            public FakeEvent Inner { get; set; }
        }
    }
}