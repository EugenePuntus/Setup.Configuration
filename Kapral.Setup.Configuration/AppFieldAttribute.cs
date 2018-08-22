using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kapral.Setup.Configuration
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class AppFieldAttribute : Attribute
    {
        public AppFieldAttribute() { }

        public AppFieldAttribute(string name) : this()
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
