namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(JSGeolocationSelector))]
    public class When_js_geolocation_selector
    {
        #region Estabilish value

        static JSCoordsSelector currentPosition;

        #endregion

        Because of = () =>
                         {
                             currentPosition = Selector.JS.Navigator.CurrentPosition;
                         };

        It should_be_latitude = () => currentPosition.Latitude
                                                     .ToString()
                                                     .ShouldEqual("@@javascript:navigator.geolocation.currentPosition.latitude@@");

        It should_be_longitude = () => currentPosition.Longitude
                                                      .ToString()
                                                      .ShouldEqual("@@javascript:navigator.geolocation.currentPosition.longitude@@");

        It should_be_accuracy = () => currentPosition.Accuracy
                                                     .ToString()
                                                     .ShouldEqual("@@javascript:navigator.geolocation.currentPosition.accuracy@@");

        It should_be_altitude_accuracy = () => currentPosition.AltitudeAccuracy
                                                              .ToString()
                                                              .ShouldEqual("@@javascript:navigator.geolocation.currentPosition.altitudeAccuracy@@");

        It should_be_altitude_heading = () => currentPosition.Heading
                                                             .ToString()
                                                             .ShouldEqual("@@javascript:navigator.geolocation.currentPosition.heading@@");

        It should_be_altitude_speed = () => currentPosition.Speed
                                                           .ToString()
                                                           .ShouldEqual("@@javascript:navigator.geolocation.currentPosition.speed@@");
    }
}