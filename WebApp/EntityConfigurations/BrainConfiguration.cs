﻿using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using WebApp.Models;

namespace WebApp.EntityConfigurations
{
    public class BrainConfiguration : EntityTypeConfiguration<Brain>
    {
        public BrainConfiguration()
        {
        }
    }
}