using System;
using Machine.Specifications;

namespace Incoding.MSpecContrib
{
    public class InternalSpecificationException : SpecificationException
    {
        public InternalSpecificationException()
        {
        }

        public InternalSpecificationException(string message) : base(message)
        {
        }

        public InternalSpecificationException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}