﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Exceptions
{
    public class PropertyNameNotFoundException : Exception
    {
        public PropertyNameNotFoundException(Type type, string nameNotFound) : base(_MyExceptionMessages.PropertyNameNotFoundForClass(type, nameNotFound))
        {

        }
    }
}