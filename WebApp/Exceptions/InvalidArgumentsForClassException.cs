using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Exceptions
{
    public class InvalidArgumentsForClassException : Exception
    {
        public InvalidArgumentsForClassException(Type type) : base(_MyExceptionMessages.InvalidArgumentsForClass(type))
        {
        }
    }
}