namespace Incoding.Block
{
    using System;

    public class OptionOfDelayAttribute : Attribute
    {
        public OptionOfDelayAttribute()
        {
            Async = false;
            TimeOut = 0;
        }

        public bool Async { get; set; }

        public int TimeOut { get; set; }
    }
}