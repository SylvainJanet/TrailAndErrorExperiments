using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class Color : BaseEntity
    {
        public string Name { get; set; }

        public Color(string name)
        {
            Name = name;
        }

        public Color()
        {
        }
    }
}