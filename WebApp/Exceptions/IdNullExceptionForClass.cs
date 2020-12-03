using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Exceptions
{
    public class IdNullExceptionForClass : Exception
    {
        public IdNullExceptionForClass(Type t) : base(_MyExceptionMessages.IdNullForClass(t))
        {

        }
    }
}