namespace Incoding.CQRS
{
    using System;

    public static class IncStructureResponseExtensions
    {
        public static DateTime GetValueOrDefault(this IncDateTimeResponse response, DateTime def)
        {
            if (response == null)
                return def;
            return response.Value.GetValueOrDefault(def);
        }

        public static DateTime GetValueOrDefault(this IncDateTimeResponse response)
        {
            if (response == null)
                return default(DateTime);
            return response.Value.GetValueOrDefault();
        }
    }
}