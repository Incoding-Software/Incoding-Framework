namespace Incoding.Block
{
    using System;

    public class OptionOfDelayAttribute : Attribute
    {
        public bool Async { get; set; }

        public int TimeOut { get; set; }
    }
}