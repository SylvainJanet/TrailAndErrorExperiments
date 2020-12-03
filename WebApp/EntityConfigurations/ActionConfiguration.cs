using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace WebApp.EntityConfigurations
{
    public class ActionConfiguration : EntityTypeConfiguration<Models.Action>
    {
        public ActionConfiguration()
        {
        }
    }
}