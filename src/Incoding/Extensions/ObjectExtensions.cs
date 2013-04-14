namespace Incoding.Extensions
{
    #region << Using >>

    using Newtonsoft.Json;

    #endregion

    public static class ObjectExtensions
    {
        #region Factory constructors

        public static TDeserialize DeserializeFromJson<TDeserialize>(this string json)
        {
            return JsonConvert.DeserializeObject<TDeserialize>(json);
        }

        public static string ToJsonString(this object ob)
        {
            return JsonConvert.SerializeObject(ob);
        }

        #endregion
    }
}