namespace Incoding.Block
{
    using System;

    public class OptionOfDelayAttribute : Attribute
    {
        public bool Async { get; set; }

        
        public int TimeOutOfMillisecond { get; set; }
    }
}