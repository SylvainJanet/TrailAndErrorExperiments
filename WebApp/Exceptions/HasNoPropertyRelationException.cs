using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Exceptions
{
    public class HasNoPropertyRelationException : Exception
    {
        public HasNoPropertyRelationException(Type t1, Type t2) : base(_MyExceptionMessages.HasNoPropertyRelation(t1,t2))
        {

        }
    }
}